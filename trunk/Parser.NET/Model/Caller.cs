namespace Parser.Model
{
	/// <summary>
	/// Информация о вызове.
	/// </summary>
	public class Caller : AbstractNode, IName
	{

		#region vars

		private string funcName;
		private Params parameters;

		public string FuncName
		{
			get { return funcName; }
			set { funcName = value; }
		}

		public Params Parameters
		{
			get { return parameters; }
			set { parameters = value; }
		}

		#endregion

		public void SetName(string value)
		{
			FuncName = value;
		}

		public void SetStart(int? value)
		{
			// TODO старта не может быть 
		}

		public void SetParams(Params value)
		{
			Parameters = value;
		}
	}
}
