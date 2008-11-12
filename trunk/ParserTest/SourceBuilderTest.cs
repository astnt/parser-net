using System;
using System.Collections.Generic;
using NUnit.Framework;
using Parser;
using Parser.Model;
using Parser.Util;

namespace ParserTest
{
	/// <summary>
	/// Summary description for SourceBuilderTest
	/// </summary>
	[TestFixture]
	public class SourceBuilderTest : AbstractParserTest
	{

		[Test]
		public void FuncDecTest()
		{
			string source = @"@main[]
	Launch this func ^test[]

@test[]
	Some text for <html/>
";
			SourceBuilder builder = new SourceBuilder();
			builder.Parse(source);
			// создаем объект для дампа
			Dumper d = new Dumper();
			// выводим результат дампа
			Model(d.Dump((RootNode) builder.RootNode));
			// создаем исполнитель
			Executor exec = new Executor();
			// TODO проверить структуру Asseta'ми
			// запускаем выполнение ноды
			exec.Run((RootNode)builder.RootNode);
			// полученые строковый результат
			string actual = exec.Output.ToString();
			// выводим результат
			Result(actual);
			Assert.AreEqual(@"
	Launch this func 
	Some text for <html/>


", actual);
		}

		[Test]
		public void ParamsAndVarsTest()
		{
			SourceBuilder builder = new SourceBuilder();
			builder.Parse(@"
@main[]
	$myvar[abc]
	test is ^test[what;777]

@test[word;number]
	tested $word $number $myvar

");
			Dumper d = new Dumper();
			Model(d.Dump((RootNode) builder.RootNode));
			// TODO Assert для структуры
			Executor exec = new Executor();
			exec.Run((RootNode) builder.RootNode);
			string actual = exec.Output.ToString();
			Result(actual);
			Assert.AreEqual(@"
	
	test is 
	tested what 777 abc



", actual);
		}

		[Test]
		public void EvalSimpleTest()
		{
			SourceBuilder builder = new SourceBuilder();
			builder.Parse(@"
@main[]
	2 + 2 = ^eval(2+2)
");
			Dumper d = new Dumper();
			Model(d.Dump((RootNode)builder.RootNode));

			Executor exec = new Executor();
			exec.Run((RootNode)builder.RootNode);
			string actual = exec.Output.ToString();
			Result(actual);
			Assert.AreEqual(@"
	2 + 2 = 4
", actual);
		}

		[Test]
		public void ExcelTableMenuTest()
		{
			string actual = Parse(@"
@main[]
	$table[^table::excel[select * from list1A1:H1;sample.xls]]
	some text
	^table.menu{
		<cell>$table.0</cell>
	}
");
			Result(actual);
			Assert.AreEqual(@"
		<cell>one<cell>
		<cell>two<cell>
		<cell>three<cell>
", actual);
		}

		[Test]
		public void WhiteSpaceInParamsTest()
		{
			string actual = Parse(@"
@main[]
	^test[param is; good]

@test[test;add]
	some $test $add
");
			Result(actual);
			Assert.IsTrue(actual.Contains("some param is  good"));
		}

		[Test]
		public void EvalCallsTest()
		{
			string actual = Parse(@"@main[] 2+2=^eval(2+2) 3*9=^eval(3 * 9)");
			Result(actual);
			Assert.AreEqual(" 2+2=4 3*9=27", actual);
		}

		private string Parse(string source)
		{
			SourceBuilder builder = new SourceBuilder();
			builder.Parse(source);
			Dumper d = new Dumper();
			Model(d.Dump((RootNode)builder.RootNode));

			Executor exec = new Executor();
			exec.Run((RootNode)builder.RootNode);
			return exec.Output.ToString();
		}

		[Test]
		public void EscapingTest()
		{
			string source = @"@main[] ^^test[] text";
			Console.WriteLine(@"SOURCE{{{0}}}", source);
			string actual = Parse(source);
			Result(actual);
			Assert.AreEqual(@" ^test[] text", actual);
		}

		[Test]
		public void SimpleEscapingTest()
		{
			string source = @"@main[] $var[select * from list$1A1:H1;sample.xls] $var";
			string actual = Parse(source);
			Result(actual);
			Assert.IsTrue(actual.Contains("select * from list$1A1:H1;sample.xls"));
		}

		/// <summary>
		/// Тест переприсваивания.
		/// </summary>
		[Test]
		public void VarReAssignmentTest()
		{
			string actual = Parse(@"@main[] $test[first] $test $test[second] $test ");
			Result(actual);
			Assert.AreEqual("  first  second ", actual);
		}

	}
}
