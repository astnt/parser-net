using System;
using System.Text;
using NUnit.Framework;
using Parser.Facade;
using Parser.Model.Context;

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
		[Test]
		public void PositionIndexOfTest()
		{
			string actual = Parse(@"
@main[]
	$var[Алтайский край (не включая г. Барнаул, г. Бийск, г. Новоалтайск и г. Рубцовск)]
	<pos>^var.pos[(не]</pos>
");
			Result(actual);
			Assert.IsTrue(actual.Contains("<pos>15</pos>"));
		}
		[Test]
		public void ContainsVariableTest()
		{
			string actual =
				Parse(
					@"
@main[]
	$var[Алтайский край (не включая г. Барнаул, г. Бийск, г. Новоалтайск и г. Рубцовск)]
	$var2[край]
	^if(^var.contains[$var2]){true}{error}
	

");
			Result(actual);
			Assert.IsTrue(actual.Contains("true"));
//			Assert.IsTrue(!actual.Contains("error"));
		}
		[Test]
		public void FromForeinVarTest()
		{
			ParserFacade pf = new ParserFacade();
			pf.Parse(@"
@main[]
	$this
	$var[^this.test[get]]

<value>$var</value>

	^if($var eq 'test'){true}{error}
	
");
			Model(pf.Dump());
			pf.AddVar("this", new Test());
			string actual = pf.Run();
			Result(actual);
			Assert.IsTrue(actual.Contains("<value>test</value>"));
			Assert.IsTrue(actual.Contains("true"));
			Assert.IsTrue(!actual.Contains("error"));
			// TODO //Assert.IsTrue(!actual.Contains("ParserTest.Model.Context.ParserStringTest+Test"));
		}
		public class Test
		{
			public ParserString test(StringBuilder value)
			{
				Console.WriteLine("Test.test() called");
				ParserString t = new ParserString("test");
				return t;
			}
		}
	}
}
