using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parser.Model
{
	/// <summary>
	/// Отмечает модели, содержащие <see cref="Executor"/>,
	/// и выполняющие код дерева парсера.
	/// </summary>
	interface IExecutable
	{
		void AddExecutor(Executor executor);
	}
}
