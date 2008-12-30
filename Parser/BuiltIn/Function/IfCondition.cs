using System;
using Parser.Model;
using Parser.Model.Context;
using Parser.Syntax;

namespace Parser.BuiltIn.Function
{
	[ParserName(SyntaxName)]
	public class IfCondition : ICompute
	{
		public const string SyntaxName = "if";
		private Executor executor;
		public object Compute(Caller caller, Executor exec)
		{
			executor = exec;
			if (CheckExpression((Parametr)caller.Childs[0])) // UNDONE add Expression here
			{
				exec.Run(((Parametr)caller.Childs[1]).Childs);
			}
			else if(caller.Childs.Count > 2)
			{
				exec.Run(((Parametr)caller.Childs[2]).Childs);
			}
			return null;
		}
		/// <summary>
		/// Отработать выражение.
		/// </summary>
		/// <param name="param"></param>
		/// <returns></returns>
		private bool CheckExpression(Parametr param)
		{
			bool result = false;
			int index = 0;
			foreach (AbstractNode node in param.Childs)
			{
				Operator op = node as Operator;
				if(op != null)
				{
					String left = GetValue(param.Childs[index - 1]);
					String right = GetValue(param.Childs[index + 1]);
					if(op.Operation == Expressions.EqualString)
					{
						result = left == right;
					}
					if(op.Operation == Expressions.Equal)
					{
						result = left != null && left.Equals(right);
					}
				}
				index += 1; 
			}
			if (param.Childs != null && param.Childs.Count == 1)
			{
				executor.Run(param.Childs);
				return (Boolean) executor.Output;
			}
			return result;
		}

		private String GetValue(AbstractNode node)
		{
			if((node as Text) != null)
			{
				return ((Text) node).Body.TrimStart('\'').TrimEnd('\'');
			}
			if((node as VariableCall) != null)
			{
				Object variable = executor.ContextManager.GetVar((VariableCall)node);
				if(variable as ContextVariable != null)
				{
					return ((ContextVariable)variable).Value.ToString();
				}
				else
				{
					return variable.ToString();
				}
			}
			return null;
		}
	}
}
