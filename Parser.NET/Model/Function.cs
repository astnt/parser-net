using System;
using System.Reflection;
using Parser.BuiltIn.Function;

namespace Parser.Model
{
	public class Function : Node, IName
	{

		#region vars

		private String name;
		public String Name
		{
			get { return name; }
			set { name = value; }
		}

		private Params parameters;
		public Params Parameters
		{
			get { return parameters; }
			set { parameters = value; }
		}

		private ConstructorInfo refObject;
		/// <summary>
		/// Ссылка на объект, который будет осуществлять вычисления параметров.
		/// </summary>
		public ConstructorInfo RefObject
		{
			get { return refObject; }
			set { refObject = value; }
		}

		

		#endregion

		#region IName implementation

		public void SetName(string value)
		{
			name = value;
		}

		public void SetStart(int? value)
		{
			Start = value;
		}

		public void SetParams(Params value)
		{
			Parameters = value;
		}

		#endregion

		

	}
}
