using System;
using Parser.Model;
using Parser.Syntax;

namespace Parser.Factory
{
	/// <summary>
	/// Сборщик нод.
	/// </summary>
	public class NodeFactory
	{

		#region vars

		private SourceBuilder sb;

		public SourceBuilder Sb
		{
			get { return sb; }
			set { sb = value; }
		}

		private bool hasParams = false;

		#endregion

		/// <summary>
		/// Похоже, что это нода.
		/// Пытаемся создать.
		/// </summary>
		/// <param name="c">Объявляющий символ.</param>
		/// <param name="node">Текущая нода.</param>
		/// <param name="createdNode">Созданная нода.</param>
		/// <returns>Создана ли нода?</returns>
		public bool CreateNode(char c, Node node, out AbstractNode createdNode)
		{
			bool IsNodeCreated = false;
			createdNode = null;
			string name;
			if(CreateName(c, out name))
			{
				if(c == CharsInfo.FuncDeclarationStart)
				{
					// @func[] | @func[param1;param2] | @func[][local;var]
					createdNode = CreateNode(new Function(), name);
				}
				if(c == CharsInfo.CallerDeclarationStart)
				{
					// ^something[] | ^something[ ^if(true){ ... } ]
					createdNode = CreateNode(new Caller(), name);
				}
				if(c == CharsInfo.VariableDeclarationStart)
				{
					// $var[something] | $var[ ^if(false){}{ ... }] | $var 
					if (hasParams)
					{
						createdNode = CreateNode(new Variable(), name);
					}
					else
					{
						createdNode = CreateNode(new VariableCall(), name);
					}
				}
				IsNodeCreated = true;
			}
			return IsNodeCreated;
		}

		private AbstractNode CreateNode(VariableCall varCall, string name)
		{
			varCall.Name = name.Split('.');
			return varCall;
		}

		private AbstractNode CreateNode(Variable variable, string name)
		{
			variable.Name = name;
			return variable;
		}

		private AbstractNode CreateNode(Caller caller, string name)
		{
			caller.Name = name.Split('.');
			return caller;
		}

		private Node CreateNode(Function function, string name)
		{
			function.Name = name;
			return function;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="c">Объявляющий символ.</param>
		/// <param name="name"></param>
		/// <returns></returns>
		private bool CreateName(char c, out string name)
		{
			hasParams = true;
			bool isCreated;

			int index = sb.CurrentIndex.Value;
			int defIndex = sb.CurrentIndex.Value;
			while(true)
			{
				// если за границами строки
				if(sb.source.Length <= index)
				{
					hasParams = false;
					isCreated = IfVarThenCreate(index, c, defIndex, out name);
					break;
				}
				char current = sb.source[index];
				if(CharsInfo.IsInSpaceChars(current)
					||	current == '<'
					||	current == '"'
					||	current == '-'
					) // TODO добавить остальные символы
				{
					hasParams = false;
					isCreated = IfVarThenCreate(index, c, defIndex, out name);
					break;
				}
				if(
						 current == CharsInfo.ParamsStart
					|| current == CharsInfo.ParamsEvalStart
					|| current == CharsInfo.ParamsCodeStart
					)
				{
					// + обрезаем лишнее
					name = GetName(index, defIndex);
					isCreated = true;
					// сдвигаем обработку парсера
					sb.CurrentIndex = index; // + 1 ']'
					break;
				}
				index += 1;
			}
			return isCreated;
		}

		private bool IfVarThenCreate(int index, char c, int defIndex, out string name)
		{
			bool isCreated = false;
			name = String.Empty;
			if(c == CharsInfo.VariableDeclarationStart)
			{
				name = GetName(index, defIndex);
				isCreated = true;
			}
			sb.CurrentIndex = index - 1; // HACK ?
			return isCreated;
		}

		private string GetName(int index, int defIndex)
		{
			string name;
			name = sb.source.Substring(defIndex + 1, index - 1 - defIndex);
			return name;
		}
	}
}
