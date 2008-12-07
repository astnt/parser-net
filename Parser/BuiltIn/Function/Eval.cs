using System;
using System.Collections.Generic;
using System.Text;
using Parser;
using Parser.Model;

namespace Parser.BuiltIn.Function
{
	/// <summary>
	/// Парсер выражений типа 231 - (343 + 34234) + 343 * 232;
	/// TODO правильная приоретность (скобки, умножения), обозначения переменных
	/// по-идее если выражение статичное то быть вычисляться еще <see cref="SourceBuilder"/>
	/// </summary>
	[ParserName("eval")]
	public class Eval : ICompute
	{
		public object Compute(Caller caller, Executor exec)
		{
			Parametr param = caller.Childs[0] as Parametr;
			if(param == null || param.Childs.Count == 0)
			{
				
			}
			// param.Names[0] - это "параметр" ^eval(45-45*12), то есть "45-45*12"
			string expressionInText = null;
			StringBuilder expressionIn = null;
			Text abstractNode = param.Childs[0] as Text;
			if (abstractNode != null)
			{
				expressionInText = abstractNode.Body;
			}
			// TODO ниже - корявая конструкция
			if (expressionIn != null)
			{
				expressionInText = expressionIn.ToString();
			}
			if(expressionInText != null)
			{
				return Compute(GetExpression(expressionInText)).ToString();
			}
			return null;
		}

		public double Compute(string expression)
		{
			return Compute(GetExpression(expression));
		}

		/// <summary>
		/// Выполнение вычисления.
		/// TODO сохранять закэшированными
		/// </summary>
		/// <param name="expression">2+2</param>
		/// <returns>=4</returns>
		public Expression GetExpression(string expression)
		{
			//Console.WriteLine(expression);
			DigitMatch digitMatch = new DigitMatch();
			digitMatch.Operation = '+';
			Expression me = new Expression();
			for (int position = 0; position < expression.Length; position += 1)
			{
				char c = expression[position];
				bool isDigit = IsDigit(c);
				if (isDigit)
				{
					if (digitMatch.Length == 0)
					{
						digitMatch.Start = position;
					}
					digitMatch.Length += 1;
				}
				bool isOperator = IsOperator(c);
				// если прерывается пробельными символами
				if (IsSpacing(c) || isOperator || position + 1 == expression.Length)
				{
					string digitToParse = expression.Substring(digitMatch.Start, digitMatch.Length);
					double digit;
					//Console.WriteLine("digit {0}", digitToParse);
					if (Double.TryParse(digitToParse, out digit))
					{
						Element element = new Element(digit, digitMatch.Operation);
						//Console.WriteLine("digit {0} oper '{1}'", digit, digitMatch.Operation);
						me.Left(element);
						digitMatch = new DigitMatch();
					}
				}
				if (isOperator)
				{
					digitMatch.Operation = c;
				}
			}
			return me;
		}

		private double Compute(Expression me)
		{
			Double result = 0;
			foreach (Element element in me)
			{
				switch (element.Operation)
				{
					case '+':
						result += element.Value;
						break;
					case '*':
						result *= element.Value;
						break;
					case '-':
						result -= element.Value;
						break;
					case '/':
						result /= element.Value;
						break;
				}
			}
			return result;
		}
		/// <summary>
		/// 
		/// </summary>
		private static bool IsOperator(char c)
		{
			return "*+/-".Contains(c.ToString());
		}
		/// <summary>
		/// 
		/// </summary>
		public static bool IsDigit(char c)
		{
			return 47 < c && c < 58;
		}
		/// <summary>
		/// ? \r\n\t 160(char)
		/// </summary>
		public static bool IsSpacing(char c)
		{
			return (((char)160) + " \r\n\t").Contains(c.ToString());
		}
		/// <summary>
		/// 
		/// </summary>
		private struct DigitMatch
		{
			public int Start;
			public int Length;
			public char Operation;
		}
	}
}
