using System;
using System.Reflection;
using Parser.BuiltIn.Function;

namespace Parser.Model
{
	/// <summary>
	/// Назвал функции, но ближе к "ветке" кода.
	/// </summary>
	public class Function : Node
	{

		#region vars

		private String name;
		public String Name
		{
			get { return name; }
			set { name = value; }
		}

		private ConstructorInfo refObject;
		/// <summary>
		/// Ссылка на объект, который будет осуществлять вычисления параметров.
		/// Или делать какие-то еще действия.
		/// TODO возможно не самый лучший способ и стоит переделать.
		/// </summary>
		public ConstructorInfo RefObject
		{
			get { return refObject; }
			set { refObject = value; }
		}

		#endregion


	}
}
