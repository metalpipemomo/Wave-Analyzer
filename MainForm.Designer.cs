namespace WaveAnalyzer
{
    partial class MainForm
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
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
            this.compressionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compressFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openCompressedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sampleRateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.hzToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quantizationLevelsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bitsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bitsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.bitsToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.somethingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.windowsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.plainDFTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hannWindowToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.triangularWindowToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.allToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.StartPlay = new System.Windows.Forms.Button();
            this.Record = new System.Windows.Forms.Button();
            this.TimeMS = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // chart1
            // 
            this.chart1.BackColor = System.Drawing.Color.Transparent;
            this.chart1.BorderlineColor = System.Drawing.Color.Black;
            this.chart1.BorderlineWidth = 2;
            chartArea2.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chart1.Legends.Add(legend2);
            this.chart1.Location = new System.Drawing.Point(12, 87);
            this.chart1.Name = "chart1";
            this.chart1.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.None;
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.IsVisibleInLegend = false;
            series2.Legend = "Legend1";
            series2.Name = "Original";
            series2.YValuesPerPoint = 10;
            this.chart1.Series.Add(series2);
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
            this.editToolStripMenuItem,
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
            this.fileDetailsToolStripMenuItem,
            this.compressionToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.importToolStripMenuItem.Text = "Import";
            this.importToolStripMenuItem.Click += new System.EventHandler(this.importToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            this.clearToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.clearToolStripMenuItem.Text = "Clear";
            this.clearToolStripMenuItem.Click += new System.EventHandler(this.clearToolStripMenuItem_Click);
            // 
            // fileDetailsToolStripMenuItem
            // 
            this.fileDetailsToolStripMenuItem.Name = "fileDetailsToolStripMenuItem";
            this.fileDetailsToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.fileDetailsToolStripMenuItem.Text = "File Details";
            this.fileDetailsToolStripMenuItem.Click += new System.EventHandler(this.fileDetailsToolStripMenuItem_Click);
            // 
            // compressionToolStripMenuItem
            // 
            this.compressionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.compressFileToolStripMenuItem,
            this.openCompressedToolStripMenuItem});
            this.compressionToolStripMenuItem.Name = "compressionToolStripMenuItem";
            this.compressionToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.compressionToolStripMenuItem.Text = "Compression";
            // 
            // compressFileToolStripMenuItem
            // 
            this.compressFileToolStripMenuItem.Name = "compressFileToolStripMenuItem";
            this.compressFileToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.compressFileToolStripMenuItem.Text = "Compress File";
            this.compressFileToolStripMenuItem.Click += new System.EventHandler(this.compressFileToolStripMenuItem_Click);
            // 
            // openCompressedToolStripMenuItem
            // 
            this.openCompressedToolStripMenuItem.Name = "openCompressedToolStripMenuItem";
            this.openCompressedToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.openCompressedToolStripMenuItem.Text = "Open Compressed";
            this.openCompressedToolStripMenuItem.Click += new System.EventHandler(this.openCompressedToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sampleRateToolStripMenuItem,
            this.quantizationLevelsToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // sampleRateToolStripMenuItem
            // 
            this.sampleRateToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.toolStripMenuItem3,
            this.toolStripMenuItem4,
            this.hzToolStripMenuItem});
            this.sampleRateToolStripMenuItem.Name = "sampleRateToolStripMenuItem";
            this.sampleRateToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.sampleRateToolStripMenuItem.Text = "Sample Rate";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(180, 22);
            this.toolStripMenuItem2.Text = "11025 Hz";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(180, 22);
            this.toolStripMenuItem3.Text = "22050 Hz";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(180, 22);
            this.toolStripMenuItem4.Text = "44100 Hz";
            this.toolStripMenuItem4.Click += new System.EventHandler(this.toolStripMenuItem4_Click);
            // 
            // hzToolStripMenuItem
            // 
            this.hzToolStripMenuItem.Name = "hzToolStripMenuItem";
            this.hzToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.hzToolStripMenuItem.Text = "88200 Hz";
            this.hzToolStripMenuItem.Click += new System.EventHandler(this.hzToolStripMenuItem_Click);
            // 
            // quantizationLevelsToolStripMenuItem
            // 
            this.quantizationLevelsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bitsToolStripMenuItem,
            this.bitsToolStripMenuItem1,
            this.bitsToolStripMenuItem2});
            this.quantizationLevelsToolStripMenuItem.Name = "quantizationLevelsToolStripMenuItem";
            this.quantizationLevelsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.quantizationLevelsToolStripMenuItem.Text = "Quantization Levels";
            // 
            // bitsToolStripMenuItem
            // 
            this.bitsToolStripMenuItem.Name = "bitsToolStripMenuItem";
            this.bitsToolStripMenuItem.Size = new System.Drawing.Size(108, 22);
            this.bitsToolStripMenuItem.Text = "8 bits";
            this.bitsToolStripMenuItem.Click += new System.EventHandler(this.bitsToolStripMenuItem_Click);
            // 
            // bitsToolStripMenuItem1
            // 
            this.bitsToolStripMenuItem1.Name = "bitsToolStripMenuItem1";
            this.bitsToolStripMenuItem1.Size = new System.Drawing.Size(108, 22);
            this.bitsToolStripMenuItem1.Text = "16 bits";
            this.bitsToolStripMenuItem1.Click += new System.EventHandler(this.bitsToolStripMenuItem1_Click);
            // 
            // bitsToolStripMenuItem2
            // 
            this.bitsToolStripMenuItem2.Name = "bitsToolStripMenuItem2";
            this.bitsToolStripMenuItem2.Size = new System.Drawing.Size(108, 22);
            this.bitsToolStripMenuItem2.Text = "24 bits";
            this.bitsToolStripMenuItem2.Click += new System.EventHandler(this.bitsToolStripMenuItem2_Click);
            // 
            // somethingToolStripMenuItem
            // 
            this.somethingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.windowsToolStripMenuItem});
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
            this.allToolStripMenuItem});
            this.windowsToolStripMenuItem.Name = "windowsToolStripMenuItem";
            this.windowsToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.windowsToolStripMenuItem.Text = "Windows";
            // 
            // plainDFTToolStripMenuItem
            // 
            this.plainDFTToolStripMenuItem.Name = "plainDFTToolStripMenuItem";
            this.plainDFTToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.plainDFTToolStripMenuItem.Text = "Plain DFT";
            this.plainDFTToolStripMenuItem.Click += new System.EventHandler(this.plainDFTToolStripMenuItem_Click);
            // 
            // hannWindowToolStripMenuItem1
            // 
            this.hannWindowToolStripMenuItem1.Name = "hannWindowToolStripMenuItem1";
            this.hannWindowToolStripMenuItem1.Size = new System.Drawing.Size(173, 22);
            this.hannWindowToolStripMenuItem1.Text = "Hann Window";
            this.hannWindowToolStripMenuItem1.Click += new System.EventHandler(this.hannWindowToolStripMenuItem1_Click);
            // 
            // triangularWindowToolStripMenuItem1
            // 
            this.triangularWindowToolStripMenuItem1.Name = "triangularWindowToolStripMenuItem1";
            this.triangularWindowToolStripMenuItem1.Size = new System.Drawing.Size(173, 22);
            this.triangularWindowToolStripMenuItem1.Text = "Triangular Window";
            this.triangularWindowToolStripMenuItem1.Click += new System.EventHandler(this.triangularWindowToolStripMenuItem1_Click);
            // 
            // allToolStripMenuItem
            // 
            this.allToolStripMenuItem.Name = "allToolStripMenuItem";
            this.allToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.allToolStripMenuItem.Text = "All";
            this.allToolStripMenuItem.Click += new System.EventHandler(this.allToolStripMenuItem_Click);
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
            // TimeMS
            // 
            this.TimeMS.AutoSize = true;
            this.TimeMS.BackColor = System.Drawing.Color.Transparent;
            this.TimeMS.ForeColor = System.Drawing.Color.Black;
            this.TimeMS.Location = new System.Drawing.Point(459, 505);
            this.TimeMS.Name = "TimeMS";
            this.TimeMS.Size = new System.Drawing.Size(52, 13);
            this.TimeMS.TabIndex = 15;
            this.TimeMS.Text = "Time (ms)";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SlateGray;
            this.ClientSize = new System.Drawing.Size(934, 561);
            this.Controls.Add(this.TimeMS);
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
        private System.Windows.Forms.ToolStripMenuItem compressionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem compressFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openCompressedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sampleRateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem quantizationLevelsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bitsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bitsToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem bitsToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem hzToolStripMenuItem;
        private System.Windows.Forms.Label TimeMS;
    }
}

