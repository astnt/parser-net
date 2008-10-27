using System;
using System.Text;

namespace ParserTest
{
	public abstract class AbstractParserTest
	{
		internal void Result(string actual)
		{
			Console.WriteLine(String.Format("RESULT{{{0}}}", actual));
		}

		internal void Model(StringBuilder modelInString)
		{
			Console.WriteLine(modelInString);
		}
		
	}
}
