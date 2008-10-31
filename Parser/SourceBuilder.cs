﻿using System;
using System.Reflection;
using Parser.BuiltIn.Function;
using Parser.Factory;
using Parser.Model;
using Parser.Syntax;

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
			AddBuiltInMembers();
		}

		/// <summary>
		/// Добавляет встроенные функции.
		/// </summary>
		private void AddBuiltInMembers()
		{
			Assembly assembly = Assembly.GetExecutingAssembly();
			AddBuiltInMember("Parser.BuiltIn.Function.Eval", assembly);
			AddBuiltInMember("Parser.BuiltIn.Function.ExcelTableCreate", assembly);
		}

		private void AddBuiltInMember(string namespaceName, Assembly assembly){
			// TODO ниже временная запись
			// будет добавлено добавление статичных объектов по неймспейсу
			Function builtInFunc = new Function();
			builtInFunc.Parent = rootNode;
			builtInFunc.Parameters = new Params();
			builtInFunc.Parameters.Names = new string[]{"0"}; // TODO ?
			
			Type parserType = assembly
				.GetType(String.Format(namespaceName)); // берем нужный тип Assembly

			ConstructorInfo ci = parserType.GetConstructor(new Type[0]);

			// TODO в теории ParserNameAttribute может быть не нулевым и аттрибута вообще может не быть
			ParserNameAttribute pna = 
				(ParserNameAttribute)parserType.GetCustomAttributes(false)[0];

			builtInFunc.SetName(pna.Name);
			builtInFunc.RefObject = ci;
			// добавляем в дерево
			rootNode.Add(builtInFunc);
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

				// если объявление чего-нибудь
				if (IsDeclarationChar)
				{
					index = TryCreateNode(index, c, ref node);
				}
				c = source[index];
				// если конец параметра
				if (
						 c == CharsInfo.ParamsEnd
					|| c == CharsInfo.ParamsEvalEnd
					|| c == CharsInfo.ParamsCodeEnd)
				{
					node = GoDown(index, node);
				}
				// если в параметре и ';'
				if(IsInParametr && c == CharsInfo.ParametrSeparator)
				{
					node = SplitParametr(index, node);
				}
				// если не в текстовой ноде
				if (!isInTextNode)
				{
					CreateText(node);
				}
				// если в конце строки
				if (index == lastCharIndex)
				{
					currentText.Body = source.Substring(currentText.Start.Value); // TODO прерывание
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
			isInTextNode = false; // подготовим открытие текстовой ноды
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
			isInTextNode = false;
			if(IsInParametr)
			{
				node = node.Parent; // спускаемся из параметра
				IsInParametr = false; // закрываем параметр
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
				isInTextNode = false;
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
				return;
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
			if (length < 0)
			{
				//currentText.Body = String.Empty;
				// TODO для оптимизации, такая текстовая нода должна быть удалена.
			}
			else
			{
				currentText.Body = source.Substring(currentText.Start.Value, length);
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

		#endregion 
		 
	}
}
