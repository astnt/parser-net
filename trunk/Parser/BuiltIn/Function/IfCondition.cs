using Parser.Model;

namespace Parser.BuiltIn.Function
{
	[ParserName("if")]
	public class IfCondition : IExecutable
	{
		private Executor exec;
		public void AddExecutor(Executor executor)
		{
			exec = executor;
		}
	}
}
