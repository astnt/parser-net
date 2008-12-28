﻿using System.Text;
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
	}
}