using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Parser.BuiltIn.Function;
using Parser.Model;

namespace Parser.Util
{
	public class BuiltInUtil
	{
		private Node rootNode;
		public  Node RootNode
		{
			set { rootNode = value; }
		}
		/// <summary>
		/// Добавляет встроенные функции.
		/// </summary>
		public void AddBuiltInMembers()
		{
			Assembly assembly = Assembly.GetExecutingAssembly();
			AddBuiltInMember("Parser.BuiltIn.Function.Eval", assembly);
			AddBuiltInMember("Parser.BuiltIn.Function.ExcelTableCreate", assembly);
			AddBuiltInMember("Parser.BuiltIn.Function.IfCondition", assembly);
			AddBuiltInMember("Parser.BuiltIn.Function.HashCreate", assembly);
		}
		/// <summary>
		/// Добавить встроенный метод.
		/// </summary>
		/// <param name="namespaceName">Неймспейс.</param>
		/// <param name="assembly">Сборка.</param>
		private void AddBuiltInMember(string namespaceName, Assembly assembly)
		{
			// TODO ниже временная запись
			// будет добавлено добавление статичных объектов по неймспейсу
			Function builtInFunc = new Function();
			builtInFunc.Parent = rootNode;

			Type parserType = assembly
				.GetType(String.Format(namespaceName)); // берем нужный тип Assembly

			ConstructorInfo ci = parserType.GetConstructor(new Type[0]);

			// TODO в теории ParserNameAttribute может быть не нулевым и аттрибута вообще может не быть
			ParserNameAttribute pna =
				(ParserNameAttribute)parserType.GetCustomAttributes(false)[0];

			builtInFunc.Name = pna.Name;
			builtInFunc.RefObject = ci;
			// добавляем в дерево
			rootNode.Add(builtInFunc);
		}
	}
}
