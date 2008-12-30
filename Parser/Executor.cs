using System;
using System.Collections.Generic;
using System.Text;
using Parser.BuiltIn.Function;
using Parser.Context;
using Parser.Model;
using Parser.Model.Context;
using Parser.Util;

namespace Parser
{
	/// <summary>
	/// Выполняет чтение и исполнение из
	/// дерева-исходника парсера.
	/// </summary>
	public class Executor
	{
		/// <summary>
		/// Запуск выполнения с корневой ноды.
		/// </summary>
		/// <param name="node">Корневая нода дерева.</param>
		public void Run(RootNode node)
		{
			TextOutput = defaultOutput; // при старте - вывод "по-молчанию"
			root = node;
			bool hasFunc = false;
			foreach (AbstractNode child in node.Childs)
			{
				Function func = child as Function;
				if (func != null)
				{
					Run(func.Childs);
					hasFunc = true;
					// так как нашли первую функцию, ее и исполняем
					// TODO наверное нужна именно @main[]
					break;
				}
			}
			if (!hasFunc)
			{
				throw new NullReferenceException("Function's declaration is not found.");
			}
		}

		/// <summary>
		/// Спорный вызов некой "исполняемой" функции типа ^eval()
		/// </summary>
		/// <param name="func"></param>
		/// <param name="caller"></param>
		private void Run(Function func, Caller caller)
		{
			ICompute computeFunc = func.RefObject.Invoke(new object[0]) as ICompute;
			object something = null;
			if (computeFunc != null)
			{
				something = computeFunc.Compute(caller, this);
			}
			if (!string.IsNullOrEmpty(something as String))
			{
				// добавляем в текущий вывод
				TextOutput.Append(something); // TODO если в результата "вычеслительной" функции получилась строка, добавляем ее 
			}
			else
			{
				// UNDONE
				Output = something; // TODO если еще какая-то хуйня, то просто типа объект.
			}
		}

		/// <summary>
		/// Разбираем детей и выясняем, что с ними делать.
		/// </summary>
		/// <param name="childs"></param>
		internal void Run(IList<AbstractNode> childs)
		{
			foreach (AbstractNode node in childs)
			{
				Text text = node as Text;
				if (text != null)
				{
					TextOutput.Append(text.Body); // TODO если напоролись на текстовую ноду, то типа гоним ее в текст.
				}
				Caller caller = node as Caller;
				if (caller != null)
				{
					// Передаем управление найденной функции.
					Function func = Call(root, caller);
					if (func != null)
					{
						Run(caller, func);
					}
				}
				Variable variable = node as Variable;
				if (variable != null)
				{
					Run(variable);
				}
				VariableCall variableCall = node as VariableCall;
				if (variableCall != null)
				{
					Run(variableCall);
				}
			}
		}

		private void Run(Caller caller, Function func)
		{
			if (func.RefObject != null)
			{
				Run(func, caller);
			}
			Run(func.Childs);
		}

		/// <summary>
		/// Вызывает переменную
		/// </summary>
		/// <param name="varCall"></param>
		private void Run(VariableCall varCall)
		{
			Object result = contextManager.GetVar(varCall);
			ContextVariable variable = result as ContextVariable;
			if (variable != null && variable.Value != null)
			{
				TextOutput.Append(variable.Value); // если напоролись на переменную, достаем и зачем-то шлепаем в текстовый контекст
			}
			else if (result != null)
			{
				TextOutput.Append(result); // если результат не контекстная переменная - суем в текстовый вывод зачем-то
			}
		}

		private void Run(Parametr parametr)
		{
			Run(parametr.Childs);
		}

