using NUnit.Framework;
using Parser.Facade;

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
			Assert.IsTrue(actual.Contains(new TestModel().testMethod()));
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
