using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parser.Model
{
	/// <summary>
	/// Вызов переменной $var | $var.of.something
	/// </summary>
	public class VariableCall : AbstractNode
	{

		#region vars

		private string[] name;

		public string[] Name
		{
			get { return name; }
			set { name = value; }
		}

		#endregion
	}
}
