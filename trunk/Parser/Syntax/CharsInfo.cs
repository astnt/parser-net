using System;

namespace Parser.Syntax
{
	public static class CharsInfo
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

		/// <summary>
		/// В пробельных символах \r\n\t (char)160
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		public static bool IsInSpaceChars(char c)
		{
			return c == '\n'
					|| c == '\r'
					|| c == ' '
					|| c == (char) 160;
		}
		/// <summary>
		/// 
		/// </summary>
		public static bool IsDigit(char c)
		{
			return 47 < c && c < 58;
		}
	}
}
