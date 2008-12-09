using NUnit.Framework;
using Parser;
using Parser.BuiltIn.Function;
using Parser.Facade;
using Parser.Model;

namespace ParserTest.SourceBuilderTests
{
	[TestFixture]
	public class IfTest : AbstractParserTest
	{
		[Test]
		public void GenericIfTest()
		{
			ParserFacade pf = new ParserFacade();
			pf.Parse(@"
@main[]
	$something[val]
	^if($something eq 'val'){
		something is equal<br/>
		value is $something
	}
");
			Model(pf.Dump());
			string actual = pf.Run();
			Result(actual);
			Assert.IsTrue(actual.Contains("something is equal"));
			Assert.IsTrue(actual.Contains("value is val"));
			Assert.IsTrue(!actual.Contains("{"));
		}
		[Test]
		public void ElseTest()
		{
			ParserFacade pf = new ParserFacade();
			pf.Parse(@"
@main[]
	$somevar[458]
	^if($somevar == 100){
		true
	}{
		false
	}
");
			Model(pf.Dump());
			string actual = pf.Run();
			Result(actual);
			Assert.IsTrue(actual.Contains("false"));
			Assert.IsTrue(!actual.Contains("true"));
		}
	}
}
