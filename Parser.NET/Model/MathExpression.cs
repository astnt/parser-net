using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parser.NET.Model
{
	public class MathExpression : IEnumerable<MathElement>
	{

		private List<MathElement> items = new List<MathElement>();

		/// <summary>
		/// По приоритету влево (раньше)
		/// </summary>
		/// <param name="item"></param>
		public void Left(MathElement item)
		{
			items.Add(item);
		}

		/// <summary>
		/// По приоритету вправо (позже)
		/// </summary>
		/// <param name="item"></param>
		public void Right(MathElement item)
		{
			
		}

		IEnumerator<MathElement> IEnumerable<MathElement>.GetEnumerator()
		{
			return items.GetEnumerator();
		}

		public IEnumerator GetEnumerator()
		{
			return ((IEnumerable<MathElement>)this).GetEnumerator();
		}
	}
}
