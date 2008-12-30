using NUnit.Framework;

namespace ParserTest.SourceBuilderTests
{
	[TestFixture]
	public class VarsMethodsAndCastsTest : AbstractParserTest
	{
		[Test]
		public void StringSplitTest()
		{
			string actual = Parse(@"
@main[]
	$str[что-то там]
	^str.split[6;3]
");
			Result(actual);
			Assert.IsTrue(actual.Contains("там"));
			Assert.IsTrue(!actual.Contains("что-то"));
		}
	}
}
