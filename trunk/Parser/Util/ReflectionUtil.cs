﻿using System;
using System.Reflection;
using System.Text;
using Parser.Model.Context;

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
	}
}
