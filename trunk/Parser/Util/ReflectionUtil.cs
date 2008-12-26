using System;
using System.Reflection;

namespace Parser.Util
{
	/// <summary>
	/// Упрощение работы с отражением.
	/// </summary>
	public class ReflectionUtil
	{
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
	}
}
