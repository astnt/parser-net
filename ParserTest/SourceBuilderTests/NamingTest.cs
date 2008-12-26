using System.Text;
using NUnit.Framework;

namespace ParserTest.SourceBuilderTests
{
	[TestFixture]
	public class NamingTest : AbstractParserTest
	{
		[Test]
		public void NameInXmlAttributeTest()
		{
			string actual = Parse(@"@main[] $var[value] <option value=""$var"">$var</option>");
			Result(actual);
			Assert.AreEqual(actual, @"  <option value=""value"">value</option>");
		}
	}
}
