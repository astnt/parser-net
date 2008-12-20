using System;
using System.Text;
using Parser;
using Parser.Model;
using Parser.Util;

namespace ParserTest
{
	public abstract class AbstractParserTest
	{

		internal void Result(string actual)
		{
			Console.WriteLine(String.Format("RESULT{{{0}}}", actual));
		}

		internal void Model(RootNode node)
		{
			Dumper d = new Dumper();
			Model(d.Dump(node));
		}

		internal void Model(StringBuilder modelInString)
		{
			Console.WriteLine(modelInString);
		}

		internal string Parse(string source)
		{
			return Parse(source, true);
		}
		internal string Parse(string source, Boolean hasModelOutput)
		{
			Parser.SourceBuilder builder = new Parser.SourceBuilder();
			builder.Parse(source);
			Dumper d = new Dumper();
			if(hasModelOutput){
				StringBuilder model = d.Dump((RootNode) builder.RootNode);
				Model(model);
			}
			Executor exec = new Executor();
			exec.Run((RootNode)builder.RootNode);
			return exec.TextOutput.ToString();
		}
		
	}
}
