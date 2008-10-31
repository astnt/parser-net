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
			return String.Empty;
		}
	}
}
