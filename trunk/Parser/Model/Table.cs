using System;
using System.Collections.Generic;
using System.Text;
using Parser.Util;

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
			if (parametr == null)
			{
				throw new Exception(String.Format(@"Parametr of caller '{0}' is null.", Dumper.Dump(caller.Name)));
			}
			Text text = (Text) parametr.Childs[0];
				return this[Int32.Parse(text.Body)];
			
		}
		public String this[String col] // HACK
		{
			get
			{
				int value;
				if(Int32.TryParse(col, out value))
				{
					return rows[currentRow].Cells[Convert.ToInt32(col)].ToString();
				}
				return null;
			}
		}
		public T this[Int32 col]
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
				if (parametr != null)
				{
					 exec.Run(parametr.Childs);
				}
				else
				{
					throw new Exception(String.Format(@"Parametr of caller '{0}' is null.",
						Dumper.Dump(caller.Name)));
				}
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
