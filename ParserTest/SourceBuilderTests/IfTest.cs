using NUnit.Framework;
using Parser;
using Parser.BuiltIn.Function;
using Parser.Facade;
using Parser.Model;
using Parser.Syntax;

namespace ParserTest.SourceBuilderTests
{
	[TestFixture]
	public class IfTest : AbstractParserTest
	{
		[Test]
		public void GenericIfTest()
		{
			ParserFacade pf = new ParserFacade();
			pf.Parse(@"
@main[]
	$something[val]
	^if($something eq 'val'){
		something is equal<br/>
		value is $something
	}
");
			Model(pf.Dump());
			string actual = pf.Run();
			Result(actual);
			Assert.IsTrue(actual.Contains("something is equal"));
			Assert.IsTrue(actual.Contains("value is val"));
			Assert.IsTrue(!actual.Contains("{"));
		}
		[Test]
		public void ElseTest()
		{
			ParserFacade pf = new ParserFacade();
			pf.Parse(@"
@main[]
	$somevar[458]
	^if($somevar == 100){
		true
	}{
		false
	}
");
			Model(pf.Dump());
			string actual = pf.Run();
			Result(actual);
			Assert.IsTrue(actual.Contains("false"));
			Assert.IsTrue(!actual.Contains("true"));
		}
		[Test]
		public void FloatEqTest()
		{
			ParserFacade pf = new ParserFacade();
			pf.Parse(@"
@main[]
	$somevar[100]
	^if($somevar == 100){
		false
	}{
		true
	}
");
			Model(pf.Dump());
			string actual = pf.Run();
			Result(actual);
			Assert.IsTrue(actual.Contains("false"));
			Assert.IsTrue(!actual.Contains("true"));
		}
		[Test]
		public void EvalAndMethodsCallTest()
		{
			ParserFacade pf = new ParserFacade();
			pf.Parse(@"
@main[]
	$somevar[458]
	^if($somevar == 458 (^eval(2+2) == 2 && ^text[] eq 'some')){
		true
	}{
		false
	}

@text[]
some

");
			Model(pf.Dump());
			string actual = pf.Run();
			Result(actual);
//			Assert.IsTrue(actual.Contains("false"));
//			Assert.IsTrue(!actual.Contains("true"));
			Assert.Fail(); // UNDONE
		}
		/// <summary>
		/// Тестирует правильный разбор выражений в if
		/// </summary>
		[Test]
		public void SearchForExpressionsTest()
		{
			string source = "  eq 'val'";
			SourceBuilder sb = new SourceBuilder();
			RootNode root = new RootNode();
			Caller caller = new Caller();
			caller.Name = new string[]{ IfCondition.SyntaxName };
			root.Add(caller);
			Parametr param = new Parametr();
			caller.Add(param);
			sb.source = source;
			sb.CurrentText = new Text();
			sb.CurrentText.Start = 0;
			param.Add(sb.CurrentText);
			int index = sb.SearchForExpressions(source.Length, param);
			sb.CloseCurrentText(index);
			Model(root);

			// assert tree
			Assert.IsTrue(((Operator)param.Childs[0]).Operation == Expressions.EqualString);
			Assert.IsTrue(((Text)param.Childs[1]).Body == "'val'");
		}
		[Test]
		public void FloatIfTest()
		{
			string actual = Parse(@"
@main[]
	$f[1]
	^if($f eq '1' && ^test[] eq 'text'){true}{false}

@test[]text");
			Result(actual);
			Assert.IsTrue(actual.Contains("true"));
		}

	}
}
