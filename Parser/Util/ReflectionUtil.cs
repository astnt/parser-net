using System;
using System.Reflection;
using Parser.Model;
using Parser.Model.Context;

namespace Parser.Util
{
	/// <summary>
	/// Упрощение работы с отражением.
	/// </summary>
	public class ReflectionUtil
	{
		/// <summary>
		/// Поиск метода.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		public MethodInfo SearchMethod(Object value, String[] name)
		{
			Type type = value.GetType();
			MethodInfo methodInfo = type.GetMethod(name[0], new Type[] { });
			if (methodInfo == null) // попробуем еще раз получить метод
			{
				methodInfo = type.GetMethod(name[0]);
			}
			return methodInfo;
		}
		/// <summary>
		/// Поиск значения.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		public Object SearchValue(Object value, String[] name)
		{
			Type type = value.GetType();
			Object result = null;
			for (int i = 0; i < name.Length; i += 1)
			{
				PropertyInfo propertyInfo = type.GetProperty(name[i]);
				if (propertyInfo != null)
				{
					result = propertyInfo.GetGetMethod().Invoke(value, null);
					type = result.GetType();
					value = result;
				}
				else
				{
					PropertyInfo indexer = type.GetProperty("Item", new Type[] {typeof (String)});
//					PropertyInfo p2 = type.GetProperty("Item", new Type[] {typeof (Int32)});
					if(indexer != null)
					{
						result = indexer.GetGetMethod().Invoke(value, new Object[]{ name[i] });
						if (result != null)
						{
							type = result.GetType();
							value = result;
						}
					}
				}
			}
			return result;
		}

		public Object GetObjectFromMethod(ContextVariable var, Caller caller, Object[] vars)
		{
			Object resultOfMethod;
			MethodInfo methodInfo = SearchMethod(var.Value, new String[] { caller.Name[1] });
			if (methodInfo != null)
			{
				if (var.Value as IExecutable != null) // если относиться к типам выполняющим парсерное дерево,
				{
					try
					{
						resultOfMethod = methodInfo.Invoke(var.Value, new object[] {caller});
					}
					catch(TargetParameterCountException tpce)
					{
						throw new TargetParameterCountException(String.Format(@"Parameter count mismatch in method '{0}' of type '{1}'."
							, caller.Name[1], var.Value.GetType()));
					}
				}
				else // для остальных
				{
					// превращаем в стандартный объект для "вне" парсерных методов
					if (vars != null && vars.Length == 1 && vars[0].ToString() == String.Empty)
					{
						// убиваем переменные, если это пустая строка, т.е. ^method[]
						vars = null;
					}
					resultOfMethod = methodInfo.Invoke(var.Value, vars);
				}
			}
			else
			{
				throw new NullReferenceException(
					String.Format(@"Function or method with name ""{0}"" not found.", Dumper.Dump(caller.Name)));
			}
			return resultOfMethod;
		}
	}
}
