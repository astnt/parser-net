namespace Parser.Model
{
	/// <summary>
	/// Интерфейс для объетов name-объектов типа:
	/// ^callFunc[], @func[], $variable
	/// </summary>
	interface IName
	{
		void SetName(string value);
		void SetStart(int? value);
		void SetParams(Params value);
	}
}
