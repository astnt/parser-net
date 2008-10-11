using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parser.Model
{
	/// <summary>
	/// Обобщенное хранение в таблице.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class Table<T> : IEnumerable<T>, IEnumerator<T>
	{
		public IDictionary<string, T> cells = new Dictionary<string, T>();

		public void Add(int row, int col, T value)
		{
			if (row > rowsCount) rowsCount += 1;
			if (col > colsCount) colsCount += 1;
			cells.Add(String.Format("{0},{1}", row, col), value);
		}

		public T this[int row, int col]
		{
			get { return cells[String.Format("{0},{1}", row, col)]; }
//			set
//			{
//
//				cells.Add(String.Format("{0},{1}", row, col), value);
//			}
		}

		#region vars

		private int rowsCount = 0;
		public int RowsCount
		{
			get { return rowsCount; }
			set { rowsCount = value; }
		}

		private int colsCount = 0;
		public int ColsCount
		{
			get { return colsCount; }
			set { colsCount = value; }
		}

		private T current;
		private int currentRow = 0;

		#endregion

		#region enumerator implementation

		T IEnumerator<T>.Current
		{
			get
			{
				return current = this[currentRow, 0];
			}
		}

		public void Dispose()
		{
			//?
		}

		public bool MoveNext()
		{
			currentRow += 1;
			return true;
		}

		public void Reset()
		{
			currentRow = 0;
		}

		public object Current // ?
		{
			get { return current; }
		}

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return (IEnumerator<T>) GetEnumerator();
		}

		public IEnumerator GetEnumerator()
		{
			return ((IEnumerable<T>) this).GetEnumerator();
		}

		#endregion

		public class Row
		{
			
		}

	}
}
