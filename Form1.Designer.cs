
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
            this.File = new System.Windows.Forms.Button();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.Clear = new System.Windows.Forms.Button();
            this.Save = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.Copy = new System.Windows.Forms.Button();
            this.Cut = new System.Windows.Forms.Button();
            this.Paste = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.somethingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hannWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.triangularWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generateFilterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // File
            // 
            this.File.Location = new System.Drawing.Point(12, 39);
            this.File.Name = "File";
            this.File.Size = new System.Drawing.Size(75, 23);
            this.File.TabIndex = 0;
            this.File.Text = "File";
            this.File.UseVisualStyleBackColor = true;
            this.File.Click += new System.EventHandler(this.File_Click);
            // 
            // chart1
            // 
            this.chart1.BackColor = System.Drawing.Color.SlateGray;
            this.chart1.BorderlineColor = System.Drawing.Color.SlateGray;
            this.chart1.BorderlineWidth = 2;
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(12, 68);
            this.chart1.Name = "chart1";
            this.chart1.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.None;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.IsVisibleInLegend = false;
            series1.Legend = "Legend1";
            series1.Name = "Original";
            series1.YValuesPerPoint = 10;
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(894, 179);
            this.chart1.TabIndex = 1;
            this.chart1.Click += new System.EventHandler(this.chart1_Click);
            // 
            // Clear
            // 
            this.Clear.Location = new System.Drawing.Point(175, 39);
            this.Clear.Name = "Clear";
            this.Clear.Size = new System.Drawing.Size(75, 23);
            this.Clear.TabIndex = 2;
            this.Clear.Text = "Clear";
            this.Clear.UseVisualStyleBackColor = true;
            this.Clear.Click += new System.EventHandler(this.Clear_Click);
            // 
            // Save
            // 
            this.Save.Location = new System.Drawing.Point(93, 39);
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(75, 23);
            this.Save.TabIndex = 3;
            this.Save.Text = "Save";
            this.Save.UseVisualStyleBackColor = true;
            this.Save.Click += new System.EventHandler(this.Save_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(835, 255);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(71, 21);
            this.button1.TabIndex = 5;
            this.button1.Text = "DIE";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Copy
            // 
            this.Copy.Location = new System.Drawing.Point(93, 253);
            this.Copy.Name = "Copy";
            this.Copy.Size = new System.Drawing.Size(75, 23);
            this.Copy.TabIndex = 7;
            this.Copy.Text = "Copy";
            this.Copy.UseVisualStyleBackColor = true;
            this.Copy.Click += new System.EventHandler(this.Copy_Click);
            // 
            // Cut
            // 
            this.Cut.Location = new System.Drawing.Point(12, 253);
            this.Cut.Name = "Cut";
            this.Cut.Size = new System.Drawing.Size(75, 23);
            this.Cut.TabIndex = 8;
            this.Cut.Text = "Cut";
            this.Cut.UseVisualStyleBackColor = true;
            this.Cut.Click += new System.EventHandler(this.Cut_Click);
            // 
            // Paste
            // 
            this.Paste.Location = new System.Drawing.Point(175, 253);
            this.Paste.Name = "Paste";
            this.Paste.Size = new System.Drawing.Size(75, 23);
            this.Paste.TabIndex = 9;
            this.Paste.Text = "Paste";
            this.Paste.UseVisualStyleBackColor = true;
            this.Paste.Click += new System.EventHandler(this.Paste_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.somethingToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(922, 24);
            this.menuStrip1.TabIndex = 10;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // somethingToolStripMenuItem
            // 
            this.somethingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hannWindowToolStripMenuItem,
            this.triangularWindowToolStripMenuItem,
            this.generateFilterToolStripMenuItem});
            this.somethingToolStripMenuItem.Name = "somethingToolStripMenuItem";
            this.somethingToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.somethingToolStripMenuItem.Text = "DFT";
            // 
            // hannWindowToolStripMenuItem
            // 
            this.hannWindowToolStripMenuItem.Name = "hannWindowToolStripMenuItem";
            this.hannWindowToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.hannWindowToolStripMenuItem.Text = "Hann Window";
            this.hannWindowToolStripMenuItem.Click += new System.EventHandler(this.hannWindowToolStripMenuItem_Click);
            // 
            // triangularWindowToolStripMenuItem
            // 
            this.triangularWindowToolStripMenuItem.Name = "triangularWindowToolStripMenuItem";
            this.triangularWindowToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.triangularWindowToolStripMenuItem.Text = "Triangular Window";
            this.triangularWindowToolStripMenuItem.Click += new System.EventHandler(this.triangularWindowToolStripMenuItem_Click);
            // 
            // generateFilterToolStripMenuItem
            // 
            this.generateFilterToolStripMenuItem.Name = "generateFilterToolStripMenuItem";
            this.generateFilterToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.generateFilterToolStripMenuItem.Text = "Generate Filter";
            this.generateFilterToolStripMenuItem.Click += new System.EventHandler(this.generateFilterToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SlateGray;
            this.ClientSize = new System.Drawing.Size(922, 539);
            this.Controls.Add(this.Paste);
            this.Controls.Add(this.Cut);
            this.Controls.Add(this.Copy);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.Save);
            this.Controls.Add(this.Clear);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.File);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button File;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Button Clear;
        private System.Windows.Forms.Button Save;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button Copy;
        private System.Windows.Forms.Button Cut;
        private System.Windows.Forms.Button Paste;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem somethingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hannWindowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem triangularWindowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generateFilterToolStripMenuItem;
    }
}

