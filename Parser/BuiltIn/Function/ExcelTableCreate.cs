using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Parser.Model;

namespace Parser.BuiltIn.Function
{
	[ParserName("table::excel")]
	public class ExcelTableCreate : ICompute
	{
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
