using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace MapExtractor
{
    public partial class MainForm : Form
    {
        private Bitmap img;

        private List<Point> points;
        private Dictionary<int, Point> pointMap;
        private List<Segment> segments;
        private Dictionary<int, List<Segment>> pointSegments;
        private List<Vertex> vertices;
        private Dictionary<int, Vertex> vertexMap;
        private List<Path> paths;
        private Dictionary<int, List<Path>> vertexPaths;

        public MainForm()
        {
            InitializeComponent();
            string wd = Environment.CurrentDirectory;
            var loadedImg = Image.FromFile("../../map.png");
            img = new Bitmap(loadedImg);
            mapPanel.BackgroundImage = img;
            mapPanel.BackgroundImageLayout = ImageLayout.None;
        }

        /*
        private void CleanPaths()
        {
            DateTime start = DateTime.Now;
            List<Path> shortPaths = ListShortPaths();
            foreach (Path p in shortPaths)
            {
                string key1 = CreatePointKey(p.V1.Point.X, p.V1.Point.Y);
                List<Path> v1Paths = vertexPathMap[key1];
                string key2 = CreatePointKey(p.V2.Point.X, p.V2.Point.Y);
                List<Path> v2Paths = vertexPathMap[key2];

                // TODO

                if (v1Paths.Count == 1 && v2Paths.Count == 3)
                {
                    // v1 is a dead end, remove path and merge paths at v2
                    //paths.Remove(p);
                    //v2Paths.Remove(p);
                    //MergePaths(v2Paths[0], v2Paths[1]);
                }
            }
            TimeSpan span = DateTime.Now - start;
            Log("Done cleaning paths in " + span.TotalMilliseconds + "ms!");
        }
        

        private List<Segment> ListNeighborSegments(Point p)
        {
            List<Segment> connected = new List<Segment>();
            foreach (Segment s in segments)
                if (s.P1 == p || s.P2 == p)
                    connected.Add(s);
            return connected;
        }

        private List<Path> ListNeighborPaths(Vertex v)
        {
            List<Path> neighbors = new List<Path>();
            foreach (Path p in paths)
                if (p.V1 == v || p.V2 == v)
                    neighbors.Add(p);
            return neighbors;
        }
        */

        private List<Path> ListShortPaths()
        {
            List<Path> shorts = new List<Path>();
            double min = Double.Parse(textBoxMinLength.Text);
            List<Path> shortPaths = new List<Path>();
            foreach (Path p in paths)
                if (p.Length() < min)
                    shorts.Add(p);
            return shorts;
        }

        private void buttonOriginal_Click(object sender, EventArgs e)
        {
            mapPanel.BackgroundImage = img;
        }

        private void buttonDrawSegments_Click(object sender, EventArgs e)
        {
            if (segments != null)
            {
                Bitmap copy = new Bitmap(mapPanel.BackgroundImage);
                Graphics g = Graphics.FromImage(copy);
                foreach (Segment s in segments)
                    g.DrawLine(Pens.Red, s.P1, s.P2);
                mapPanel.BackgroundImage = copy;
            }
        }

        private void buttonDrawVertices_Click(object sender, EventArgs e)
        {
            if (segments != null)
            {
                Bitmap copy = new Bitmap(mapPanel.BackgroundImage);
                Graphics g = Graphics.FromImage(copy);
                foreach (Vertex v in vertices)
                    g.FillRectangle(Brushes.Green, new Rectangle(v.Point.X - 2, v.Point.Y - 2, 4, 4));
                mapPanel.BackgroundImage = copy;
            }
        }

        private void buttonDrawPaths_Click(object sender, EventArgs e)
        {
            if (paths != null)
            {
                Bitmap copy = new Bitmap(mapPanel.BackgroundImage);
                Graphics g = Graphics.FromImage(copy);
                //g.FillRectangle(Brushes.White, 0, 0, copy.Width, copy.Height); 
                Random random = new Random();
                foreach (Path p in paths)
                {
                    Color c = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
                    g.DrawLines(new Pen(c), p.Points.ToArray());
                }
                mapPanel.BackgroundImage = copy;
            }
        }

        private void buttonDrawShortPaths_Click(object sender, EventArgs e)
        {
            if (paths != null)
            {
                Bitmap copy = new Bitmap(mapPanel.BackgroundImage);
                Graphics g = Graphics.FromImage(copy);
                Random random = new Random();
                List<Path> shorts = ListShortPaths();
                foreach (Path p in shorts)
                    g.DrawLines(new Pen(Color.Red, 3), p.Points.ToArray());
                mapPanel.BackgroundImage = copy;
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "PNG Files|*.png";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                mapPanel.BackgroundImage.Save(dialog.FileName);
            }
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
            PointExtractor pointExt = new PointExtractor(img);
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

            labelPoints.Text = points.Count.ToString();
            labelSegments.Text = segments.Count.ToString();
            labelVertices.Text = vertices.Count.ToString();
            labelPaths.Text = paths.Count.ToString();
        }
    }
}
