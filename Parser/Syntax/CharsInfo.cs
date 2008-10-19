using System;

namespace Parser.Syntax
{
	static class CharsInfo
	{
		public static Char FuncDeclarationStart = '@';
		public static Char ParamsStart = '[';
		public static Char ParamsEnd = ']';
		public static Char ParamsEvalStart = '(';
		public static Char ParamsEvalEnd = ')';
		public static Char ParamsCodeStart = '{';
		public static Char ParamsCodeEnd = '}';
		public static Char CallerDeclarationStart = '^';
		public static Char VariableDeclarationStart = '$';
	}
}
