using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Parser.Model;
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
//			string s = "";
//			foreach (object o in vars)
//			{
//				s += ";" + o.ToString().Trim();
//			}
//			Console.WriteLine("table::excel[{0}]",s);
//			Console.WriteLine("query '{0}'", vars[0]);
//			File.ReadAllText(vars[1].ToString().Trim());
			ExcelWrapper ew = new ExcelWrapper();
			ew.Load(
				vars[1].ToString()
				, vars[0].ToString().Trim() // путь к файлу
			);

			Table<string> table = new Table<string>();
			foreach (DataRow row in ew.Table.Rows)
			{
				Row<string> r = new Row<string>();
				foreach (object item in row.ItemArray)
				{
					r.Cells.Add(item.ToString()); // HACK ? Возможно лучше как полученный объекто  хранить
				}
				table.Rows.Add(r);
			}
			return table;
//			return ew;
//			return String.Empty;
		}
	}
}
