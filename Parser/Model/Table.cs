using System.Collections.Generic;

namespace Parser.Model
{
	/// <summary>
	/// Обобщенное хранение в таблице.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class Table<T>
	{
		private List<Row<T>> rows = new List<Row<T>>();
		public List<Row<T>> Rows
		{
			get { return rows; }
			set { rows = value; }
		}
		public void Add(int row, int col, T value)
		{
			if(rows.Count <= row)
			{
				rows.Add(new Row<T>());
			}
			if (Rows[row].Cells.Count <= col)
			{
				Rows[row].Cells.Add(value);
				return;
			}
			Rows[row].Cells[col] = value;
		}
		public T this[int row, int col]
		{
			get { return Rows[row].Cells[col]; }
		}
		public T this[int col]
		{
			get { return rows[currentRow].Cells[col]; }
		}
		private int currentRow = 0;
		/// <summary>
		/// $table[^table::create[]]
		/// ^table.menu{
		///		$table.0
		/// }
		/// </summary>
		/// <param name="param"></param>
		/// <returns></returns>
		public Row<T> menu(Parametr param)
		{
			Row<T> row = Rows[currentRow];
			return row;
		}
	}
	public class Row<T>
	{
		private List<T> cells = new List<T>();
		public List<T> Cells
		{
			get { return cells; }
			set { cells = value; }
		}
	}
}
