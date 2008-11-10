namespace Parser.Model
{
	/// <summary>
	/// Информация о вызове.
	/// </summary>
	public class Caller : Node
	{

		private string[] name;

		/// <summary>
		/// .some.name.of.func
		/// </summary>
		public string[] Name
		{
			get { return name; }
			set { name = value; }
		}

	}
}
