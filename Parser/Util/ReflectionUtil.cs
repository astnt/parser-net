using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Parser.Util
{
	/// <summary>
	/// Упрощение работы с отражением.
	/// </summary>
	public class ReflectionUtil
	{
		public void SearchMethod(String[] name)
		{
			Type t = typeof(StringBuilder);
			Console.WriteLine("Type of class: " + t);
			Console.WriteLine("Namespace: " + t.Namespace);
			MethodInfo[] mi = t.GetMethods();
			Console.WriteLine("Methods are:");

			foreach (MethodInfo i in mi)
			{
				Console.WriteLine("Name: " + i.Name);
				ParameterInfo[] pif = i.GetParameters();
				foreach (ParameterInfo p in pif)
				{
					Console.WriteLine("Type: " + p.ParameterType + " parameter name: " + p.Name);
				}
			}
		}
	}
}
