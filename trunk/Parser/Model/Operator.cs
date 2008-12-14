using System;

namespace Parser.Model
{
	/// <summary>
	/// Части +-/* и true/false ("eq") выражения.
	/// </summary>
	public class Operator : AbstractNode
	{
		#region vars
		private string operation;
		public string Operation
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
		public Operator(Double value, string operation)
		{
			this.value = value;
			this.operation = operation;
		}
		public Operator(string operation)
		{
			this.operation = operation;
		}
	}
}