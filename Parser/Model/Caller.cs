namespace Parser.Model
{
	/// <summary>
	/// Информация о вызове.
	/// </summary>
	public class Caller : Node
	{

		private string funcName;

		public string FuncName
		{
			get { return funcName; }
			set { funcName = value; }
		}


	}
}
