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
            this.panelScroll = new System.Windows.Forms.Panel();
            this.mapBox = new System.Windows.Forms.PictureBox();
            this.listBoxLog = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBoxSimplify = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxCollapse = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonLoad = new System.Windows.Forms.Button();
            this.buttonStart = new System.Windows.Forms.Button();
            this.groupBoxStats = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.labelPolygons = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelPoints = new System.Windows.Forms.Label();
            this.labelPaths = new System.Windows.Forms.Label();
            this.labelSegments = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.labelVertices = new System.Windows.Forms.Label();
            this.groupBoxDrawing = new System.Windows.Forms.GroupBox();
            this.checkBoxInteractive = new System.Windows.Forms.CheckBox();
            this.buttonDrawPolygons = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonOriginal = new System.Windows.Forms.Button();
            this.buttonDrawSegments = new System.Windows.Forms.Button();
            this.buttonDrawVertices = new System.Windows.Forms.Button();
            this.buttonDrawPaths = new System.Windows.Forms.Button();
            this.buttonExport = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.panelScroll.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mapBox)).BeginInit();
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
            this.splitContainer.Panel1.Controls.Add(this.panelScroll);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.listBoxLog);
            this.splitContainer.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer.Panel2.Controls.Add(this.groupBoxStats);
            this.splitContainer.Panel2.Controls.Add(this.groupBoxDrawing);
            this.splitContainer.Size = new System.Drawing.Size(1339, 746);
            this.splitContainer.SplitterDistance = 1125;
            this.splitContainer.SplitterWidth = 2;
            this.splitContainer.TabIndex = 0;
            // 
            // panelScroll
            // 
            this.panelScroll.AutoScroll = true;
            this.panelScroll.Controls.Add(this.mapBox);
            this.panelScroll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelScroll.Location = new System.Drawing.Point(0, 0);
            this.panelScroll.Name = "panelScroll";
            this.panelScroll.Size = new System.Drawing.Size(1125, 746);
            this.panelScroll.TabIndex = 1;
            // 
            // mapBox
            // 
            this.mapBox.Location = new System.Drawing.Point(0, 0);
            this.mapBox.Name = "mapBox";
            this.mapBox.Size = new System.Drawing.Size(100, 50);
            this.mapBox.TabIndex = 0;
            this.mapBox.TabStop = false;
            this.mapBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mapBox_MouseDown);
            this.mapBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.mapBox_MouseMove);
            this.mapBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mapBox_MouseUp);
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
            this.groupBox1.Controls.Add(this.textBoxSimplify);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.textBoxCollapse);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.buttonLoad);
            this.groupBox1.Controls.Add(this.buttonStart);
            this.groupBox1.Location = new System.Drawing.Point(7, 176);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 188);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Processing";
            // 
            // textBoxSimplify
            // 
            this.textBoxSimplify.Location = new System.Drawing.Point(124, 103);
            this.textBoxSimplify.Name = "textBoxSimplify";
            this.textBoxSimplify.Size = new System.Drawing.Size(70, 20);
            this.textBoxSimplify.TabIndex = 24;
            this.textBoxSimplify.Text = "2";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 106);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(95, 13);
            this.label6.TabIndex = 23;
            this.label6.Text = "Simplify Threshold:";
            // 
            // textBoxCollapse
            // 
            this.textBoxCollapse.Location = new System.Drawing.Point(124, 77);
            this.textBoxCollapse.Name = "textBoxCollapse";
            this.textBoxCollapse.Size = new System.Drawing.Size(70, 20);
            this.textBoxCollapse.TabIndex = 22;
            this.textBoxCollapse.Text = "10";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 80);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 13);
            this.label5.TabIndex = 21;
            this.label5.Text = "Collapse Threshold:";
            // 
            // buttonLoad
            // 
            this.buttonLoad.Location = new System.Drawing.Point(6, 19);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(188, 23);
            this.buttonLoad.TabIndex = 20;
            this.buttonLoad.Text = "Load Binary Image";
            this.buttonLoad.UseVisualStyleBackColor = true;
            this.buttonLoad.Click += new System.EventHandler(this.buttonLoad_Click);
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(6, 48);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(188, 23);
            this.buttonStart.TabIndex = 19;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // groupBoxStats
            // 
            this.groupBoxStats.Controls.Add(this.label7);
            this.groupBoxStats.Controls.Add(this.labelPolygons);
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
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 118);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "Polygons:";
            // 
            // labelPolygons
            // 
            this.labelPolygons.AutoSize = true;
            this.labelPolygons.Location = new System.Drawing.Point(76, 118);
            this.labelPolygons.Name = "labelPolygons";
            this.labelPolygons.Size = new System.Drawing.Size(13, 13);
            this.labelPolygons.TabIndex = 16;
            this.labelPolygons.Text = "0";
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
            this.groupBoxDrawing.Controls.Add(this.buttonExport);
            this.groupBoxDrawing.Controls.Add(this.checkBoxInteractive);
            this.groupBoxDrawing.Controls.Add(this.buttonDrawPolygons);
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
            // checkBoxInteractive
            // 
            this.checkBoxInteractive.AutoSize = true;
            this.checkBoxInteractive.Location = new System.Drawing.Point(6, 200);
            this.checkBoxInteractive.Name = "checkBoxInteractive";
            this.checkBoxInteractive.Size = new System.Drawing.Size(99, 17);
            this.checkBoxInteractive.TabIndex = 18;
            this.checkBoxInteractive.Text = "Interactive map";
            this.checkBoxInteractive.UseVisualStyleBackColor = true;
            // 
            // buttonDrawPolygons
            // 
            this.buttonDrawPolygons.Location = new System.Drawing.Point(6, 135);
            this.buttonDrawPolygons.Name = "buttonDrawPolygons";
            this.buttonDrawPolygons.Size = new System.Drawing.Size(188, 23);
            this.buttonDrawPolygons.TabIndex = 17;
            this.buttonDrawPolygons.Text = "Draw Polygons";
            this.buttonDrawPolygons.UseVisualStyleBackColor = true;
            this.buttonDrawPolygons.Click += new System.EventHandler(this.buttonDrawPolygons_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(6, 164);
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
            // buttonExport
            // 
            this.buttonExport.Location = new System.Drawing.Point(102, 196);
            this.buttonExport.Name = "buttonExport";
            this.buttonExport.Size = new System.Drawing.Size(92, 23);
            this.buttonExport.TabIndex = 19;
            this.buttonExport.Text = "Export JSON";
            this.buttonExport.UseVisualStyleBackColor = true;
            this.buttonExport.Click += new System.EventHandler(this.buttonExport_Click);
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
            this.panelScroll.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mapBox)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBoxStats.ResumeLayout(false);
            this.groupBoxStats.PerformLayout();
            this.groupBoxDrawing.ResumeLayout(false);
            this.groupBoxDrawing.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer;
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
        private System.Windows.Forms.Panel panelScroll;
        private System.Windows.Forms.PictureBox mapBox;
        private System.Windows.Forms.Button buttonLoad;
        private System.Windows.Forms.TextBox textBoxSimplify;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxCollapse;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label labelPolygons;
        private System.Windows.Forms.Button buttonDrawPolygons;
        private System.Windows.Forms.CheckBox checkBoxInteractive;
        private System.Windows.Forms.Button buttonExport;
    }
}

