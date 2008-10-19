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

		private int? start;
		private int? end;

		/// <summary>
		/// End of node body.
		/// </summary>
		public int? End
		{
			get { return end; }
			set { end = value; }
		}

		/// <summary>
		/// Start of node body.
		/// </summary>
		public int? Start
		{
			get { return start; }
			set { start = value; }
		}

	}
}
