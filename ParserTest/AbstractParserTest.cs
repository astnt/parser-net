using System;
using System.Text;
using Parser.Model;
using Parser.Util;

namespace ParserTest
{
	public abstract class AbstractParserTest
	{
		internal void Result(string actual)
		{
			Console.WriteLine(String.Format("RESULT{{{0}}}", actual));
		}

		internal void Model(RootNode node)
		{
			Dumper d = new Dumper();
			Model(d.Dump(node));
		}

		internal void Model(StringBuilder modelInString)
		{
			Console.WriteLine(modelInString);
		}
		
	}
}
