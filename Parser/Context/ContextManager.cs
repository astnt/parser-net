using System;
using System.Collections.Generic;
using Parser.Model;

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

		private IDictionary<string, IDictionary<string, Variable>> contexts = new Dictionary<string, IDictionary<string, Variable>>();

		/// <summary>
		/// Создает глобальный контекст.
		/// </summary>
		public ContextManager()
		{
			contexts.Add(GLOBAL, new Dictionary<string, Variable>());
		}

		/// <summary>
		/// Добавляет в глобальный контекст переменную.
		/// </summary>
		/// <param name="variable">Описание переменной.</param>
		public void AddVar(Variable variable)
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
		public Variable GetVar(VariableCall variable)
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
				return contexts[GLOBAL][key];
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
				contexts[func.Name] = new Dictionary<string, Variable>();
			}
			if(vars == null)
			{
				// UNDONE
//				Console.WriteLine("Parameters in {0} is null.", func.Name);
				return;
			}
			for (int position = 0; position < vars.Count; position += 1)
			{
				Variable variable = new Variable();
				if(func.Childs.Count <= position) // если детей (принемаемых значений) меньше чем передаваемых значений
				{
					break;
				}
				Text name = ((Parametr) func.Childs[position]).Childs[0] as Text;
				if(name == null || name.Body == null) // HACK
				{
					break;
				}
				variable.Name = name.Body; // FIXME в что-то понятнее
				// TODO value как переменная
				variable.Value = vars[position];
				variable.Parent = func;
				IDictionary<string, Variable> variables = contexts[func.Name];
				if (!variables.ContainsKey(variable.Name))
				{
					contexts[func.Name].Add(variable.Name, variable);
				}
				else
				{
					contexts[func.Name][variable.Name] = variable;
					//throw new ArgumentException(String.Format(@"Variable with name ""{0}"" already exist in function ""{1}"".", variable.Name, func.Name));
				}
			}
		}
	}
}
