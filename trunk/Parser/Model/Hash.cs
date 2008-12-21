using System;
using System.Collections.Generic;
using System.Text;

namespace Parser.Model
{
	public class Hash : IExecutable
	{
		private Dictionary<String, Object> dictionary = new  Dictionary<String, Object>();
		public Hash()
		{
			
		}
		public void addKey(Caller caller)
		{
			string key = ((Text) ((Parametr) caller.Childs[0]).Childs[0]).Body;
			string value = ((Text)((Parametr)caller.Childs[1]).Childs[0]).Body;
			if(dictionary.ContainsKey(key))
			{
				dictionary[key] = value;
			}
			else
			{
				dictionary.Add(key, value);
			}
		}
		public string getKey(Caller caller)
		{
			StringBuilder key = new StringBuilder(((Text)((Parametr)caller.Childs[0]).Childs[0]).Body);
			return dictionary[key.ToString()].ToString();
		}
		public void each(Caller caller)
		{
			string variableNameOfKey = ((Text)((Parametr)caller.Childs[0]).Childs[0]).Body; 
			string variableNameOfValue = ((Text)((Parametr)caller.Childs[1]).Childs[0]).Body;
			Parametr bodyOfEachCycle = ((Parametr)caller.Childs[2]);
			foreach (KeyValuePair<string, object> pair in dictionary)
			{
				// update переменных в начале цикла.
				exec.ContextManager.AddVar(variableNameOfKey, pair.Key);
				exec.ContextManager.AddVar(variableNameOfValue, pair.Value);
				// выполнение тела цикла.
				exec.Run(bodyOfEachCycle.Childs);
			}
		}
		private Executor exec;
		public void AddExecutor(Executor executor)
		{
			exec = executor;
		}
	}
}
