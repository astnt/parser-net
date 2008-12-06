using NUnit.Framework;
using Parser.Facade;
using Parser.Model;
using Parser.Util;

namespace ParserTest.SourceBuilderTests
{
	[TestFixture]
	public class FuncCallTest : AbstractParserTest
	{
		[Test]
		public void FuncCallInConstructorTest()
		{
			string source = @"
@main[]
	$table[^table::excel[SELECT * FROM [Лист1^$A4:A7];^path[]]]
	some text
	^table.menu{
		<cell>^table.column(0)</cell>
	}

@path[]
../../resources/sample.xls

";
			ParserFacade pf = new ParserFacade();
			pf.Parse(source);

			string actual = pf.Run();
			Result(actual);
			Model((new Dumper()).Dump((RootNode) pf.Builder.RootNode));
		}
	}
}