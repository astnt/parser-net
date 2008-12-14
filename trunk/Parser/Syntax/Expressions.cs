using System;
using System.Collections.Generic;

namespace Parser.Syntax
{
	public static class Expressions
	{
		public const string EqualString = "eq";
		public const string Equal = "==";
		public static readonly List<String> IfExpressions
			= new List<String>(new string[]{ EqualString, Equal });
	}
}
