using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parser.Model
{
	public class Hash
	{
//		private Dictionary<StringBuilder, Object> dictionary = new  Dictionary<StringBuilder, Object>();
		private Dictionary<String, Object> dictionary = new  Dictionary<String, Object>();
		public Hash()
		{
			
		}
		public void addKey(StringBuilder key, StringBuilder value)
		{
			if(dictionary.ContainsKey(key.ToString()))
			{
				dictionary[key.ToString()] = value;
			}
			else
			{
				dictionary.Add(key.ToString(), value);
			}
		}
		public string getKey(StringBuilder key)
		{
			return dictionary[key.ToString()].ToString();
		}
	}
}
