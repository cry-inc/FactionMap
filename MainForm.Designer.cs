namespace MapExtractor
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
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.mapPanel = new System.Windows.Forms.Panel();
            this.listBoxLog = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonStart = new System.Windows.Forms.Button();
            this.groupBoxStats = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelPoints = new System.Windows.Forms.Label();
            this.labelPaths = new System.Windows.Forms.Label();
            this.labelSegments = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.labelVertices = new System.Windows.Forms.Label();
            this.groupBoxDrawing = new System.Windows.Forms.GroupBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonOriginal = new System.Windows.Forms.Button();
            this.buttonDrawSegments = new System.Windows.Forms.Button();
            this.buttonDrawVertices = new System.Windows.Forms.Button();
            this.buttonDrawPaths = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBoxStats.SuspendLayout();
            this.groupBoxDrawing.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer.IsSplitterFixed = true;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.mapPanel);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.listBoxLog);
            this.splitContainer.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer.Panel2.Controls.Add(this.groupBoxStats);
            this.splitContainer.Panel2.Controls.Add(this.groupBoxDrawing);
            this.splitContainer.Size = new System.Drawing.Size(1339, 746);
            this.splitContainer.SplitterDistance = 1132;
            this.splitContainer.SplitterWidth = 2;
            this.splitContainer.TabIndex = 0;
            // 
            // mapPanel
            // 
            this.mapPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapPanel.Location = new System.Drawing.Point(0, 0);
            this.mapPanel.Name = "mapPanel";
            this.mapPanel.Size = new System.Drawing.Size(1132, 746);
            this.mapPanel.TabIndex = 0;
            // 
            // listBoxLog
            // 
            this.listBoxLog.Enabled = false;
            this.listBoxLog.FormattingEnabled = true;
            this.listBoxLog.Location = new System.Drawing.Point(8, 607);
            this.listBoxLog.Name = "listBoxLog";
            this.listBoxLog.Size = new System.Drawing.Size(199, 134);
            this.listBoxLog.TabIndex = 21;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonStart);
            this.groupBox1.Location = new System.Drawing.Point(7, 176);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 188);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Processing";
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(6, 19);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(188, 23);
            this.buttonStart.TabIndex = 19;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // groupBoxStats
            // 
            this.groupBoxStats.Controls.Add(this.label4);
            this.groupBoxStats.Controls.Add(this.label1);
            this.groupBoxStats.Controls.Add(this.label2);
            this.groupBoxStats.Controls.Add(this.labelPoints);
            this.groupBoxStats.Controls.Add(this.labelPaths);
            this.groupBoxStats.Controls.Add(this.labelSegments);
            this.groupBoxStats.Controls.Add(this.label3);
            this.groupBoxStats.Controls.Add(this.labelVertices);
            this.groupBoxStats.Location = new System.Drawing.Point(7, 12);
            this.groupBoxStats.Name = "groupBoxStats";
            this.groupBoxStats.Size = new System.Drawing.Size(200, 158);
            this.groupBoxStats.TabIndex = 19;
            this.groupBoxStats.TabStop = false;
            this.groupBoxStats.Text = "Stats";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 95);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Paths:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Points:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Segments:";
            // 
            // labelPoints
            // 
            this.labelPoints.AutoSize = true;
            this.labelPoints.Location = new System.Drawing.Point(76, 23);
            this.labelPoints.Name = "labelPoints";
            this.labelPoints.Size = new System.Drawing.Size(13, 13);
            this.labelPoints.TabIndex = 3;
            this.labelPoints.Text = "0";
            // 
            // labelPaths
            // 
            this.labelPaths.AutoSize = true;
            this.labelPaths.Location = new System.Drawing.Point(76, 95);
            this.labelPaths.Name = "labelPaths";
            this.labelPaths.Size = new System.Drawing.Size(13, 13);
            this.labelPaths.TabIndex = 14;
            this.labelPaths.Text = "0";
            // 
            // labelSegments
            // 
            this.labelSegments.AutoSize = true;
            this.labelSegments.Location = new System.Drawing.Point(76, 46);
            this.labelSegments.Name = "labelSegments";
            this.labelSegments.Size = new System.Drawing.Size(13, 13);
            this.labelSegments.TabIndex = 4;
            this.labelSegments.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Vertices:";
            // 
            // labelVertices
            // 
            this.labelVertices.AutoSize = true;
            this.labelVertices.Location = new System.Drawing.Point(76, 71);
            this.labelVertices.Name = "labelVertices";
            this.labelVertices.Size = new System.Drawing.Size(13, 13);
            this.labelVertices.TabIndex = 9;
            this.labelVertices.Text = "0";
            // 
            // groupBoxDrawing
            // 
            this.groupBoxDrawing.Controls.Add(this.buttonSave);
            this.groupBoxDrawing.Controls.Add(this.buttonOriginal);
            this.groupBoxDrawing.Controls.Add(this.buttonDrawSegments);
            this.groupBoxDrawing.Controls.Add(this.buttonDrawVertices);
            this.groupBoxDrawing.Controls.Add(this.buttonDrawPaths);
            this.groupBoxDrawing.Location = new System.Drawing.Point(7, 370);
            this.groupBoxDrawing.Name = "groupBoxDrawing";
            this.groupBoxDrawing.Size = new System.Drawing.Size(200, 231);
            this.groupBoxDrawing.TabIndex = 17;
            this.groupBoxDrawing.TabStop = false;
            this.groupBoxDrawing.Text = "Drawing";
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(6, 135);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(188, 23);
            this.buttonSave.TabIndex = 16;
            this.buttonSave.Text = "Save Image";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonOriginal
            // 
            this.buttonOriginal.Location = new System.Drawing.Point(6, 19);
            this.buttonOriginal.Name = "buttonOriginal";
            this.buttonOriginal.Size = new System.Drawing.Size(188, 23);
            this.buttonOriginal.TabIndex = 6;
            this.buttonOriginal.Text = "Show Input Image";
            this.buttonOriginal.UseVisualStyleBackColor = true;
            this.buttonOriginal.Click += new System.EventHandler(this.buttonOriginal_Click);
            // 
            // buttonDrawSegments
            // 
            this.buttonDrawSegments.Location = new System.Drawing.Point(6, 48);
            this.buttonDrawSegments.Name = "buttonDrawSegments";
            this.buttonDrawSegments.Size = new System.Drawing.Size(188, 23);
            this.buttonDrawSegments.TabIndex = 5;
            this.buttonDrawSegments.Text = "Draw Segments";
            this.buttonDrawSegments.UseVisualStyleBackColor = true;
            this.buttonDrawSegments.Click += new System.EventHandler(this.buttonDrawSegments_Click);
            // 
            // buttonDrawVertices
            // 
            this.buttonDrawVertices.Location = new System.Drawing.Point(6, 77);
            this.buttonDrawVertices.Name = "buttonDrawVertices";
            this.buttonDrawVertices.Size = new System.Drawing.Size(188, 23);
            this.buttonDrawVertices.TabIndex = 10;
            this.buttonDrawVertices.Text = "Draw Vertices";
            this.buttonDrawVertices.UseVisualStyleBackColor = true;
            this.buttonDrawVertices.Click += new System.EventHandler(this.buttonDrawVertices_Click);
            // 
            // buttonDrawPaths
            // 
            this.buttonDrawPaths.Location = new System.Drawing.Point(6, 106);
            this.buttonDrawPaths.Name = "buttonDrawPaths";
            this.buttonDrawPaths.Size = new System.Drawing.Size(188, 23);
            this.buttonDrawPaths.TabIndex = 11;
            this.buttonDrawPaths.Text = "Draw Paths";
            this.buttonDrawPaths.UseVisualStyleBackColor = true;
            this.buttonDrawPaths.Click += new System.EventHandler(this.buttonDrawPaths_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1339, 746);
            this.Controls.Add(this.splitContainer);
            this.Name = "MainForm";
            this.Text = "MapExtractor";
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBoxStats.ResumeLayout(false);
            this.groupBoxStats.PerformLayout();
            this.groupBoxDrawing.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.Panel mapPanel;
        private System.Windows.Forms.Label labelSegments;
        private System.Windows.Forms.Label labelPoints;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonDrawSegments;
        private System.Windows.Forms.Button buttonOriginal;
        private System.Windows.Forms.Label labelVertices;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonDrawVertices;
        private System.Windows.Forms.Button buttonDrawPaths;
        private System.Windows.Forms.Label labelPaths;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBoxDrawing;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBoxStats;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.ListBox listBoxLog;
        private System.Windows.Forms.Button buttonStart;
    }
}

