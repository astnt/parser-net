using System;

namespace Parser.NET.Model
{
	/// <summary>
	/// Части мат. выражения.
	/// </summary>
	public class MathElement
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

		public MathElement(Double value, char operation)
		{
			this.value = value;
			this.operation = operation;
		}
	}
}