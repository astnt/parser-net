using System.Collections;
using System.Collections.Generic;

namespace Parser.Model
{
	public class Expression : IEnumerable<Element>
	{
		private List<Element> items = new List<Element>();
		/// <summary>
		/// По приоритету влево (раньше)
		/// </summary>
		/// <param name="item"></param>
		public void Left(Element item)
		{
			items.Add(item);
		}
		/// <summary>
		/// По приоритету вправо (позже)
		/// </summary>
		/// <param name="item"></param>
		public void Right(Element item)
		{
			
		}
		IEnumerator<Element> IEnumerable<Element>.GetEnumerator()
		{
			return items.GetEnumerator();
		}
		public IEnumerator GetEnumerator()
		{
			return ((IEnumerable<Element>)this).GetEnumerator();
		}
	}
}