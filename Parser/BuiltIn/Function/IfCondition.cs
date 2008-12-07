using System.Collections.Generic;
using Parser.Model;

namespace Parser.BuiltIn.Function
{
	[ParserName("if")]
	public class IfCondition : ICompute
	{
		public object Compute(Caller caller, Executor exec)
		{
			if(true) // UNDONE add Expression here
			{
				exec.Run(((Parametr)caller.Childs[1]).Childs);
			}
			return null;
		}
	}
}
