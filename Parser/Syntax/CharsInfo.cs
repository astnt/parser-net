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
		public static Char ParametrSeparator = ';';

		/// <summary>
		/// В символах ]})
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		public static bool IsInParamsEndChars(char c)
		{
			return c == ParamsEnd
					|| c == ParamsEvalEnd
					|| c == ParamsCodeEnd;
		}

	}
}
