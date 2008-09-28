using System;
using Parser.Model;
using Parser.Syntax;

namespace Parser.Builder
{
	abstract class Builder
	{

		#region vars

		internal IName buildedObject;
		static string Chars = String.Empty;

		/// <summary>
		/// Созданный объект.
		/// </summary>
		public IName BuildedObject
		{
			get { return buildedObject; }
			set { buildedObject = value; }
		}

		#endregion

		/// <summary>
		/// Проверяет целостность объявлений типа:
		/// ^nameOfFunc[], @nameOfFunc[], $nameOfVar
		/// </summary>
		/// <param name="c">Символ.</param>
		/// <returns>Не пробельный ли символ?</returns>
		internal bool IsObjectNameChar(char c)
		{
			if (
						 c == (char)32
					|| c == (char)160
					|| c == '\r'
					|| c == '\n'
					|| c == '\t'
				)
			{
				return false;
			}
			return true;
		}

		/// <summary>
		/// Создает объект с интерфейсом <see cref="IName"/>.
		/// </summary>
		/// <param name="sb"></param>
		/// <param name="source"></param>
		/// <returns>Создан ли?</returns>
		public virtual bool Build(SourceBuilder sb, string source)
		{
			bool isValid = false;
			bool hasName = false;
			Params param = null;
			int paramsStart = 0;
			for (int from = sb.CurrentIndex.Value; from < source.Length; from += 1)
			{
				char c = source[from];

				// ищем говно в опредении, если нашли — это не функция
				if (
						!IsObjectNameChar(c)
					)
				{
					// if has not name — not valid
					isValid = false;
					break;
				}

				// get func name
				if (
						(c == CharsInfo.ParamsStart || c == CharsInfo.ParamsEvalStart)
						&& !hasName
					)
				{
					// TODO FIXME в параметрах могут быть пробелы
					paramsStart = from + 1; // '['
					buildedObject.SetName(source.Substring(sb.CurrentIndex.Value, from - sb.CurrentIndex.Value));
					param = new Params();
					buildedObject.SetParams(param); // TODO создать параметры
					hasName = true;
				}

				// get params and close func declaration
				if (
						hasName
						&& (c == CharsInfo.ParamsEnd || c == CharsInfo.ParamsEvalEnd)
					)
				{
					param.Names = source.Substring(paramsStart, from - paramsStart).Split(';');
					// TODO добавить конструктор параметров
					buildedObject.SetStart(from + 1); // skip '@'
					// указываем новый индекс
					sb.CurrentIndex = from;
				}
			}

			// если нашли имя, то типа все ок
			// TODO еще параметры могут неправильно написаны.
			if (hasName)
			{
				isValid = true;
			}

			return isValid;
		}

	}
}