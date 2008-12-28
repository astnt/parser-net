using System;
using System.Collections.Generic;
using Parser.Model;
using Parser.Model.Context;

namespace Parser.Context
{
	/// <summary>
	/// Управление контекстом переменых.
	/// </summary>
	public class ContextManager
	{
		/// <summary>
		/// Ключ глобального контекста.
		/// </summary>
		private const string GLOBAL = "__GLOBAL__"; // (?)

		private readonly IDictionary<string, IDictionary<string, ContextVariable>> contexts = 
			new Dictionary<string, IDictionary<string, ContextVariable>>();

		private Executor exec;

		/// <summary>
		/// Создает глобальный контекст.
		/// </summary>
		public ContextManager(Executor executor)
		{
			exec = executor;
			contexts.Add(GLOBAL, new Dictionary<string, ContextVariable>());
		}
		/// <summary>
		/// Сокращение для добаления переменной.
		/// Добавляет в GLOBAL. TODO в контекст конкретной функции/тела цикла.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		public void AddVar(string name, object value)
		{
			AddVar(new ContextVariable(name, value));
		}
		/// <summary>
		/// Добавляет в глобальный контекст переменную.
		/// </summary>
		/// <param name="variable">Описание переменной.</param>
		public void AddVar(ContextVariable variable)
		{
			if (contexts[GLOBAL].ContainsKey(variable.Name))
			{
				contexts[GLOBAL][variable.Name] = variable;
			}
			else
			{
				contexts[GLOBAL].Add(variable.Name, variable);
			}
		}
		public Variable GetVar(string name)
		{
			Variable var = null;
			if (contexts[GLOBAL].ContainsKey(name))
			{
				var = contexts[GLOBAL][name];
			}
			return var;
		}
		/// <summary>
		/// Получить значение переменной.
		/// </summary>
		/// <param name="variable">Описание переменной.</param>
		/// <returns>Переменная со значением.</returns>
		public Object GetVar(VariableCall variable)
		{
			string key = variable.Name[0]; // TODO UNDONE
			Function func = variable.Parent as Function;
			if (func != null && contexts.ContainsKey(func.Name))
			{
				// если функция (наверное всегда функция?), пытаемся найти в ее контексте
				if(contexts[func.Name].ContainsKey(key))
				{
					return contexts[func.Name][key];
				}
			}
			// если не нашли в функции, то поищем в глобальном контексте
			if (contexts[GLOBAL].ContainsKey(key))
			{
				// UNDONE
				ContextVariable contextVariable = contexts[GLOBAL][key];
				if (variable.Name.Length > 1)
				{
					return exec.RefUtil.SearchValue(contextVariable.Value, variable.Name);
				}
				return contextVariable;
			}
			return null;
		}
		/// <summary>
		/// Синхронизирует передаваемые параметры с принимаемыми,
		/// добавляет в контекст функции переменные.
		/// </summary>
		/// <param name="vars"></param>
		/// <param name="func"></param>
		/// <param name="caller"></param>
		public void AddVars(List<object> vars, Function func, Caller caller)
		{
			if(!contexts.ContainsKey(func.Name))
			{
				contexts[func.Name] = new Dictionary<string, ContextVariable>();
			}
			if(vars == null)
			{
				// UNDONE
//				Console.WriteLine("Parameters in {0} is null.", func.Name);
				return;
			}
			for (int position = 0; position < vars.Count; position += 1)
			{
				ContextVariable contextVariable = new ContextVariable();
				if(func.Childs.Count <= position) // если детей (принемаемых значений) меньше чем передаваемых значений
				{
					break;
				}
				Text name = ((Parametr) func.Childs[position]).Childs[0] as Text;
				if(name == null || name.Body == null) // HACK
				{
					break;
				}
				contextVariable.Name = name.Body; // FIXME в что-то понятнее
				// TODO value как переменная
				contextVariable.Value = vars[position];
				contextVariable.Parent = func;
				IDictionary<string, ContextVariable> variables = contexts[func.Name];
				if (!variables.ContainsKey(contextVariable.Name))
				{
					contexts[func.Name].Add(contextVariable.Name, contextVariable);
				}
				else
				{
					contexts[func.Name][contextVariable.Name] = contextVariable;
					//throw new ArgumentException(String.Format(@"Variable with name ""{0}"" already exist in function ""{1}"".", variable.Name, func.Name));
				}
			}
		}
	}
}
