using System;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using Parser.Facade;
using Parser.Model;

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
цепочка $model.Some.Length - длинна List 	
значение $model.SomeValue из поля
");
			Model(pf.Dump());
			pf.AddVar("model", new TestModel());
			string actual = pf.Run();
			Result(actual);
			Assert.IsTrue(actual.Contains((new TestModel()).SomeValue));
		}
		[Test]
		public void IndexerAccessTest()
		{
			Type myType = typeof(Table<int>);
			MemberInfo[] memberInfoArray = myType.GetDefaultMembers();
			if (memberInfoArray.Length > 0)
			{
				foreach (MemberInfo memberInfoObj in memberInfoArray)
				{
					Console.WriteLine("The default member name is: " + memberInfoObj.ToString());
				}
			}
			Console.WriteLine("0, {0}", myType.GetDefaultMembers()[0].ToString());
			// UNDONE дописать остальную логику и Assert
			Table<string> table = new Table<string>();
			MemberInfo indexer = myType.GetDefaultMembers()[0];
			table.Add(0, 0, "value-from-indexer");
//			Console.WriteLine(indexer.);
//			MethodInfo index = indexer as MethodInfo;
//			object result = null;
//			if (index != null)
//			{
//				result = index.Invoke(table, new object[] { 0 });
//			}
//			Console.WriteLine("result is '{0}'", result);
		}
	}
}
