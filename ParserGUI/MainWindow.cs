using System;
using System.Windows.Forms;
using Parser;
using Parser.Model;
using Parser.Util;

namespace ParserGUI
{
	public partial class MainWindow : Form
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void MainWindow_Load(object sender, EventArgs e)
		{
//			textBoxSource.Text = @"
//@main[]
//	$myvar[xxx]
//	test is ^test[what;777]
//
//@test[word;number]
//	tested $word $number $myvar
//";
			textBoxSource.Text = @"

@main[]
	$table[^table::excel[SELECT * FROM [Лист1^$A4:B7];../../../ParserTest/resources/sample.xls ]]
	some text
	$table
<table border=""1"">
^table.menu{
<tr>
<td>^table.column[0]</td><td>^table.column[1]</td>
</tr>
}
</table>


	$table2[^table::excel[SELECT * FROM [Лист1^$C4:C7];../../../ParserTest/resources/sample.xls ]]
	another text
 $table2

	$table2
<table border=""1"">
^table2.menu{
<tr>
<td>^table2.column[0]</td>
</tr>
}
</table>



";
		}

		private void buttonParse_Click(object sender, EventArgs e)
		{
			DateTime sourceBuildStart = DateTime.Now;
			SourceBuilder builder = new SourceBuilder();
			builder.Parse(textBoxSource.Text);
			double sourceBuildEnd = DateTime.Now.Subtract(sourceBuildStart).TotalMilliseconds;

			Dumper d = new Dumper();
			string dump = d.Dump((RootNode)builder.RootNode).ToString()
				.Replace("<", @"&lt;")
				.Replace(">", @"&gt;")
				;

			double execEnd = 0;
			string actual;
			try
			{
				DateTime execStart = DateTime.Now;
				Executor exec = new Executor();
				exec.Run((RootNode) builder.RootNode);
				actual = exec.TextOutput.ToString();
				execEnd = DateTime.Now.Subtract(execStart).TotalMilliseconds;
			}
			catch(Exception ex)
			{
				actual = "<h1>" + ex.Message + "</h1>"
				+ ex.StackTrace;
			}
			// System.Windows.Forms.WebBrowser
			webBrowserOutPut.DocumentText =
				String.Format("{0}<hr/>parse time {2} ms, exec time {3}<hr/><pre>{1}</pre>", actual, dump
					, sourceBuildEnd, execEnd);
			
		}
	}
}
