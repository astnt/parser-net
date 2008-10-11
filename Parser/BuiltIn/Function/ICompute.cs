using Parser.Model;

namespace Parser.BuiltIn.Function
{
	/// <summary>
	/// Интерфейс для вычислений.
	/// </summary>
	public interface ICompute
	{
		object Compute(Params param);
	}
}
