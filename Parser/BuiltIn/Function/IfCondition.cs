using System.Collections.Generic;
using Parser.Model;

namespace Parser.BuiltIn.Function
{
	[ParserName("if")]
	public class IfCondition : IExecutable, ICompute
	{
		private Executor exec;
		public void AddExecutor(Executor executor)
		{
			exec = executor;
		}
		public object Compute(List<object> vars)
		{

			return null;
		}
	}
}
