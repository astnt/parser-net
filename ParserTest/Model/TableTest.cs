using System;
using NUnit.Framework;
using Parser.Model;

namespace ParserTest.Model
{
	[TestFixture]
	public class TableTest
	{
		[Test]
		public void SetAndGetCellsTest()
		{
			Table<string> t = new Table<string>();
			t.Add(0, 0, "first");
			t.Add(0, 1, "second");
			t.Add(1, 0, "third");
			t.Add(1, 1, "fourth");
			//foreach (string s in t)
			//{
				
			//}
			Console.WriteLine(t[0, 0]); Assert.AreEqual("first", t[0, 0]);
			Console.WriteLine(t[0, 1]); Assert.AreEqual("second", t[0, 1]);
			Console.WriteLine(t[1, 0]); Assert.AreEqual("third", t[1, 0]);
			Console.WriteLine(t[1, 1]); Assert.AreEqual("fourth", t[1, 1]);
			// TODO assert foreach
//			Parametr param = new Parametr();
//			t.menu(param);
		}
	}
}
