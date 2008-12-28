using System;
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
//				Console.WriteLine("char'{0}'", c);
				// если символ соответствует одному из [e]q, [=]=, [&]&, [|]| и т.п.
				if(Expressions.expressionFirstChar.ContainsKey(c))
				{
					// ести родитель ^if()
					if ((node.Parent as Caller) != null && ((Caller)node.Parent).Name[0] == IfCondition.SyntaxName)
					{
						String expression = Expressions.expressionFirstChar[c];
						if (IsExpression(index, expression))
						{
							CloseCurrentText(index);
							RemoveTextNodeIfNotInExpression(node);
							Operator op = new Operator(expression);
							node.Add(op);
							index += expression.Length - 1;
							CurrentIndex = index;
							continue;
						}
					}
				}

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
					CloseCurrentText(index - 1);
					IsInEscape = true;
					continue;
				}
				// ^[^]
				// ^[$]
				if (
					(
						c == CharsInfo.CallerDeclarationStart
						|| c == CharsInfo.VariableDeclarationStart
						|| CharsInfo.IsInParamsEndChars(c)
//						(index == 0 || source[index - 1] != CharsInfo.CallerDeclarationStart)
					)
					&& source[index - 1] == CharsInfo.CallerDeclarationStart)
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
				if (!IsInEscape
//					&& /* TODO move to boolean */ (index == 0 || source[index - 1] != CharsInfo.CallerDeclarationStart)
					&& CharsInfo.IsInParamsEndChars(c))
				{
//					CloseCurrentText(index - 1);
					if(source.Length > index + 1 && source[index + 1] == CharsInfo.ParamsCodeStart)
					{
						node = SplitParametr(index, node);
						CurrentIndex = index += 1;
					}
					else
					{
//						CurrentIndex = index -= 1;
//						Console.WriteLine(node);
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
					CurrentText.Body = source.Substring(CurrentText.Start.Value); // TODO прерывание
				}
				// если пора заканчивать заэскейпливание
				if (c == ' ' || c == (char)160 || CharsInfo.IsInParamsEndChars(c)) // TODO нужно добавить другие символы, вынести в фунцкию
				{
					IsInEscape = false;
				}
			}
		}
		/// <summary>
		/// Удалить лишнюю текстовую ноду, если не входит в boolean-выражение.
		/// </summary>
		/// <param name="node"></param>
		private void RemoveTextNodeIfNotInExpression(Node node)
		{
			String valueOfTextNode = currentText.Body.Trim();
			if (String.IsNullOrEmpty(valueOfTextNode)
				||
				// проверяем не соотвествует ли 'string value', то есть [']string value[']
				(valueOfTextNode[0] != CharsInfo.StringInExpressionDeclaration
				&& valueOfTextNode[valueOfTextNode.Length-1] != CharsInfo.StringInExpressionDeclaration
				// если это не число
				&& !CharsInfo.IsDigit(valueOfTextNode[0])
				)
			)
			{
				node.Childs.Remove(currentText);
			}
		}

		private bool IsExpression(Int32 index, String expression)
		{
			return CharsInfo.IsInSpaceChars(source[index - 1])
				&& CharsInfo.IsInSpaceChars(source[index + expression.Length])
				&& source.Substring(index, expression.Length) == expression
				;
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
			CurrentText = new Text();
			CurrentText.Start = CurrentIndex + 1;
			if (source.Length <= CurrentText.Start.Value)
			{
				return; // HACK
			}
			CurrentText.Parent = node;
			if(node != null)
			{
				node.Add(CurrentText);
			}
			else
			{
				Console.WriteLine("WARN: node for current text is null");
			}
			isInTextNode = true;
		}
		/// <summary>
		/// Закрываем текущую текстовую ноду.
		/// </summary>
		/// <param name="index"></param>
		public void CloseCurrentText(int index)
		{
			if (CurrentText != null)
			{
				int length = index - CurrentText.Start.Value;
				if (length <= 0)
				{
					//currentText.Body = String.Empty;
					// TODO для оптимизации, такая текстовая нода должна быть удалена.
				}
				else
				{
					CurrentText.Body = source.Substring(CurrentText.Start.Value, length);
				}
				isInTextNode = false; // HACK по-идее нода должны быть именно закрыта
			}
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

		public Text CurrentText
		{
			get { return currentText; }
			set { currentText = value; }
		}

		#endregion 
		 
	}
}
