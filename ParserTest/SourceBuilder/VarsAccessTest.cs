using System.Text;
using NUnit.Framework;
using Parser;
using Parser.Facade;
using Parser.Model;
using Parser.Util;
using ParserTest;

namespace ParserTest.SourceBuilderTests
{
	[TestFixture]
	public class VarsAccessTest : AbstractParserTest
	{
		[Test]
		public void GenericAccessTest()
		{
			string source = @"
@main[]
	модель - $something вызов метода:
	^something.testMethod()
";
			ParserFacade pf = new ParserFacade();
			pf.Parse(source);
			Model(pf.Dump());

			pf.AddVar("something", new TestModel());
			string actual = pf.Run();

			Result(actual);
			// содержит описание типа, если указана только сама переменная
			Assert.IsTrue(actual.Contains(new TestModel().GetType().ToString()));
		}

		public class TestModel
		{
			private string someValue = "text-from-test-model";
			public string SomeValue
			{
				get { return someValue; }
				set { someValue = value; }
			}
			public string testMethod()
			{
				return "text-from-test-method";
			}
		}

	}
}
