using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using NUnit.Framework;
using Parser.Util;

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
	}
}
