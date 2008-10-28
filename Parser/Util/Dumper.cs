using System;
using System.Collections.Generic;
using System.Text;
using Parser.Model;

namespace Parser.Util
{
	/// <summary>
	/// Визуальное отображение объектов.
	/// </summary>
	public class Dumper
	{

		#region vars
		
		private StringBuilder dumpResult;
		private StringBuilder tabs;

		private const string spaces = "  ";

		private Boolean isRemoveSpaces = true;

		/// <summary>
		/// При выдаче текстовой ноды убираеть пробелы?
		/// По-молчанию - да.
		/// </summary>
		public bool IsRemoveSpaces
		{
			get { return isRemoveSpaces; }
			set { isRemoveSpaces = value; }
		}

		#endregion

		public StringBuilder Dump(RootNode rootNode)
		{
			dumpResult = new StringBuilder();
			tabs = new StringBuilder();
			DumpChilds(rootNode.Childs);
			return dumpResult;
		}

		/// <summary>
		/// Перебирает детей, и добаляет их в строковый результат.
		/// </summary>
		/// <param name="childs">Дети ноды.</param>
		private void DumpChilds(IEnumerable<AbstractNode> childs)
		{
			foreach (AbstractNode node in childs)
			{
				dumpResult.Append(tabs);
				Function function = node as Function;
				if (function != null)
				{
					// TODO параметры
					dumpResult.AppendFormat("({0})@{1}[](Childs-{2})", function.GetType(), function.Name /*,function.Parameters.Names.Length*/,function.Childs.Count);
					DumpChilds(function);
				}
				Text text = node as Text;
				if (text != null)
				{
					dumpResult.AppendFormat("({0}){{{1}}}",text.GetType(),DumpText(text.Body));
				}
				Caller caller = node as Caller;
				if (caller != null)
				{
					// TODO параметры
					dumpResult.AppendFormat("({0})^{1}[]",caller.GetType(),caller.FuncName /*,caller.Parameters.Names.Length*/);
					DumpChilds(caller);
				}
				Variable variable = node as Variable;
				if(variable != null)
				{
					// TODO if не нужен, т.к. вызов переменной - varCall
					if (variable.Value != null)
					{
						dumpResult.AppendFormat("({0})${1}[{2}]", variable.GetType(), variable.Name, variable.Value);
					}
					else
					{
						dumpResult.AppendFormat("({0})${1}", variable.GetType(), variable.Name);
					}
					DumpChilds(variable);
				}
				VariableCall varCall = node as VariableCall;
				if(varCall != null)
				{
					dumpResult.AppendFormat("({0})${1}", varCall.GetType(), Dump(varCall.Name));
				}
				Parametr parametr = node as Parametr;
				if (parametr != null)
				{
					dumpResult.AppendFormat("({0})", parametr.GetType());
					DumpChilds(parametr);
				}
				dumpResult.Append('\n');
			}
		}

		private object Dump(string[] names)
		{
			StringBuilder result = new StringBuilder();
			foreach (string name in names)
			{
				result.AppendFormat(".{0}",name);
			}
			return result;
		}

		private void DumpChilds(Node function)
		{
			if (function.Childs != null)
			{
				dumpResult.Append('\n');
				tabs.Append(spaces);
				DumpChilds(function.Childs);
				tabs.Remove(tabs.Length - spaces.Length, spaces.Length);
			}
		}

		/// <summary>
		/// Добавляет текст, удаляет пробельные символы если это указано в свойстве <see cref="isRemoveSpaces"/>.
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		private string DumpText(string text)
		{
			if(isRemoveSpaces && text != null)
			{
				return text
					.Replace("\r", "")
					.Replace("\n", "")
					;
			}
			return text;
		}

	}
}
