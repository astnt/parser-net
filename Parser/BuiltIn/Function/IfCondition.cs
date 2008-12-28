using Parser.Model;
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
//				Text text = node as Text;
//				if(text != null && String.IsNullOrEmpty(text.Body))
//				{
//					continue;
//				}
				Operator op = node as Operator;
				if(op != null)
				{
					if(op.Operation == Expressions.EqualString)
					{
						string left = GetValue(param.Childs[index - 1]);
						string right = GetValue(param.Childs[index + 1]);
						result = left == right;
					}
					if(op.Operation == Expressions.Equal)
					{
						object left = GetValue(param.Childs[index - 1]);
						object right = GetValue(param.Childs[index + 1]);
//						result = left == right;
						result = left.Equals(right);
					}
				}
				index += 1; 
			}
			return result;
		}

		private string GetValue(AbstractNode node)
		{
			if((node as Text) != null)
			{
				return ((Text) node).Body.TrimStart('\'').TrimEnd('\'');
			}
			if((node as VariableCall) != null)
			{
				Variable variable = executor.ContextManager.GetVar((VariableCall)node);
				return variable.Value.ToString();
			}
			return null;
		}
	}
}
