using System;
using System.Collections.Generic;
using System.Text;
using Parser.BuiltIn.Function;
using Parser.Context;
using Parser.Model;

namespace Parser
{
	/// <summary>
	/// Выполняет чтение и исполнение из потокобезопасного
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
		/// Текстовый результат.
		/// TODO добавить getter и setter
		/// </summary>
		public StringBuilder Output;
		private RootNode root;
		/// <summary>
		/// Глобальный контекст.
		/// </summary>
		private ContextManager contextManager = new ContextManager();
		#endregion

		/// <summary>
		/// Запуск выполнения с корневой ноды.
		/// </summary>
		/// <param name="node">Корневая нода дерева.</param>
		public void Run(RootNode node)
		{
			Output = defaultOutput;
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
			// 
			object something = computeFunc.Compute(caller.Parameters);
			string result = something as String;
			if(result != null)
			{
				Output.Append(result);
			}
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
					Output.Append(text.Body);
				}
				Caller caller = node as Caller;
				if(caller != null)
				{
					// Передаем управление найденной функции.
					Function func = Call(root, caller);
					if (func.RefObject != null)
					{
						Run(func, caller);
					}
					Run(func.Childs);
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
					Output.Append(variable.Value);
				}
				else // достаем из контекста если нет value
				{
					Variable contextVar = contextManager.GetVar(varCall);
					Run(contextVar);
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
				
				Output = variableOutput; // меняем "поток" вывода
//				Run(variable.Childs); // пишется в "поток" переменной
				Run((Parametr)variable.Childs[0]);
				contextVariable.Value = variableOutput; // присваеваем значение пременной, кладем в контекст
				
				Output = defaultOutput; // возвращаем дефолтный поток вывода
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
			foreach (AbstractNode child in node.Childs)
			{
				func = child as Function;
				if(func != null && func.Name == caller.FuncName)
				{
					hasFuncLikeInCaller = true;
					break;
				}
			}
			if (hasFuncLikeInCaller)
			{
				List<object> vars = ExtractVars(caller);
				contextManager.AddVars(vars, func, caller);
			}
			else
			{
				throw new NullReferenceException(
					String.Format(@"Function with name ""{0}"" not found.", caller.FuncName));
			}
			return func;
		}

		private List<object> ExtractVars(Caller caller)
		{
// узнаем какие переменные (или строки есть в caller)
			// добавляем в контекст функции.
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
			foreach (AbstractNode node in childs)
			{
				Text text = node as Text; // FIXME повторение
				if (text != null)
				{
					return text.Body; // WARN возможно не правильно
				}
			}
			return null;
		}
	}
}
