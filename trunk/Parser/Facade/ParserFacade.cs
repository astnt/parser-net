using System;
using System.Text;
using Parser.Model;
using Parser.Model.Context;
using Parser.Util;

namespace Parser.Facade
{
	/// <summary>
	/// Облегчение работы с парсером.
	/// </summary>
	public class ParserFacade
	{
		private SourceBuilder builder;
		private Executor exec;

		public SourceBuilder Builder
		{
			get { return builder; }
			set { builder = value; }
		}

		/// <summary>
		/// Разбирает строку и создает из нее модель.
		/// </summary>
		/// <param name="source">Строка исходника.</param>
		public void Parse(string source)
		{
			Builder = new SourceBuilder();
			Builder.Parse(source);
			exec = new Executor();
		}
		/// <summary>
		/// Создает строку описывающую модель парсера.
		/// </summary>
		/// <returns>И возвращает ее.</returns>
		public StringBuilder Dump(){
			Dumper d = new Dumper();
			return d.Dump((RootNode) Builder.RootNode);
		}
		/// <summary>
		/// Добавить переменную.
		/// </summary>
		public void AddVar(string name, object value)
		{
			if(string.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("Can't add variable with empty name");
			}
			ContextVariable var = new ContextVariable();
			var.Name = name;
			var.Value = value;
			if(value as IExecutable != null)
			{
				((IExecutable)value).AddExecutor(exec);
			}
			exec.ContextManager.AddVar(var);
		}
		/// <summary>
		/// Выполняет модель парсера.
		/// </summary>
		/// <returns>Результирующая строка.</returns>
		public string Run()
		{
			exec.Run((RootNode)Builder.RootNode);
			return exec.TextOutput.ToString();
		}

	}
}
