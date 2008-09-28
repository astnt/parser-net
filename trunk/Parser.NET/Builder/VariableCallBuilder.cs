using Parser.Model;
using Parser.Syntax;

namespace Parser.Builder
{
	class VariableCallBuilder : Builder
	{
		/// <summary>
		/// Создает описание вызова переменной.
		/// </summary>
		public VariableCallBuilder()
		{
			buildedObject = new Variable();
		}

		public override bool Build(SourceBuilder sb, string source)
		{
			bool isValid = false;
			bool hasName = false;
			for (int from = sb.CurrentIndex.Value; from < source.Length; from += 1)
			{
				char c = source[from];

				// если начались параметры - это не вызов
				// TODO возможно это должно быть оптимизировано сменой индекса в SourceBuilder
				if(c == CharsInfo.ParamsStart || c == CharsInfo.ParamsEnd)
				{
					break;
				}

				// ищем говно в опредении, если нашли — это не функция
				if (
						!IsObjectNameChar(c)
					)
				{
					// if has not name — not valid
					buildedObject.SetName(source.Substring(sb.CurrentIndex.Value, from - sb.CurrentIndex.Value));
					hasName = true;
					sb.CurrentIndex = from - 1; // чтобы не убивало пробелы
					break;
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
