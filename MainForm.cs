﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Globalization;

namespace MapExtractor
{
    public partial class MainForm : Form
    {
        private Bitmap bmp;

        private List<Point> points;
        private Dictionary<int, Point> pointMap;
        private List<Segment> segments;
        private Dictionary<int, List<Segment>> pointSegments;
        private List<Vertex> vertices;
        private Dictionary<int, Vertex> vertexMap;
        private List<Path> paths;
        private Dictionary<int, List<Path>> vertexPaths;

        private void LoadImage(Image img)
        {
            bmp = new Bitmap(img);
            mapBox.SizeMode = PictureBoxSizeMode.AutoSize;
            mapBox.Image = bmp;
            mapBox.Cursor = Cursors.Hand;
        }

        public MainForm()
        {
            InitializeComponent();
            Image img = Image.FromFile("../../map.png");
            LoadImage(img);
        }

        private void buttonOriginal_Click(object sender, EventArgs e)
        {
            mapBox.Image = bmp;
        }

        private void buttonDrawSegments_Click(object sender, EventArgs e)
        {
            if (segments != null)
            {
                Bitmap copy = new Bitmap(mapBox.Image);
                Graphics g = Graphics.FromImage(copy);
                foreach (Segment s in segments)
                    g.DrawLine(Pens.Red, s.P1, s.P2);
                mapBox.Image = copy;
            }
        }

        private void buttonDrawVertices_Click(object sender, EventArgs e)
        {
            if (segments != null)
            {
                Bitmap copy = new Bitmap(mapBox.Image);
                Graphics g = Graphics.FromImage(copy);
                foreach (Vertex v in vertices)
                    g.FillRectangle(Brushes.Green, new Rectangle(v.Point.X - 2, v.Point.Y - 2, 4, 4));
                mapBox.Image = copy;
            }
        }

        private void buttonDrawPaths_Click(object sender, EventArgs e)
        {
            if (paths != null)
            {
                Bitmap copy = new Bitmap(mapBox.Image);
                Graphics g = Graphics.FromImage(copy);
                g.FillRectangle(Brushes.White, 0, 0, copy.Width, copy.Height); 
                Random random = new Random();
                foreach (Path p in paths)
                {
                    Color c = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
                    g.DrawLines(new Pen(c), p.Points.ToArray());
                }
                mapBox.Image = copy;
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "PNG Files|*.png";
            if (dialog.ShowDialog() == DialogResult.OK)
                mapBox.Image.Save(dialog.FileName);
        }

        private void Log(string msg)
        {
            listBoxLog.Items.Add(msg);
            listBoxLog.SelectedIndex = listBoxLog.Items.Count - 1;
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            Stopwatch stopWatch = new Stopwatch();

            stopWatch.Start();
            PointExtractor pointExt = new PointExtractor(bmp);
            points = pointExt.Points;
            pointMap = pointExt.PointMap;
            stopWatch.Stop();
            Log("Done extracting points in " + stopWatch.ElapsedMilliseconds + "ms");

            stopWatch.Restart();
            SegmentExtractor segExt = new SegmentExtractor(pointExt);
            segments = segExt.ExtractSegments();
            pointSegments = segExt.PointSegments;
            stopWatch.Stop();
            Log("Done extracting segments in " + stopWatch.ElapsedMilliseconds + "ms");

            stopWatch.Restart();
            VertexExtractor vertExt = new VertexExtractor(segExt);
            vertices = vertExt.Vertices;
            vertexMap = vertExt.VertexMap;
            stopWatch.Stop();
            Log("Done extracting vertices in " + stopWatch.ElapsedMilliseconds + "ms");

            
            stopWatch.Restart();
            PathExtractor pathExt = new PathExtractor(vertExt);
            paths = pathExt.Paths;
            vertexPaths = pathExt.VertexPaths;
            stopWatch.Stop();
            Log("Done extracting paths in " + stopWatch.ElapsedMilliseconds + "ms");

            double ct = Double.Parse(textBoxCollapse.Text, CultureInfo.InvariantCulture);
            stopWatch.Restart();
            int collapsed = pathExt.CollapseVertices(ct);
            stopWatch.Stop();
            Log("Collapsed " + collapsed + " vertices in " + stopWatch.ElapsedMilliseconds + "ms");

            stopWatch.Restart();
            int loose = pathExt.RemoveLooseEnds();
            stopWatch.Stop();
            Log("Removed " + loose + " loose ends in " + stopWatch.ElapsedMilliseconds + "ms");
            
            stopWatch.Restart();
            int merged = pathExt.MergeConsecutivePaths();
            stopWatch.Stop();
            Log("Merged " + merged + " consecutive paths in " + stopWatch.ElapsedMilliseconds + "ms");

            double st = Double.Parse(textBoxSimplify.Text, CultureInfo.InvariantCulture);
            stopWatch.Restart();
            pathExt.SimplifyPaths(st);
            stopWatch.Stop();
            Log("Simplified paths in " + stopWatch.ElapsedMilliseconds + "ms");
            
            labelPoints.Text = points.Count.ToString();
            labelSegments.Text = segments.Count.ToString();
            labelVertices.Text = vertices.Count.ToString();
            labelPaths.Text = paths.Count.ToString();
        }

        private bool dragging = false;
        private Point mousePos = new Point();

        private void mapBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mousePos = e.Location;
                dragging = true;
            }
        }

        private void mapBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging &&
               (mapBox.Image.Width > panelScroll.ClientSize.Width ||
               mapBox.Image.Height > panelScroll.ClientSize.Height))
            {
                Point scrollPos = new Point(
                    -panelScroll.AutoScrollPosition.X + (mousePos.X - e.X),
                    -panelScroll.AutoScrollPosition.Y + (mousePos.Y - e.Y)
                );
                panelScroll.AutoScrollPosition = scrollPos;
            }
        }

        private void mapBox_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "PNG IMAGE|*.png";
            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                Image img = Image.FromFile(openDialog.FileName);
                LoadImage(img);
            }
        }
    }
}
