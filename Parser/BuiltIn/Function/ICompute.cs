using Parser.Model;

namespace Parser.BuiltIn.Function
{
	/// <summary>
	/// Интерфейс для вычислений парсерных конструкций.
	/// </summary>
	public interface ICompute
	{
		object Compute(Caller caller, Executor exec);
	}
}
