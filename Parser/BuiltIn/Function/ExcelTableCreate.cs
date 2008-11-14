using System;
using System.Collections.Generic;
using System.IO;
using Parser.NET.Util.Wrapper;

namespace Parser.BuiltIn.Function
{
	/// <summary>
	/// Конструктор таблицы из excel.
	/// </summary>
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
				s += ";" + o.ToString().Trim();
			}
			Console.WriteLine("table::excel[{0}]",s);
			Console.WriteLine("query '{0}'", vars[0]);
			File.ReadAllText(vars[1].ToString().Trim());
			ExcelWrapper ew = new ExcelWrapper();
			ew.Load(
				vars[1].ToString()
				, vars[0].ToString().Trim() // путь к файлу
			);
			return ew;
//			return String.Empty;
		}
	}
}
