using System;

namespace Parser.Model
{
	/// <summary>
	/// Описание переменной.
	/// </summary>
	public class Variable : Node
	{

		public Variable()
		{
			
		}
		public Variable(string name, object value)
		{
			this.name = name;
			this.value = value;
		}

		#region vars

		private string name;
		/// <summary>
		/// Имя (возможно path.to.var?)
		/// </summary>
		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		private object value;
		/// <summary>
		/// Некий обощенный хранимый объект.
		/// </summary>
		public object Value
		{
			get { return value; }
			set { this.value = value; }
		}

		#endregion

	}
}