		/// <summary>
		/// Вызов или помещение в контекст.
		/// </summary>
		/// <param name="variable">Синтаксическое описание переменной из дерева.</param>
		private void Run(Variable variable)
		{
			// если есть значение переменной - это объявление
			if (variable.Childs.Count > 0)
			{
				// пишем в контекст
				ContextVariable contextVariable = new ContextVariable();
				contextVariable.Name = variable.Name;
				StringBuilder variableOutput = new StringBuilder(); // если вывод переменной, то это обязательно почему-то String

				TextOutput = variableOutput; // меняем "поток" вывода
				Run((Parametr) variable.Childs[0]);

				if(Output == null) // HACK типа если в Output не насралось,
				{
					Output = new ParserString(TextOutput);
				}
				contextVariable.Value = Output ?? TextOutput; // какая-то хуйня
				Output = null; // TODO Обнуляем HACK

				TextOutput = defaultOutput; // возвращаем дефолтный поток вывода
				contextManager.AddVar(contextVariable); // добавляем в контекст
				if (contextVariable.Value as IExecutable != null)
				{
					// добавляем ссылку на Executor, для дальнейшего выполнения дерева.
					((IExecutable) contextVariable.Value).AddExecutor(this);
				}
			}
		}
		/// <summary>
		/// Возвращает нужную для обработки функцию.
		/// </summary>
		/// <param name="node">Корневая нода, так как в ней находяться объявленные функции.</param>
		/// <returns>Искомая функция.</returns>
		/// <param name="caller">Описание вывода.</param>
		public Function Call(RootNode node, Caller caller)
		{
			Function func = null;
			Boolean hasFuncLikeInCaller = false;
			// поиск функции с таким именем
			if (caller.Name.Length == 1)
			{
				foreach (AbstractNode child in node.Childs)
				{
					func = child as Function;
					if (func != null && func.Name == caller.Name[0])
					{
						hasFuncLikeInCaller = true;
						break;
					}
				}
				// если есть функции с таким именем как в caller
				if (hasFuncLikeInCaller && /* RefObject - функция другого типа TODO вынести наследованим? */ func.RefObject == null)
				{
					// вызов когда добавляем в контекст
					List<object> vars = ExtractVars(caller);
					// кладем в контекст найденной функции
					contextManager.AddVars(vars, func, caller);
				}
				if (!hasFuncLikeInCaller)
				{
					throw new NullReferenceException(
						String.Format(@"Function or method with name ""{0}"" not found.", Dumper.Dump(caller.Name)));
				}
			}
			// поиск переменной с таким именем -> вызов ее метода(ов)
			if (caller.Name.Length > 1)
			{
				Object result = contextManager.GetVar(caller.Name[0]);
				ContextVariable var = result as ContextVariable;
				if (var != null && var.Value != null)
				{
					Object[] vars = null;
					if (var.Value as IExecutable == null)
					{
						vars = ExtractVars(caller).ToArray();
					}
					object resultOfMethod = refUtil.GetObjectFromMethod(var, caller, vars);
					TextOutput.Append(resultOfMethod); // опять почему-то прихуячили к текстовомоу выводу результат метода
				}
			}
			return func;
		}
		/// <summary>
		/// Узнаем какие переменные (или строки есть в caller)
		/// Возвращаем ввиду списка значений.
		/// </summary>
		/// <param name="caller"></param>
		/// <returns></returns>
		public List<object> ExtractVars(Caller caller)
		{
			List<object> vars = new List<object>();
			foreach (AbstractNode child in caller.Childs)
			{
				Parametr parametr = child as Parametr;
				// пробегаемся по детям, если параметр, то добавляем "значение"
				if (parametr != null)
				{
					vars.Add(ExtractVar(parametr.Childs));
				}
			}
			return vars;
		}

		private object ExtractVar(IList<AbstractNode> childs)
		{
			TextOutput = new StringBuilder();
			Run(childs);
			StringBuilder stringBuilder = new StringBuilder(TextOutput.ToString());
			TextOutput = defaultOutput;
			// UNDONE результаты другого типа 
			return stringBuilder;
		}

		#region vars

		/// <summary>
		/// Текстовый результат.
		/// </summary>
		private StringBuilder defaultOutput = new StringBuilder();

		/// <summary>
		/// Текстовый вывод. (мега хуйня, куда собирается весь текст для вывода)
		/// TODO добавить getter и setter
		/// </summary>
		public StringBuilder TextOutput;

		/// <summary>
		/// Объектный вывод.
		/// </summary>
		public object Output;

		private RootNode root;
		private ContextManager contextManager;

		/// <summary>
		/// Глобальный контекст.
		/// </summary>
		public ContextManager ContextManager
		{
			get { return contextManager; }
		}

		/// <summary>
		/// Утилита объединяющая методы для работы с отражением.
		/// </summary>
		public ReflectionUtil RefUtil
		{
			get { return refUtil; }
			set { refUtil = value; }
		}

		private ReflectionUtil refUtil = new ReflectionUtil();

		public Executor()
		{
			contextManager = new ContextManager(this);
		}

		#endregion
	}
}