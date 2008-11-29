using System.Collections.Generic;
using NUnit.Framework;
using Parser.BuiltIn.Function;
using Parser.Facade;

namespace ParserTest.SourceBuilderTests
{
	[TestFixture]
	public class IfTest : AbstractParserTest, ICompute
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

		public object Compute(List<object> vars)
		{
			throw new System.NotImplementedException();
		}
	}
}
