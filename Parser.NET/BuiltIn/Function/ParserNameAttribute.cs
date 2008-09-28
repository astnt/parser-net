using System;

namespace Parser.BuiltIn.Function
{
	/// <summary>
	/// Как называется функция в тексте обрабатываемого файла парсером.
	/// </summary>
	public class ParserNameAttribute : Attribute
	{

		#region vars

		private string name;

		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		#endregion

		public ParserNameAttribute(string name)
		{
			this.name = name;
		}

	}
}
