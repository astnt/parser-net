using NUnit.Framework;

namespace ParserTest
{
	[TestFixture]
	public class HashTest : AbstractParserTest
	{
		[Test]
		public void GenericTest()
		{
			string actual = Parse(
				@"
@main[]
	$man[^hash::create[]]

	<p>Name of class is $man</p>

	^man.addKey[name;Вася]
	^man.addKey[age;22]
	^man.addKey[sex;m]

	name=^man.getKey[name]
	age=^man.getKey[age]
	sex=^man.getKey[sex]

", true);
/*
	$.name[Вася] 
	$.age[22] 
	$.sex[m] 
	^man.foreach[key;value]{ 
					$key=$value 
	}[<br />]
		 
*/
			Result(actual);
			Assert.IsTrue(actual.Contains("name=Вася"));
			Assert.IsTrue(actual.Contains("age=22"));
			Assert.IsTrue(actual.Contains("sex=m"));
		}
	}
}
