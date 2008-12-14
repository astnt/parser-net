using System.Collections;
using System.Collections.Generic;

namespace Parser.Model
{
	/// <summary>
	/// deprecated
	/// </summary>
	public class Expression : IEnumerable<Operator>
	{
		private List<Operator> items = new List<Operator>();
		/// <summary>
		/// По приоритету влево (раньше)
		/// </summary>
		/// <param name="item"></param>
		public void Left(Operator item)
		{
			items.Add(item);
		}
		/// <summary>
		/// По приоритету вправо (позже)
		/// </summary>
		/// <param name="item"></param>
		public void Right(Operator item)
		{
			
		}
		IEnumerator<Operator> IEnumerable<Operator>.GetEnumerator()
		{
			return items.GetEnumerator();
		}
		public IEnumerator GetEnumerator()
		{
			return ((IEnumerable<Operator>)this).GetEnumerator();
		}
	}
}