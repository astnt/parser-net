namespace ParserGUI
{
	partial class MainWindow
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.panelControls = new System.Windows.Forms.Panel();
			this.buttonParse = new System.Windows.Forms.Button();
			this.panelLayuot = new System.Windows.Forms.Panel();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.textBoxSource = new System.Windows.Forms.TextBox();
			this.webBrowserOutPut = new System.Windows.Forms.WebBrowser();
			this.panelControls.SuspendLayout();
			this.panelLayuot.SuspendLayout();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// panelControls
			// 
			this.panelControls.Controls.Add(this.buttonParse);
			this.panelControls.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panelControls.Location = new System.Drawing.Point(0, 392);
			this.panelControls.Name = "panelControls";
			this.panelControls.Size = new System.Drawing.Size(526, 46);
			this.panelControls.TabIndex = 0;
			// 
			// buttonParse
			// 
			this.buttonParse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonParse.Location = new System.Drawing.Point(439, 11);
			this.buttonParse.Name = "buttonParse";
			this.buttonParse.Size = new System.Drawing.Size(75, 23);
			this.buttonParse.TabIndex = 0;
			this.buttonParse.Text = "Parse";
			this.buttonParse.UseVisualStyleBackColor = true;
			this.buttonParse.Click += new System.EventHandler(this.buttonParse_Click);
			// 
			// panelLayuot
			// 
			this.panelLayuot.Controls.Add(this.splitContainer1);
			this.panelLayuot.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelLayuot.Location = new System.Drawing.Point(0, 0);
			this.panelLayuot.Name = "panelLayuot";
			this.panelLayuot.Size = new System.Drawing.Size(526, 392);
			this.panelLayuot.TabIndex = 1;
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.textBoxSource);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.webBrowserOutPut);
			this.splitContainer1.Size = new System.Drawing.Size(526, 392);
			this.splitContainer1.SplitterDistance = 259;
			this.splitContainer1.TabIndex = 0;
			// 
			// textBoxSource
			// 
			this.textBoxSource.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBoxSource.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.textBoxSource.Location = new System.Drawing.Point(0, 0);
			this.textBoxSource.Multiline = true;
			this.textBoxSource.Name = "textBoxSource";
			this.textBoxSource.Size = new System.Drawing.Size(259, 392);
			this.textBoxSource.TabIndex = 0;
			// 
			// webBrowserOutPut
			// 
			this.webBrowserOutPut.Dock = System.Windows.Forms.DockStyle.Fill;
			this.webBrowserOutPut.Location = new System.Drawing.Point(0, 0);
			this.webBrowserOutPut.MinimumSize = new System.Drawing.Size(20, 20);
			this.webBrowserOutPut.Name = "webBrowserOutPut";
			this.webBrowserOutPut.Size = new System.Drawing.Size(263, 392);
			this.webBrowserOutPut.TabIndex = 0;
			// 
			// MainWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(526, 438);
			this.Controls.Add(this.panelLayuot);
			this.Controls.Add(this.panelControls);
			this.Name = "MainWindow";
			this.Text = "GUI for Parser.NET test";
			this.Load += new System.EventHandler(this.MainWindow_Load);
			this.panelControls.ResumeLayout(false);
			this.panelLayuot.ResumeLayout(false);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.PerformLayout();
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panelControls;
		private System.Windows.Forms.Button buttonParse;
		private System.Windows.Forms.Panel panelLayuot;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.TextBox textBoxSource;
		private System.Windows.Forms.WebBrowser webBrowserOutPut;
	}
}

