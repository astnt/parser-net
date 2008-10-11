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
		private StringBuilder output = new StringBuilder();

		/// <summary>
		/// Текстовы результат.
		/// </summary>
		public StringBuilder Output
		{
			get { return output; }
			set { output = value; }
		}

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

		private void Run(Function func, Caller caller)
		{
			ICompute computeFunc = (ICompute) func.RefObject.Invoke(new object[0]);
			object something = computeFunc.Compute(caller.Parameters);
			string result = something as String;
			if(result != null)
			{
				output.Append(result);
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
					output.Append(text.Body);
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
			}
		}

		/// <summary>
		/// Вызов или помещение в контекст.
		/// </summary>
		/// <param name="variable">Синтаксическое описание переменной из дерева.</param>
		private void Run(Variable variable)
		{
			// если есть значение переменной - это объявление
			if (variable.Value != null)
			{
				// пишем в контекст
				contextManager.AddVar(variable);
			}
			else // вызов
			{
				Variable contextVar = contextManager.GetVar(variable);
				if (contextVar != null)
				{
					string value = contextVar.Value as String;
					if (value != null)
					{
						// пишем в вывод
						output.Append(value);
					}
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
				// узнаем какие переменные (или строки есть в caller)
				// добавляем в контекст функции.
				contextManager.AddVars(caller.Parameters, func, caller);
			}
			else
			{
				throw new NullReferenceException(
					String.Format(@"Function with name ""{0}"" not found.", caller.FuncName));
			}
			
			return func;
		}

	}
}
