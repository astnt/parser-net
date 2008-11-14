﻿using System;
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
	$table[^table::excel[SELECT * FROM [Лист1^$A5:A7];../../../ParserTest/resources/sample.xls ]]
	some text
	$table
";
		}

		private void buttonParse_Click(object sender, EventArgs e)
		{
			SourceBuilder builder = new SourceBuilder();
			builder.Parse(textBoxSource.Text);
			Dumper d = new Dumper();

			string dump = d.Dump((RootNode)builder.RootNode).ToString();

			Executor exec = new Executor();
			string actual;
			try
			{
				exec.Run((RootNode) builder.RootNode);
				actual = exec.Output.ToString();
			}
			catch(Exception ex)
			{
				actual = "<h1>" + ex.Message + "</h1>"
				+ ex.StackTrace;
			}
			// System.Windows.Forms.WebBrowser
			webBrowserOutPut.DocumentText =
				String.Format("{0}<hr/><pre>{1}</pre>", actual, dump);
			
		}
	}
}
