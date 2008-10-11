namespace Parser.Model
{
	/// <summary>
	/// Параметры для <see cref="Function"/>, <see cref="Caller"/>.
	/// </summary>
	public class Params
	{

		#region vars

		private string[] names;

		/// <summary>
		/// Имена перечиcленные через ';'.
		/// </summary>
		public string[] Names
		{
			get { return names; }
			set { names = value; }
		}

		#endregion


	}
}
