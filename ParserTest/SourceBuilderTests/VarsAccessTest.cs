using System;
using System.Collections.Generic;
using NUnit.Framework;
using Parser.Facade;
using Parser.Model;
using Parser.Util;

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
		/// <summary>
		/// Тестовая модель для проверки доступа к свойствам.
		/// </summary>
		public class TestModel
		{
			private string someValue = "text-from-test-model-field";
			public string SomeValue
			{
				get { return someValue; }
				set { someValue = value; }
			}
			public List<int> Some
			{
				get { return new List<int>(); }
			}
			public string testMethod()
			{
				return "text-from-test-method";
			}
		}
		[Test]
		public void PropertyAccessingTest()
		{
			ParserFacade pf = new ParserFacade();
			pf.Parse(@"
@main[]
цепочка $model.Some.Count - длинна List 	
значение $model.SomeValue из поля
");
//			TestModel m = new TestModel();
//			m.Some.Count
			Model(pf.Dump());
			pf.AddVar("model", new TestModel());
			string actual = pf.Run();
			Result(actual);
			Assert.IsTrue(actual.Contains((new TestModel()).Some.Count.ToString()));
			Assert.IsTrue(actual.Contains((new TestModel()).SomeValue));
		}
		[Test]
		public void IndexerAccessTest()
		{
			// UNDONE дописать остальную логику и Assert
			Table<string> table = new Table<string>();
//			MemberInfo indexer = myType.GetDefaultMembers()[0];
			table.Add(0, 0, "value-from-indexer");
			ReflectionUtil ru = new ReflectionUtil();
			Object result = ru.SearchValue(table, new String[] { "0" });
			Console.WriteLine("value is '{0}'", result);
			Assert.AreEqual("value-from-indexer", result); // UNDONE
		}
	}
}
