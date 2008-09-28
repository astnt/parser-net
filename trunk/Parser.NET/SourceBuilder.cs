using System;
using System.Reflection;
using Parser.Builder;
using Parser.BuiltIn.Function;
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
			Parse(rootNode);

			AddBuiltInMembers();
		}

		/// <summary>
		/// Добавить встроенные функции.
		/// </summary>
		private void AddBuiltInMembers()
		{
			// TODO ниже временная запись
			// будет добавлено добавление статичных объектов по неймспейсу
			Function builtInFunc = new Function();
			builtInFunc.SetName("eval");
			builtInFunc.Parent = rootNode;
			builtInFunc.Parameters = new Params();
			builtInFunc.Parameters.Names = new string[]{"0"}; // ?
			
			Type parserType = Assembly.GetExecutingAssembly()
				.GetType(String.Format("Parser.BuiltIn.Function.Eval")); // перебрать Assembly
			ConstructorInfo ci = parserType.GetConstructor(new Type[0]);
			builtInFunc.RefObject = ci;

			rootNode.Add(builtInFunc);
		}

		/// <summary>
		/// Обратываем ноду.
		/// </summary>
		/// <param name="node">Функция, текстовая нода или что-то еще.</param>
		public void Parse(Node node)
		{
			int lastCharIndex = source.Length - 1;
			// перебор строки исходника
			for (int index = CurrentIndex.Value; index < source.Length; index+=1)
			{
				// задаем индексы и char
				char c = source[index];
				CurrentIndex = index;

				// если старт функции - создаем ее
				if (c == CharsInfo.FuncDeclarationStart)
				{
					Node nodeOf = GetNodeToParseNext(index, node);
					CurrentIndex = index + 1;
					Parse((Function)Declaration(nodeOf, new FunctionBuilder()));
					break;
				}

				// создаем Caller
				if(c == CharsInfo.CallerDeclarationStart)
				{
					CloseCurrentText(index);
					CurrentIndex = index + 1;
					/*Caller caller = (Caller)*/ Declaration(node, new CallerBuilder());
				}

				// создаем/выполняем переменную
				if(c == CharsInfo.VariableDeclarationStart)
				{
					CloseCurrentText(index);
					CurrentIndex = index + 1;
					AbstractNode abstractNode = Declaration(node, new VariableBuilder());
					
					if (abstractNode != null) // TODO не явное  соощение о том, что переменная была создана
					{
						//continue;
						c = source[CurrentIndex.Value]; // char update
					}
				}

				// добавить вызов переменной
				if(c == CharsInfo.VariableDeclarationStart)
				{
					CloseCurrentText(index);
					CurrentIndex = index + 1;
					Declaration(node, new VariableCallBuilder());
					// TODO вызов переменной
				}

				// если не в текстовой ноде и ничего не прервало процесс - создаем ее
				if(!isInTextNode)
				{
					currentText = new Text(); // TODO наверно нужен для красивости TextBuilder?
					currentText.Start = CurrentIndex + 1;
					currentText.Parent = node;
					node.Add(currentText);
					isInTextNode = true;
				}

				// Подошли к концу файла
				if(index == lastCharIndex)
				{
					currentText.Body = source.Substring(currentText.Start.Value);
					// TODO прерывание
				}
			}
		}

		/// <summary>
		/// Выбираем ноду для обработки, как основную.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="node"></param>
		/// <returns></returns>
		private Node GetNodeToParseNext(int index, Node node)
		{
			Node nodeOf;
			if (node.Parent != null)
			{
				// Если есть родитель, то передаем управление ему
				nodeOf = node.Parent;
				CloseCurrentText(index);
			}
			else
			{
				// Если родителя нет, то это корневая нода
				nodeOf = node;
			}
			return nodeOf;
		}

		/// <summary>
		/// Закрываем текущую текстовую ноду.
		/// </summary>
		/// <param name="index"></param>
		private void CloseCurrentText(int index)
		{
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

		/// <summary>
		/// Создание объекта (функции, коллера и т.п.), переключение текста и добавление в ноду.
		/// </summary>
		/// <param name="node">Текущая нода куда будет добавляться созданная функция.</param>
		/// <param name="builder"></param>
		private AbstractNode Declaration(Node node, Builder.Builder builder)
		{
			if (builder.Build(this, source))
			{
				// добавляем в исполнение (поток), если нет предыдущей (@main[]?)
				// то есть первую попавшуюся
				AbstractNode builded = (AbstractNode) builder.BuildedObject;
				node.Add(builded); // TODO поправить
				builded.Parent = node;
				// если обнаружили функцию и создали ее, то закончили предыдущую текстовую ноду.
				isInTextNode = false;
				return builded;
			}
			// функция не создалась (это не обявление функции)
			return null;
		}

		#region Vars

		private Text currentText;

		private Node rootNode;

		private Boolean isInTextNode = false;

		public int? CurrentIndex;
		private string source;

		public Node RootNode
		{
			get { return rootNode; }
			set { rootNode = value; }
		}

		#endregion 
		 
	}
}
