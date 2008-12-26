using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Parser.Util;

namespace ParserTest.Util
{
	[TestFixture]
	public class ReflectionUtilTest
	{
//		[Test]
		public void MethodSearchTest()
		{
			ReflectionUtil ru = new ReflectionUtil();
			ru.SearchMethod(new string[]{ "ToString" });
			Assert.Fail();
//			StringBuilder sb = new StringBuilder();
//			sb.GetType().GetMethod("ToString", new Type[]{});
		}
	}
}
