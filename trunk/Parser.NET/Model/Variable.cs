using System;

namespace Parser.Model
{
	/// <summary>
	/// Описание переменной.
	/// </summary>
	public class Variable : Node, IName
	{

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

		/// <summary>
		/// Параметр типа: $var[parametrs]
		/// </summary>
		private Params parameters;

		#endregion

		public void SetName(string setName)
		{
			name = setName;
		}

		public void SetStart(int? start)
		{
			// TODO ?
			// преравнивается к объекту
			if (parameters.Names.Length > 0)
			{
				value = parameters.Names[0];
			}
			else
			{
				value = String.Empty;
			}
		}

		public void SetParams(Params param)
		{
			parameters = param;
		}

	}
}
