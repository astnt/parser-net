using System;
using System.Reflection;
using Parser.Builder;
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
				// собираем синтаксические данные из строк
				if(
					   c == CharsInfo.FuncDeclarationStart
					|| c == CharsInfo.CallerDeclarationStart
					|| c == CharsInfo.VariableDeclarationStart
				)
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
						bool isAdded = false;
						if(func != null)
						{
							// если объявление функции (@something[...][...])
							// перемещаем указатель парсинга на нее
							node = (Node) createdNode;
							// добавляем в корневую ноду
							rootNode.Add(node);
							isAdded = true;
						}
						if(var != null)
						{
							node.Add(createdNode);
							node = (Node)createdNode;
							isAdded = true;
						}
						if(!isAdded)
						{
							// для остальных добавляем в текущую ноду.
							node.Add(createdNode);
						}
					}
				}
				c = source[index];
				if (
						 c == CharsInfo.ParamsEnd
					|| c == CharsInfo.ParamsEvalEnd
					|| c == CharsInfo.ParamsCodeEnd
					)
				{
					CloseCurrentText(index);
					isInTextNode = false;
					// если не корень, спускаемся на ноду ниже
					if (node.Parent as RootNode == null)
					{
						node = node.Parent;
					}
					//index += 1;
					//CurrentIndex = index;
				}
				//Console.WriteLine("{1} char '{0}'", source[CurrentIndex.Value], isInTextNode);
				if (!isInTextNode)
				{
					CreateText(node);
				}
				// Подошли к концу строки
				if (index == lastCharIndex)
				{
					currentText.Body = source.Substring(currentText.Start.Value);
					// TODO прерывание
				}

			}
		}

		private void CreateText(Node node)
		{
			//Console.WriteLine("create text char '{0}' index of {1}",
			//  source[CurrentIndex.Value + 1],
			//  CurrentIndex.Value + 1);

			currentText = new Text();
			currentText.Start = CurrentIndex + 1;
			if (source[currentText.Start.Value] == ']')
			{
				currentText.Start += 1; // HACK TODO разобраться
			}
			currentText.Parent = node;
			node.Add(currentText);
			isInTextNode = true;
		}

		#region старый вариант
		/*
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
					bool isCloseTextNode;
					Node nodeOf = GetNodeToParseNext(node, out isCloseTextNode);
					if(isCloseTextNode)
					{
						CloseCurrentText(index);
					}
					CurrentIndex = index + 1;
					Parse((Function)Declaration(nodeOf, new FunctionBuilder()));
					break;
				}

				// создаем Caller
				if(c == CharsInfo.CallerDeclarationStart)
				{
					CloseCurrentText(index);
					CurrentIndex = index + 1;
					/Caller caller = (Caller)/ Declaration(node, new CallerBuilder());
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
		/// <param name="node"></param>
		/// <param name="isCloseTextNode"></param>
		/// <returns></returns>
		private Node GetNodeToParseNext(Node node, out bool isCloseTextNode)
		{
			Node nodeOf;
			if (node.Parent != null)
			{
				// Если есть родитель, то передаем управление ему
				nodeOf = node.Parent;
				isCloseTextNode = true;
			}
			else
			{
				// Если родителя нет, то это корневая нода
				nodeOf = node;
				isCloseTextNode = false;
			}
			return nodeOf;
		}
		*/
		#endregion

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
