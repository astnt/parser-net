using System;
using System.Text;

namespace Parser.Model.Context
{
	public class ParserString : IExecutable
	{
		public ParserString(String source)
		{
			value = new StringBuilder(source);
		}
		public ParserString(StringBuilder source)
		{
			value = source;
		}
		public Type GetType(Caller caller)
		{
			return GetType();
		}
		public String ToString(Caller caller)
		{
			return ToString();
		}
		public override String ToString()
		{
			return value.ToString();
		}
		/// <summary>
		/// Substring парсера.
		/// </summary>
		/// <param name="caller"></param>
		/// <returns></returns>
		public StringBuilder mid(Caller caller)
		{
//			$str[О, сколько нам открытий чудных!…]
//			^str.mid(3;20) 
			Int32 startIndex = Int32.Parse(((Text)((Parametr)caller.Childs[0]).Childs[0]).Body);
			Int32 lenght = Int32.Parse(((Text)((Parametr)caller.Childs[1]).Childs[0]).Body);

			return new StringBuilder(value.ToString().Substring(startIndex, lenght));
		}
		public Boolean contains(Caller caller)
		{
			Text text = ((Parametr) caller.Childs[0]).Childs[0] as Text;
			VariableCall variable = ((Parametr) caller.Childs[0]).Childs[0] as VariableCall;

			String valueFromParam = String.Empty;
			if (text != null) {
				valueFromParam = text.Body;
			}
			if(variable != null)
			{
				valueFromParam = ((ContextVariable)exec.ContextManager.GetVar(variable)).Value.ToString();

			}
			Boolean result = false;
			if(!String.IsNullOrEmpty(valueFromParam) && value.ToString().Contains(valueFromParam))
			{
				result = true;
			}
			return result;
		}
		/// <summary>
		/// Позиция подстроки.
		/// </summary>
		/// <param name="caller"></param>
		/// <returns></returns>
		public StringBuilder pos(Caller caller)
		{
			String example = ((Text)((Parametr)caller.Childs[0]).Childs[0]).Body;
			return new StringBuilder("" + value.ToString().IndexOf(example));
		}
		#region vars

		private StringBuilder value;
		private Executor exec;

		#endregion

		public void AddExecutor(Executor executor)
		{
			exec = executor;
		}
	}
}
