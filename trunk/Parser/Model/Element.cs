using System;

namespace Parser.Model
{
	/// <summary>
	/// Части мат. выражения.
	/// </summary>
	public class Element
	{
		#region vars
		private char operation;
		public char Operation
		{
			get { return operation; }
			set { operation = value; }
		}
		private double value;
		public double Value
		{
			get { return value; }
			set { this.value = value; }
		}
		#endregion
		public Element(Double value, char operation)
		{
			this.value = value;
			this.operation = operation;
		}
	}
}