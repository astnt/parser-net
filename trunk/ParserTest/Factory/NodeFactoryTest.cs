using System;
using NUnit.Framework;
using Parser;
using Parser.Factory;
using Parser.Model;

namespace ParserTest.Factory
{
	[TestFixture]
	public class NodeFactoryTest
	{

		[Test]
		public void FuncNameCreatingTest()
		{
			NodeFactory factory;
			RootNode root;
			SourceBuilder sb = CreateBaseVars(out factory, out root);
			sb.source = @"@funcDec[]";

			AbstractNode func;
			if (factory.CreateNode(sb.source[0], root, out func))
			{
				string createdFuncName = ((Function)func).Name;
				Console.WriteLine(createdFuncName);
				Assert.AreEqual("funcDec", createdFuncName);
			}
			else
			{
				Assert.Fail();
			}
		}

		[Test]
		public void CallerNameCreatingTest()
		{
			NodeFactory factory;
			RootNode root;
			SourceBuilder sb = CreateBaseVars(out factory, out root);
			sb.source = @"^callSomething[]";

			AbstractNode caller;
			if (factory.CreateNode(sb.source[0], root, out caller))
			{
				string createdFuncName = ((Caller)caller).FuncName;
				Console.WriteLine(createdFuncName);
				Assert.AreEqual("callSomething", createdFuncName);
			}
			else
			{
				Assert.Fail();
			}
		}

		[Test]
		public void VariableNameCreatingTest()
		{
			NodeFactory factory;
			RootNode root;
			SourceBuilder sb = CreateBaseVars(out factory, out root);
			sb.source = @"$var[something in]";

			AbstractNode variable;
			if (factory.CreateNode(sb.source[0], root, out variable))
			{
				string createdFuncName = ((Variable)variable).Name;
				Console.WriteLine(createdFuncName);
				Assert.AreEqual("var", createdFuncName);
			}
			else
			{
				Assert.Fail();
			}
		}

		[Test]
		public void VariableWithOutputNameCreatingTest()
		{
			NodeFactory factory;
			RootNode root;
			SourceBuilder sb = CreateBaseVars(out factory, out root);
			sb.source = @"$var";
			CheckName(sb, factory, root, "var");

			sb = CreateBaseVars(out factory, out root);
			sb.source = "$something ";
			CheckName(sb, factory, root, "something");
		}

		private void CheckName(SourceBuilder sb, NodeFactory factory, RootNode root
			, string expected)
		{
			AbstractNode variable;
			if (factory.CreateNode(sb.source[0], root, out variable))
			{
				string createdFuncName = ((VariableCall)variable).Name[0];
				Console.WriteLine(String.Format("[{0}]", createdFuncName));
				Assert.AreEqual(expected, createdFuncName);
			}
			else
			{
				Assert.Fail();
			}
		}

		private static SourceBuilder CreateBaseVars(out NodeFactory factory, out RootNode root)
		{
			factory = new NodeFactory();
			SourceBuilder sb = new SourceBuilder();
			sb.CurrentIndex = 0;
			factory.Sb = sb;
			root = new RootNode();
			sb.RootNode = root;
			return sb;
		}

	}
}
