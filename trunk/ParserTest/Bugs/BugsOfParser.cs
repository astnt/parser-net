using System.Text;
using NUnit.Framework;

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
	}
}