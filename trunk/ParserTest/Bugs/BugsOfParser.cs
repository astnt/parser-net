using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Parser.Facade;
using Parser.Model;

namespace ParserTest.Bugs
{
	[TestFixture]
	public class BugsOfParser : AbstractParserTest
	{
		/// <summary>
		/// Баг возникающий в теле цикла - переменная определяется некорректно.
		/// </summary>
		[Test]
		public void VariableInMenuCycleBug()
		{
			string actual = Parse(@"
@main[]
	$table[^table::excel[SELECT * FROM [Лист1^$A4:A5];../../resources/sample.xls ]]
	$var[some text]
	^table.menu{
		<cell>^table.column[0]</cell><cell>$var</cell>
	}
", true);
			Result(actual);
			Assert.IsTrue(actual.Contains("<cell>Москва / МО</cell><cell>some text</cell>"));
		}
		[Test]
		public void TypesMethodsBugTest()
		{
			string source = @"
@main[]
$var[xxx]

$var
<br/>
^var.GetType[]
$str[^var.ToString[]]

^str.GetType[]

";
			string actual = Parse(source);
			Result(actual);

//			StringBuilder sb = new StringBuilder();
//			sb.Length
		}
		[Test]
		public void IndexInTableMenuBag()
		{
			string actual = Parse(
				@"@main[]

	$table2[^table::excel[SELECT * FROM [Лист1^$C4:C7];../../../ParserTest/resources/sample.xls ]]
	another text
 $table2

	$table2
<table border=""1"">
^table2.menu{
<tr>
<td>^table2.column[0]</td><td>$table2.Index</td><td>$table2.0</td>
</tr>
}
</table>");
			Result(actual);
			Assert.IsTrue(actual.Contains(@"<tr>
<td>30</td><td>0</td><td>30</td>
</tr>

<tr>
<td>20</td><td>1</td><td>20</td>
</tr>

<tr>
<td>20</td><td>2</td><td>20</td>
</tr>"));
		}
		[Test]
		public void EqualValuesIfBugTest()
		{
			Table<String> table = new Table<String>();
			table.Add(0,0,"value-from-table");

			ParserFacade pf = new ParserFacade();
			pf.Parse(@"@main[]
	^table.menu{ $table.0 - $table.Index  ^if($table.Index == 1){error}{true} ^if($table.Index == 0){true}{error} }
");
			Model(pf.Dump());
			pf.AddVar("table", table);
			String actual = pf.Run();
			Result(actual);
			Assert.IsTrue(actual.Contains("true"));
			Assert.IsTrue(!actual.Contains("error"));
		}
		[Test]
		public void VariableNameDefintionBugTest()
		{
			ParserFacade pf = new ParserFacade();
			pf.Parse(@"
@main[]
	<!--$test-->

");
			pf.AddVar("test", new List<String>());
			String actual = pf.Run();
			Result(actual);
			Assert.IsTrue(actual.Contains(@"<!--System.Collections.Generic.List`1[System.String]-->"));
		}
		[Test]
		public void IfVariableBugTest()
		{
			ParserFacade pf = new ParserFacade();
			pf.Parse(@"@main[]

	$hasMail[false]
  $hasAddr[true]
	
	$office[129]
	$mailto[^table::excel[select * from [Лист1^$A410:L413];../../resources/mailto.test.xls]]
	^mailto.menu{
	$var[^mailto.column[6]]
	$email[^mailto.column[11]]
	<to if-name=""office"" is=""^mailto.column[6]"" var=""$var"">$email</to>
	^if($var eq $office){ ^if(^email.contains[@]){ $hasMail[true] <cc>kn_kommersant@vtb24.ru</cc><!-- адрес для копий заявок сводного почтового ящика -->  } }
	}

	<!--<to>Oshutkova.DI@vtb24.ru</to>--> <!-- адрес в случае отсутствия получателя в точке продажи -->
	<to if-name=""office"" is=""null"">ast@design.ru</to>

	<subject>^if($hasMail eq 'false'){[! нет получателя в ТП^]} ВТБ24: Предварительная заявка на оформление кредита</subject>
	<body>
		^if($hasMail eq 'false'){
		   ^if($office eq 'null'){ }{ <p>Нет получателя в точке продажи, указанной в заявке. Заявка выслана только на данный адрес.</p> }
		}
		^if($office eq 'null'){
<p>В заявке не указана точка продажи. Заявка выслана только на данный адрес.</p>
		}
	</body>
office is $office $hasMail
");
			String actual = pf.Run();
			Result(actual);

			Assert.IsTrue(actual.Contains(@"office is 129 false"));
		}
	}
}