using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parser.Model.Context
{
	/// <summary>
	/// Класс для перебора IEnumerable
	/// </summary>
	public class Enum : IExecutable
	{
		private List<Object> list = new List<object>();
		private Executor exec;
		public void AddExecutor(Executor executor)
		{
			exec = executor;
		}
	}
}
