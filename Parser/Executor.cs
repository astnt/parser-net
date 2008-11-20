using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Parser.BuiltIn.Function;
using Parser.Context;
using Parser.Model;

namespace Parser
{
	/// <summary>
	/// Выполняет чтение и исполнение из
	/// дерева-исходника парсера.
	/// </summary>
	public class Executor
	{

		#region vars
		/// <summary>
		/// Текстовы результат.
		/// </summary>
		private StringBuilder defaultOutput = new StringBuilder();

		/// <summary>
		/// Текстовый вывод.
		/// TODO добавить getter и setter
		/// </summary>
		public StringBuilder TextOutput;
		/// <summary>
		/// Объектный вывод.
		/// </summary>
		public object Output;
		private RootNode root;
		/// <summary>
		/// Глобальный контекст.
		/// </summary>
		private ContextManager contextManager = new ContextManager();
		public ContextManager ContextManager
		{
			get { return contextManager; }
		}
		#endregion

		/// <summary>
		/// Запуск выполнения с корневой ноды.
		/// </summary>
		/// <param name="node">Корневая нода дерева.</param>
		public void Run(RootNode node)
		{
			TextOutput = defaultOutput;
			root = node;
			bool hasFunc = false;
			foreach (AbstractNode child in node.Childs)
			{
				Function func = child as Function;
				if(func != null)
				{
					Run(func.Childs);
					hasFunc = true;
					// так как нашли первую функцию, ее и исполняем
					// TODO наверное нужна именно @main[]
					break;
				}
			}
			if(!hasFunc)
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
			ICompute computeFunc = (ICompute) func.RefObject.Invoke(new object[0]);
			// получаем переменные
			List<object> vars = ExtractVars(caller);
			// что-то получаем,
			object something = computeFunc.Compute(vars);
			// если строка
			string result = something as String;
			if(result != null)
			{
				// добавляем в текущий вывод
				TextOutput.Append(result);
				return;
			}
			// UNDONE
			// если что-то еще?
			Output = something;
			// вариант для отладки
			// TextOutput.Append(something.GetType().ToString());
		}

		/// <summary>
		/// Разбираем детей и выясняем, что с ними делать.
		/// </summary>
		/// <param name="childs"></param>
		private void Run(IList<AbstractNode> childs)
		{
			foreach (AbstractNode node in childs)
			{
				Text text = node as Text;
				if(text != null)
				{
					TextOutput.Append(text.Body);
				}
				Caller caller = node as Caller;
				if(caller != null)
				{
//					Console.WriteLine("Run caller with name {0}", caller.Name[0]);
					// Передаем управление найденной функции.
					Function func = Call(root, caller);
					if (func != null)
					{
						Run(caller, func);
					}
				}
				Variable variable = node as Variable;
				if(variable != null)
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
		/// UNDONE
		/// - сделать вызов по точкам
		/// - значение переменной не обязательно строка
		/// </summary>
		/// <param name="varCall"></param>
		private void Run(VariableCall varCall)
		{
			Variable variable = contextManager.GetVar(varCall);
			if (variable != null) // переменной может и не быть, в этом случае ничего не делаем
			{
				StringBuilder stringBuilder = variable.Value as StringBuilder;
				String stringValue = variable.Value as String;
				if (stringBuilder != null || stringValue != null)
				{
					TextOutput.Append(variable.Value);
					return;
				}
				// когда нет текстового значения выводим текстовое название типа
				if(varCall.Name.Length == 1 && variable.Value != null)
				{
					String typeName = variable.Value.GetType().ToString();
					TextOutput.Append(typeName);
				}
				// $some.thing.here
				if(varCall.Name.Length > 1)
				{
					object result = null;
					Type type = variable.Value.GetType();
					// ищем поле
					int index = 1;
					foreach (string name in varCall.Name)
					{
						PropertyInfo propertyInfo = type.GetProperty(name);
						// если последний в цепочке
						if (propertyInfo != null)
						{
							result = propertyInfo.GetGetMethod().Invoke(variable.Value, null);
						}
						if (result != null) type = result.GetType();
						index += 1;
					}
					if (!string.IsNullOrEmpty(result as String))
					{
						TextOutput.Append(result);
					}
					else if (result != null) {
						TextOutput.Append(result.ToString());
					}
			}
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
				Variable contextVariable = new Variable();
				contextVariable.Name = variable.Name;
				StringBuilder variableOutput = new StringBuilder();
				
				TextOutput = variableOutput; // меняем "поток" вывода
				Run((Parametr)variable.Childs[0]);

				contextVariable.Value = Output ?? TextOutput;
				TextOutput = defaultOutput; // возвращаем дефолтный поток вывода				
				contextManager.AddVar(contextVariable); // добавляем в контекст
				// TODO VariableCall?
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
			Boolean hasFuncLikeInVar = false;
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
			}
			// поиск переменной с таким именем -> вызов ее метода(ов)
			// TODO вызов метода объекта
			if(caller.Name.Length > 1)
			{
				Variable var = contextManager.GetVar(caller.Name[0]);
				if(var != null && var.Value != null)
				{
					Type type = var.Value.GetType();
					// ищем метод
					MethodInfo methodInfo = type.GetMethod(caller.Name[1]);
					if (methodInfo != null)
					{
						object methodResult = methodInfo.Invoke(var.Value, null);
						TextOutput.Append(methodResult);
						hasFuncLikeInVar = true;
					}
				}
			}
			// если есть функции с таким именем как в caller
			if (hasFuncLikeInCaller)
			{
				List<object> vars = ExtractVars(caller);
				// кладем в контекст найденной функции
				contextManager.AddVars(vars, func, caller);
			}
			if(!hasFuncLikeInCaller && !hasFuncLikeInVar) // 
			{
				throw new NullReferenceException(
					String.Format(@"Function or method with name ""{0}"" not found.", caller.Name));
//				Console.WriteLine(@"Function with name ""{0}"" not found.", caller.Name);
			}
			return func;
		}

		/// <summary>
		/// Узнаем какие переменные (или строки есть в caller)
		/// Возвращаем ввиду списка значений.
		/// </summary>
		/// <param name="caller"></param>
		/// <returns></returns>
		private List<object> ExtractVars(Caller caller)
		{
			List<object> vars = new List<object>();
			foreach (AbstractNode child in caller.Childs)
			{
				Parametr parametr = child as Parametr;
				// пробегаемся по детям, если параметр, то добавляем "значение"
				if(parametr != null)
				{
					vars.Add(ExtractVar(parametr.Childs));
				}
			}
			return vars;
		}

		private object ExtractVar(IList<AbstractNode> childs)
		{
			StringBuilder textResult = new StringBuilder();
			foreach (AbstractNode node in childs)
			{
				Text text = node as Text; // FIXME повторение
				if (text != null)
				{
					textResult.Append(text.Body); // WARN возможно не правильно
				}
			}
			// UNDONE результаты другого типа 
			return textResult;
		}
	}
}
