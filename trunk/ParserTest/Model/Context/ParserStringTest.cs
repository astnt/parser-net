using NUnit.Framework;

namespace ParserTest.Model.Context
{
	[TestFixture]
	public class ParserStringTest : AbstractParserTest
	{
//		Алтайский край (не включая г. Барнаул, г. Бийск, г. Новоалтайск и г. Рубцовск)
//		г. Барнаул
		[Test]
		public void PositionAndContainsTest()
		{
			string actual = Parse(@"
@main[]
	$var[Алтайский край (не включая г. Барнаул, г. Бийск, г. Новоалтайск и г. Рубцовск)]
	^if(^var.contains[не включая]){^var.mid[0;^var.pos[не включая]]}
");
			Result(actual);
			Assert.IsTrue(actual.Contains("Алтайский край"));
			Assert.IsTrue(!actual.Contains("не включая"));
			Assert.IsTrue(!actual.Contains("г. Барнаул"));
		}
		[Test]
		public void ContainsTest()
		{
			string actual = Parse(@"
@main[]
	$var[г. Барнаул]
	^if(^var.contains[г. ]){true}{error}
	^if(^var.contains[fuck]){error}
");
			Result(actual);
			Assert.IsTrue(actual.Contains("true"));
			Assert.IsTrue(!actual.Contains("error"));
		}
	}
}
