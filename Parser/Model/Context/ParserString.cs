using System;
using System.Text;

namespace Parser.Model.Context
{
	public class ParserString : IExecutable
	{
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
		public StringBuilder mid(Caller caller)
		{
//			$str[О, сколько нам открытий чудных!…]
//			^str.mid(3;20) 
			Int32 startIndex = Int32.Parse(((Text)((Parametr)caller.Childs[0]).Childs[0]).Body);
			Int32 lenght = Int32.Parse(((Text)((Parametr)caller.Childs[1]).Childs[0]).Body);

			return new StringBuilder(value.ToString().Substring(startIndex, lenght));
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
