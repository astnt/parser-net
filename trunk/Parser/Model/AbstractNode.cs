namespace Parser.Model
{
	/// <summary>
	/// Абстрактный узел парсера.
	/// </summary>
	public abstract class AbstractNode
	{
		private Node parent;

		/// <summary>
		/// Parent AbstractNode.
		/// </summary>
		public Node Parent
		{
			get { return parent; }
			set { parent = value; }
		}
	}
}
