namespace Parser.Model
{
	/// <summary>
	/// Хранилище текста.
	/// </summary>
	public class Text : Node
	{
		private bool taint = false;
		private string body;

		public string Body
		{
			get
			{
				// TODO возможно добавление свойства "статичности" для производительности
				if (taint)
				{
					return body;
				}
				else
				{
					return Escape(body);
				}
			}
			set
			{
				body = value;
			}
		}

		private string Escape(string source)
		{
			// TODO тут должно быть экейпинг, если текст "грязный"
			return source;
		}

		/// <summary>
		/// Показатель доверия к тексту.
		/// </summary>
		public bool Taint
		{
			get { return taint; }
			set { taint = value; }
		}

		public override void Close(int? indexOfEnd)
		{
			
		}
	}
}
