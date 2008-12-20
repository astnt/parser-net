using Parser.Model;

namespace Parser.BuiltIn.Function
{
	[ParserName("hash::create")]
	public class HashCreate : ICompute
	{
		public object Compute(Caller caller, Executor exec)
		{
			return new Hash();
		}
	}
}
