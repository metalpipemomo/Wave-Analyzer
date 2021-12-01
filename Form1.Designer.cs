namespace WaveAnalyzer
{
    partial class Form1
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.Copy = new System.Windows.Forms.Button();
            this.Cut = new System.Windows.Forms.Button();
            this.Paste = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileDetailsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.somethingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.windowsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.plainDFTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hannWindowToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.triangularWindowToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.allToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generateFilterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lowPassToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.highPassToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.StartPlay = new System.Windows.Forms.Button();
            this.Record = new System.Windows.Forms.Button();
            this.nonThreadedAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // chart1
            // 
            this.chart1.BackColor = System.Drawing.Color.Transparent;
            this.chart1.BorderlineColor = System.Drawing.Color.Black;
            this.chart1.BorderlineWidth = 2;
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(12, 87);
            this.chart1.Name = "chart1";
            this.chart1.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.None;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.IsVisibleInLegend = false;
            series1.Legend = "Legend1";
            series1.Name = "Original";
            series1.YValuesPerPoint = 10;
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(910, 422);
            this.chart1.TabIndex = 1;
            this.chart1.Click += new System.EventHandler(this.chart1_Click);
            // 
            // Copy
            // 
            this.Copy.BackColor = System.Drawing.Color.Transparent;
            this.Copy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Copy.Image = global::WaveAnalyzer.Properties.Resources.copy;
            this.Copy.Location = new System.Drawing.Point(59, 39);
            this.Copy.Name = "Copy";
            this.Copy.Size = new System.Drawing.Size(41, 42);
            this.Copy.TabIndex = 7;
            this.Copy.UseVisualStyleBackColor = false;
            this.Copy.Click += new System.EventHandler(this.Copy_Click);
            // 
            // Cut
            // 
            this.Cut.BackColor = System.Drawing.Color.Transparent;
            this.Cut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Cut.Image = ((System.Drawing.Image)(resources.GetObject("Cut.Image")));
            this.Cut.Location = new System.Drawing.Point(12, 39);
            this.Cut.Name = "Cut";
            this.Cut.Size = new System.Drawing.Size(41, 42);
            this.Cut.TabIndex = 8;
            this.Cut.UseVisualStyleBackColor = false;
            this.Cut.Click += new System.EventHandler(this.Cut_Click);
            // 
            // Paste
            // 
            this.Paste.BackColor = System.Drawing.Color.Transparent;
            this.Paste.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Paste.Image = global::WaveAnalyzer.Properties.Resources.paste;
            this.Paste.Location = new System.Drawing.Point(106, 39);
            this.Paste.Name = "Paste";
            this.Paste.Size = new System.Drawing.Size(41, 42);
            this.Paste.TabIndex = 9;
            this.Paste.UseVisualStyleBackColor = false;
            this.Paste.Click += new System.EventHandler(this.Paste_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.somethingToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(934, 24);
            this.menuStrip1.TabIndex = 10;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.clearToolStripMenuItem,
            this.fileDetailsToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.importToolStripMenuItem.Text = "Import";
            this.importToolStripMenuItem.Click += new System.EventHandler(this.importToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            this.clearToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.clearToolStripMenuItem.Text = "Clear";
            this.clearToolStripMenuItem.Click += new System.EventHandler(this.clearToolStripMenuItem_Click);
            // 
            // fileDetailsToolStripMenuItem
            // 
            this.fileDetailsToolStripMenuItem.Name = "fileDetailsToolStripMenuItem";
            this.fileDetailsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.fileDetailsToolStripMenuItem.Text = "File Details";
            this.fileDetailsToolStripMenuItem.Click += new System.EventHandler(this.fileDetailsToolStripMenuItem_Click);
            // 
            // somethingToolStripMenuItem
            // 
            this.somethingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.windowsToolStripMenuItem,
            this.generateFilterToolStripMenuItem});
            this.somethingToolStripMenuItem.Name = "somethingToolStripMenuItem";
            this.somethingToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.somethingToolStripMenuItem.Text = "DFT";
            // 
            // windowsToolStripMenuItem
            // 
            this.windowsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.plainDFTToolStripMenuItem,
            this.hannWindowToolStripMenuItem1,
            this.triangularWindowToolStripMenuItem1,
            this.allToolStripMenuItem,
            this.nonThreadedAllToolStripMenuItem});
            this.windowsToolStripMenuItem.Name = "windowsToolStripMenuItem";
            this.windowsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.windowsToolStripMenuItem.Text = "Windows";
            // 
            // plainDFTToolStripMenuItem
            // 
            this.plainDFTToolStripMenuItem.Name = "plainDFTToolStripMenuItem";
            this.plainDFTToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.plainDFTToolStripMenuItem.Text = "Plain DFT";
            this.plainDFTToolStripMenuItem.Click += new System.EventHandler(this.plainDFTToolStripMenuItem_Click);
            // 
            // hannWindowToolStripMenuItem1
            // 
            this.hannWindowToolStripMenuItem1.Name = "hannWindowToolStripMenuItem1";
            this.hannWindowToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.hannWindowToolStripMenuItem1.Text = "Hann Window";
            this.hannWindowToolStripMenuItem1.Click += new System.EventHandler(this.hannWindowToolStripMenuItem1_Click);
            // 
            // triangularWindowToolStripMenuItem1
            // 
            this.triangularWindowToolStripMenuItem1.Name = "triangularWindowToolStripMenuItem1";
            this.triangularWindowToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.triangularWindowToolStripMenuItem1.Text = "Triangular Window";
            this.triangularWindowToolStripMenuItem1.Click += new System.EventHandler(this.triangularWindowToolStripMenuItem1_Click);
            // 
            // allToolStripMenuItem
            // 
            this.allToolStripMenuItem.Name = "allToolStripMenuItem";
            this.allToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.allToolStripMenuItem.Text = "All";
            this.allToolStripMenuItem.Click += new System.EventHandler(this.allToolStripMenuItem_Click);
            // 
            // generateFilterToolStripMenuItem
            // 
            this.generateFilterToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lowPassToolStripMenuItem,
            this.highPassToolStripMenuItem});
            this.generateFilterToolStripMenuItem.Name = "generateFilterToolStripMenuItem";
            this.generateFilterToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.generateFilterToolStripMenuItem.Text = "Generate Filter";
            // 
            // lowPassToolStripMenuItem
            // 
            this.lowPassToolStripMenuItem.Name = "lowPassToolStripMenuItem";
            this.lowPassToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.lowPassToolStripMenuItem.Text = "Low-Pass";
            this.lowPassToolStripMenuItem.Click += new System.EventHandler(this.lowPassToolStripMenuItem_Click);
            // 
            // highPassToolStripMenuItem
            // 
            this.highPassToolStripMenuItem.Name = "highPassToolStripMenuItem";
            this.highPassToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.highPassToolStripMenuItem.Text = "High-Pass";
            this.highPassToolStripMenuItem.Click += new System.EventHandler(this.highPassToolStripMenuItem_Click);
            // 
            // hScrollBar1
            // 
            this.hScrollBar1.LargeChange = 2;
            this.hScrollBar1.Location = new System.Drawing.Point(0, 543);
            this.hScrollBar1.Maximum = 1;
            this.hScrollBar1.Name = "hScrollBar1";
            this.hScrollBar1.Size = new System.Drawing.Size(935, 20);
            this.hScrollBar1.TabIndex = 14;
            this.hScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar1_Scroll);
            // 
            // StartPlay
            // 
            this.StartPlay.BackColor = System.Drawing.Color.Transparent;
            this.StartPlay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.StartPlay.Image = ((System.Drawing.Image)(resources.GetObject("StartPlay.Image")));
            this.StartPlay.Location = new System.Drawing.Point(881, 39);
            this.StartPlay.Name = "StartPlay";
            this.StartPlay.Size = new System.Drawing.Size(41, 42);
            this.StartPlay.TabIndex = 13;
            this.StartPlay.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.StartPlay.UseVisualStyleBackColor = false;
            this.StartPlay.Click += new System.EventHandler(this.StartPlay_Click);
            // 
            // Record
            // 
            this.Record.BackColor = System.Drawing.Color.Transparent;
            this.Record.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Record.Image = ((System.Drawing.Image)(resources.GetObject("Record.Image")));
            this.Record.Location = new System.Drawing.Point(834, 39);
            this.Record.Name = "Record";
            this.Record.Size = new System.Drawing.Size(41, 42);
            this.Record.TabIndex = 11;
            this.Record.UseVisualStyleBackColor = false;
            this.Record.Click += new System.EventHandler(this.Record_Click);
            // 
            // nonThreadedAllToolStripMenuItem
            // 
            this.nonThreadedAllToolStripMenuItem.Name = "nonThreadedAllToolStripMenuItem";
            this.nonThreadedAllToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.nonThreadedAllToolStripMenuItem.Text = "Non-Threaded All";
            this.nonThreadedAllToolStripMenuItem.Click += new System.EventHandler(this.nonThreadedAllToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SlateGray;
            this.ClientSize = new System.Drawing.Size(934, 561);
            this.Controls.Add(this.hScrollBar1);
            this.Controls.Add(this.StartPlay);
            this.Controls.Add(this.Record);
            this.Controls.Add(this.Paste);
            this.Controls.Add(this.Cut);
            this.Controls.Add(this.Copy);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Dave Analyzer";
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Button Copy;
        private System.Windows.Forms.Button Cut;
        private System.Windows.Forms.Button Paste;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem somethingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generateFilterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lowPassToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem highPassToolStripMenuItem;
        private System.Windows.Forms.Button Record;
        private System.Windows.Forms.Button StartPlay;
        private System.Windows.Forms.HScrollBar hScrollBar1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem windowsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hannWindowToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem triangularWindowToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem allToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem plainDFTToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fileDetailsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nonThreadedAllToolStripMenuItem;
    }
}

