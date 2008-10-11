using System;
using NUnit.Framework;
using Parser.BuiltIn.Function;
using Parser.Model;

namespace Parser.NETTest.BuiltIn.Function
{
	[TestFixture]
	public class EvalTest
	{
		[Test]
		public void SpacingCharTest()
		{
			foreach (char c in ((char)160 + " \t\r\n").ToCharArray())
			{
				Assert.IsTrue(Eval.IsSpacing(c));
			}
		}
		[Test]
		public void IsDigitTest()
		{
			foreach (char c in ("1234567890").ToCharArray())
			{
				Assert.IsTrue(Eval.IsDigit(c));
			}
		}

		private Eval e;
		[Test]
		public void ExpressionsTest()
		{
			e = new Eval();
			Assert.AreEqual(0, Try("2 - 2"));
			Assert.AreEqual(0, Try("2-2"));
			Assert.AreEqual(4, Try("2 + 2"));
		}

		private double Try(string expression)
		{
			double result = e.Compute(expression);
			Console.WriteLine("{0} = {1}", expression, result);
			return result;
		}

	}
}