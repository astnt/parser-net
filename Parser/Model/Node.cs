using System.Collections.Generic;

namespace Parser.Model
{
	/// <summary>
	/// Node with childs.
	/// </summary>
	public class Node : AbstractNode
	{
		private IList<AbstractNode> childs = new List<AbstractNode>();

		public IList<AbstractNode> Childs
		{
			get { return childs; }
			set { childs = value; }
		}

		public void Add(AbstractNode node)
		{
			Childs.Add(node);
			node.Parent = this;
		}

		public virtual void Close(int? indexOfEnd)
		{

		}

		#region vars
		// TODO вынести в Text

		private int? start;

		/// <summary>
		/// Start of node body.
		/// </summary>
		public int? Start
		{
			get { return start; }
			set { start = value; }
		}

		#endregion

	}
}
