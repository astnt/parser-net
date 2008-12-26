using System;
using System.Collections.Generic;
using System.Text;

namespace Parser.Model
{
	/// <summary>
	/// Обобщенное хранение в таблице.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class Table<T> : IExecutable
	{
		private List<Row<T>> rows = new List<Row<T>>();
		public List<Row<T>> Rows
		{
			get { return rows; }
			set { rows = value; }
		}

		public int Index
		{
			get { return currentRow; }
			set { currentRow = value; }
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
		/// <summary>
		/// TODO добраться до индексера из отражения.
		/// </summary>
		public T column(Caller caller)
		{
			Parametr parametr = caller.Childs[0] as Parametr; 
			Text text = (Text) parametr.Childs[0];
			return this[Int32.Parse(text.Body)];
		}
		public T this[int col]
		{
			get { return rows[currentRow].Cells[col]; }
		}
		public T this[int row, int col]
		{
			get { return Rows[row].Cells[col]; }
		}
		private int currentRow;
		/// <summary>
		/// $table[^table::create[]]
		/// ^table.menu{
		///		$table.0
		/// }
		/// </summary>
		/// <returns></returns>
		public void menu(Caller caller)
		{
			Parametr parametr = caller.Childs[0] as Parametr; 
			currentRow = 0;
			foreach (Row<T> row in rows)
			{
				exec.Run(parametr.Childs);
				currentRow += 1; // для индексера
			}
			currentRow = 0;
		}

		private Executor exec;
		public void AddExecutor(Executor executor)
		{
			exec = executor;
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
