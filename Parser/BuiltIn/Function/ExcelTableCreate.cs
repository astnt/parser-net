using System;
using System.Collections.Generic;

namespace Parser.BuiltIn.Function
{
	[ParserName("table::excel")]
	public class ExcelTableCreate : ICompute
	{
		/// <summary>
		/// В идеале, единственное предназначение - вернуть нужный объект.
		/// </summary>
		/// <param name="vars"></param>
		/// <returns>Табличка из excel.</returns>
		public object Compute(List<object> vars)
		{
			// TODO
			string s = "";
			foreach (object o in vars)
			{
				s += ";"+o;
			}
			Console.WriteLine("table::excel[{0}]",s);
			return String.Empty;
		}
	}
}
