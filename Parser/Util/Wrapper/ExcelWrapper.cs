using System;
using System.Data;
using System.Data.OleDb;

namespace Parser.NET.Util.Wrapper
{
	/// <summary>
	/// Обертка для доступа к excel через ADO.NET
	/// </summary>
	public class ExcelWrapper
	{

		/// <summary>
		/// Подключаемся к .xls файлу.
		/// </summary>
		/// <param name="path">Путь откуда.</param>
		/// <param name="query">Например: SELECT * FROM [myRange1$A7:A122]</param>
		public void Load(string path, string query)
		{
			// Create connection string variable. Modify the "Data Source"
			// parameter as appropriate for your environment.
			String sConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" +
				"Data Source=" + path + ";" +
				"Extended Properties=Excel 8.0;";
			// Create connection object by using the preceding connection string.
			OleDbConnection objConn = new OleDbConnection(sConnectionString);
			// Open connection with the database.
			objConn.Open();
			// Create new OleDbCommand to return data from worksheet.
			OleDbCommand objCmdSelect = new OleDbCommand(query, objConn);
			// Create new OleDbDataAdapter that is used to build a DataSet
			// based on the preceding SQL SELECT statement.
			OleDbDataAdapter objAdapter = new OleDbDataAdapter();
			// Pass the Select command to the adapter.
			objAdapter.SelectCommand = objCmdSelect;
			// Create new DataSet to hold information from the worksheet.
			DataSet objDataset = new DataSet();
			// Fill the DataSet with the information from the worksheet.
			objAdapter.Fill(objDataset, "XLData");
			Table = objDataset.Tables[0];
			// Clean up objects.
			objConn.Close();
		}

		public string GetValue(int row, int col)
		{
			//if (Table.Rows.Count <= row) throw new NullReferenceException(String.Format("NULL row at {0},{1}", row, col));
			//if (Table.Rows[row].ItemArray.Length <= col) throw new NullReferenceException(String.Format("NULL col at {0},{1}", row, col));
			if (Table.Rows.Count <= row) return "NULL row";
			if (Table.Rows[row].ItemArray.Length <= col) return "NULL col";
			string result = Table.Rows[row].ItemArray[col].ToString();
			if (String.IsNullOrEmpty(result)) result = "";
			return result;
		}

		#region vars
		private DataTable table;
		public DataTable Table
		{
			get { return table; }
			set { table = value; }
		}
		#endregion

	}
}
