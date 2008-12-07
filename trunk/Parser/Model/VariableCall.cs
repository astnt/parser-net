namespace Parser.Model
{
	/// <summary>
	/// Вызов переменной $var | $var.of.something
	/// </summary>
	public class VariableCall : AbstractNode
	{

		#region vars

		private string[] name;

		public string[] Name
		{
			get { return name; }
			set { name = value; }
		}

		#endregion
	}
}
