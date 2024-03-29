﻿using System;
using System.Collections.Generic;
using System.Data;
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
		/// <returns>Табличка из excel.</returns>
		public object Compute(Caller caller, Executor exec)
		{
			List<object> vars = exec.ExtractVars(caller); // TODO заменить на разбор
//			Console.WriteLine("query [{0}], path[{1}]"
//				,	vars[1].ToString() // SELECT
//				, vars[0].ToString().Trim() // путь к файлу
//			);

			ExcelWrapper ew = new ExcelWrapper();
			ew.Load(
				  vars[1].ToString() // SELECT
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
