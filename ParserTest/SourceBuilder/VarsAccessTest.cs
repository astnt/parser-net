using NUnit.Framework;
using Parser.Model;
using Parser.Util;
using ParserTest;

namespace Parser.NETTest.SourceBuilder
{
	[TestFixture]
	public class VarsAccessTest : AbstractParserTest
	{
		[Test]
		public void SimpleVarTest()
		{
			string source = @"
@main[]
	$sb.ToString()
";
			Parser.SourceBuilder builder = new Parser.SourceBuilder();
			builder.Parse(source);
			Dumper d = new Dumper();
			Model(d.Dump((RootNode)builder.RootNode));

			Executor exec = new Executor();
			exec.Run((RootNode)builder.RootNode);
			string actual = exec.Output.ToString();

			Result(actual);
		}



	}
}
