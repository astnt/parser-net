using System;
using System.Reflection;
using Parser.BuiltIn.Function;
using Parser.Factory;
using Parser.Model;
using Parser.Syntax;
using Parser.Util;

namespace Parser
{
	/// <summary>
	/// Main code builder.
	/// </summary>
	public class SourceBuilder
	{
		/// <summary>
		/// В параметре или нет.
		/// </summary>
		public bool IsInParametr = false;
		/// <summary>
		/// В заэскейплинном месте.
		/// ^^something.in.escape 
		/// По-идее разрывается пробельным или еще каким символом.
		/// </summary>
		public bool IsInEscape = false;
		/// <summary>
		/// Start of building source code.
		/// </summary>
		/// <param name="Source">String with source code.</param>
		public void Parse(string Source)
		{
			// записываем для того чтобы не передавать параметром
			source = Source;
			// создаем корневую ноду
			rootNode = new RootNode();
			// задаем начало строки
			CurrentIndex = 0;
			// чтобы не передавить параметром укажем SourceBuilder
			factory.Sb = this;
			Parse(rootNode);
			BuiltInUtil bi = new BuiltInUtil();
			bi.RootNode = rootNode;
			bi.AddBuiltInMembers();
		}
		/// <summary>
		/// Обратываем ноду.
		/// </summary>
		/// <param name="node">Функция, текстовая нода или что-то еще.</param>
		public void Parse(Node node)
		{
			int lastCharIndex = source.Length - 1;

			for (int index = CurrentIndex.Value; index < source.Length; index += 1)
			{
				// задаем индексы и char
				char c = source[index];
				CurrentIndex = index;

				bool IsDeclarationChar =
					(	 c == CharsInfo.FuncDeclarationStart
					|| c == CharsInfo.CallerDeclarationStart
					|| c == CharsInfo.VariableDeclarationStart);
				// [^]^
				// [^]$
				if(c == CharsInfo.CallerDeclarationStart &&
						(
							source[index+1] == CharsInfo.CallerDeclarationStart
							|| source[index+1] == CharsInfo.VariableDeclarationStart
						)
					)
				{
					CloseCurrentText(index-1);
					IsInEscape = true;
					continue;
				}
				// ^[^]
				// ^[$]
				if (
					(
						c == CharsInfo.CallerDeclarationStart
						|| c == CharsInfo.VariableDeclarationStart
					)
					&& source[index-1] == CharsInfo.CallerDeclarationStart)
				{
					CloseCurrentText(index - 1); // ^$var[] -> $var вместо ^var в текст
					CurrentIndex -= 1; // для CreateText
					IsInEscape = true;
					IsDeclarationChar = false;
				}
				// если объявление чего-нибудь, если синтаксически верно
				if (IsDeclarationChar)
				{
					index = TryCreateNode(index, c, ref node);
				}
				c = source[index];
				// если в параметре и ';', то разбиваем на новый параметр
				if (IsInParametr && c == CharsInfo.ParametrSeparator)
				{
					node = SplitParametr(index, node);
				}
				// если конец параметра ])}, то спуск вниз
				if (!IsInEscape && CharsInfo.IsInParamsEndChars(c))
				{
					if(source.Length > index + 1 && source[index + 1] == CharsInfo.ParamsCodeStart)
					{
						CurrentIndex = index += 1;
						node = SplitParametr(index, node);
					}
					else
					{
						node = GoDown(index, node);
					}
				}
				// если не в текстовой ноде, то создаем
				if (!isInTextNode)
				{
					CreateText(node);
				}
				// если в конце строки "закрывам текстовую ноду"
				if (index == lastCharIndex)
				{
					currentText.Body = source.Substring(currentText.Start.Value); // TODO прерывание
				}
				// если пора заканчивать заэскейпливание
				if (c == ' ' || c == (char)160 || CharsInfo.IsInParamsEndChars(c)) // TODO нужно добавить другие символы, вынести в фунцкию
				{
					IsInEscape = false;
				}
			}
		}
		/// <summary>
		/// Разбиваем параметры.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="node"></param>
		/// <returns></returns>
		private Node SplitParametr(int index, Node node)
		{
			CloseCurrentText(index); // закрываем текущую текстовую ноду
			node = node.Parent; // спускаемся вниз
			Node parametr = new Parametr();
			node.Add(parametr); // добавляем новый
			node = parametr; // перемещаем указатель на него
			return node;
		}
		/// <summary>
		/// Спускаемся ниже по дереву, если наткнулись на закрытие параметра.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="node"></param>
		/// <returns></returns>
		private Node GoDown(int index, Node node)
		{
			CloseCurrentText(index);
			if(IsInParametr) // TODO наверно можно понять по родителю?
			{
				node = node.Parent; // спускаемся из параметра
				IsInParametr = false; // закрываем параметр
			}
			if(node as Parametr != null)
			{
				node = node.Parent;
			}
			// если не корень, спускаемся на ноду ниже
			if (node.Parent as RootNode == null)
			{
				node = node.Parent;
			}
			return node;
		}
		/// <summary>
		/// Пытаемся объявить ноду, если синтаксически соответствует.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="c"></param>
		/// <param name="node"></param>
		/// <returns></returns>
		private int TryCreateNode(int index, char c, ref Node node)
		{
			AbstractNode createdNode;
			// создаем ноды
			if(factory.CreateNode(c, node, out createdNode))
			{
				// новая нода создана, закрываем текст
				CloseCurrentText(index);
				// смещаем на отпарсенный позицию
				index = CurrentIndex.Value;
				c = source[index]; // update
				// выбираем, куда переместить по дереву созданную ноду
				Function func = createdNode as Function;
				Variable var = createdNode as Variable;
				Caller caller = createdNode as Caller;
				bool isAdded = false;
				if(func != null)
				{
					// если объявление функции (@something[...][...])
					// перемещаем указатель парсинга на нее
					node = (Node) createdNode;
					// добавляем в корневую ноду
					rootNode.Add(node);
					node = InParametr(node); // дальше, вплоть до ']' идут параметры

					isAdded = true;
				}
				if(var != null || caller != null)
				{
					node.Add(createdNode);
					// чтобы не каствать два раза
					node = (Node)createdNode;
					node = InParametr(node);
					isAdded = true;
				}
				if(!isAdded)
				{
					// для остальных добавляем в текущую ноду.
					node.Add(createdNode);
				}
			}
			return index;
		}
		/// <summary>
		/// Перемещаемся в параметр.
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		private Node InParametr(Node node)
		{
			Node parametr = new Parametr();
			node.Add(parametr);
			// передаем управление параметру
			node = parametr;
			IsInParametr = true;
			return node;
		}
		/// <summary>
		/// Пытаемся создать текствую ноду.
		/// </summary>
		/// <param name="node"></param>
		private void CreateText(Node node)
		{
			currentText = new Text();
			currentText.Start = CurrentIndex + 1;
			if (source.Length <= currentText.Start.Value)
			{
				return; // HACK
			}
			currentText.Parent = node;
			node.Add(currentText);
			isInTextNode = true;
		}
		/// <summary>
		/// Закрываем текущую текстовую ноду.
		/// </summary>
		/// <param name="index"></param>
		private void CloseCurrentText(int index)
		{
			if(currentText == null)
			{
				return; // HACK
			}
			int length = index - currentText.Start.Value;
			if (length <= 0)
			{
				//currentText.Body = String.Empty;
				// TODO для оптимизации, такая текстовая нода должна быть удалена.
			}
			else
			{
				currentText.Body = source.Substring(currentText.Start.Value, length);
			}
			isInTextNode = false; // HACK по-идее нода должны быть именно закрыта
		}

		#region Vars

		private Text currentText;

		private Node rootNode;

		private Boolean isInTextNode = false;

		public int? CurrentIndex;
		public string source;

		private NodeFactory factory = new NodeFactory();

		public Node RootNode
		{
			get { return rootNode; }
			set { rootNode = value; }
		}

		#endregion 
		 
	}
}
