using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parser.Model.Context
{
	public class ContextVariable : Variable
	{
		public ContextVariable(string name, object value) : base (name, value)
		{
			
		}
		public ContextVariable()
		{
			
		}
	}
}
