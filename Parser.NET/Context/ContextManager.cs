using System;
using System.Collections.Generic;
using Parser.Model;

namespace Parser.Context
{
	/// <summary>
	/// Управление контекстом.
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
			contexts[GLOBAL].Add(variable.Name, variable);
		}

		/// <summary>
		/// Получить значение переменной.
		/// </summary>
		/// <param name="variable">Описание переменной.</param>
		/// <returns>Объект значение переменной.</returns>
		public Variable GetVar(Variable variable)
		{
			Function func = variable.Parent as Function;
			if(func != null)
			{
				// если функция (наверное всегда функция?), пытаемся найти в ее контексте
				if(contexts[func.Name].ContainsKey(variable.Name))
				{
					return contexts[func.Name][variable.Name];
				}
			}
			// если не нашли в функции, то поищем в глобальном контексте
			if (contexts[GLOBAL].ContainsKey(variable.Name))
			{
				return contexts[GLOBAL][variable.Name];
			}
			return null;
		}

		/// <summary>
		/// Синхронизирует передаваемые параметры с принимаемыми,
		/// добавляет в контекст функции переменные.
		/// </summary>
		/// <param name="parameters"></param>
		/// <param name="func"></param>
		/// <param name="caller"></param>
		public void AddVars(Params parameters, Function func, Caller caller)
		{
			if(!contexts.ContainsKey(func.Name))
			{
				contexts[func.Name] = new Dictionary<string, Variable>();
			}
			for (int position = 0; position < parameters.Names.Length; position += 1)
			{
				Variable variable = new Variable();
				variable.Name = func.Parameters.Names[position];
				// TODO value как переменная
				variable.Value = caller.Parameters.Names[position];
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
