using System;
using System.Collections.Generic;

namespace Parser.Syntax
{
	public static class Expressions
	{
		public const string EqualString = "eq";
		public const string Equal = "==";
		public const string And = "&&";
		public const string Or = "||";
		public static readonly List<String> IfExpressions
			= new List<String>(new string[]{ EqualString, Equal, And, Or });
		public static readonly Dictionary<Char, String> expressionFirstChar = new Dictionary<Char, String>();
		static Expressions()
		{
			foreach (String expression in IfExpressions)
			{
				expressionFirstChar.Add(expression[0], expression);
			}
		}
		
	}
}
