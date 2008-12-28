using System;
using System.Reflection;
using System.Text;
using NUnit.Framework;
using Parser.Util;
using ParserTest.SourceBuilderTests;

namespace ParserTest.Util
{
	[TestFixture]
	public class ReflectionUtilTest
	{
		[Test]
		public void MethodWithoutParamsSearchTest()
		{
			ReflectionUtil ru = new ReflectionUtil();
			MethodInfo info =  ru.SearchMethod(new StringBuilder(), new string[]{ "ToString" });
			Assert.IsNotNull(info);
		}
		[Test]
		public void PropertySearchTest()
		{
			VarsAccessTest.TestModel m = new VarsAccessTest.TestModel();
			ReflectionUtil ru = new ReflectionUtil();
			Object count = ru.SearchValue(m, new String[] { "Some", "Count" });
			Object text = ru.SearchValue(m, new String[] { "SomeValue" });
			Console.WriteLine("count: {0}, text: {1}", count, text);
			Assert.AreEqual(0, count);
			Assert.AreEqual("text-from-test-model-field", text);
		}
	}
}
