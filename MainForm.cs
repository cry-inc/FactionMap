using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MapExtractor
{
    public partial class MainForm : Form
    {
        private Point[] neighbors = {
            new Point( 1,  0),
            new Point(-1,  0),
            new Point( 0,  1),
            new Point( 0, -1),
            new Point( 1,  1),
            new Point(-1, -1),
            new Point(-1,  1),
            new Point( 1, -1),                             
        };

        private Bitmap img;

        private List<Point> points = new List<Point>();
        private Dictionary<string, Point> pointMap = new Dictionary<string, Point>();

        private List<Segment> segments = new List<Segment>();
        private Dictionary<string, Segment> segmentMap = new Dictionary<string, Segment>();

        private List<Vertex> vertices = new List<Vertex>();
        private Dictionary<string, Vertex> vertexMap = new Dictionary<string, Vertex>();
        private Dictionary<string, List<Path>> vertexPathMap = new Dictionary<string, List<Path>>();

        private List<Path> paths = new List<Path>();
        private Dictionary<string, Path> pathMap = new Dictionary<string, Path>();

        public MainForm()
        {
            InitializeComponent();
            string wd = Environment.CurrentDirectory;
            var loadedImg = Image.FromFile("../../map.png");
            img = new Bitmap(loadedImg);
            mapPanel.BackgroundImage = img;
            mapPanel.BackgroundImageLayout = ImageLayout.None;
        }

        private void UpdateStats()
        {
            labelPoints.Text = points.Count.ToString();
            labelSegments.Text = segments.Count.ToString();
            labelVertices.Text = vertices.Count.ToString();
            labelPaths.Text = paths.Count.ToString();
        }

        private string CreatePointKey(int x, int y)
        {
            return x + "|" + y;
        }

        private string CreatePairKey(Point p1, Point p2)
        {
            return CreatePointKey(p1.X, p1.Y) + "||" + CreatePointKey(p2.X, p2.Y);
        }

        private bool IsColored(int x, int y)
        {
            if (x >= img.Width || x < 0 || y < 0 || y >= img.Height)
                return false;
            Color c = img.GetPixel(x, y);
            return (c.R != 255 || c.G != 255 || c.B != 255);
        }

        private Point CreatePoint(int x, int y)
        {
            string key = CreatePointKey(x, y);
            if (!pointMap.ContainsKey(key))
            {
                Point point = new Point(x, y);
                points.Add(point);
                pointMap.Add(key, point);
                return point;
            }
            else return pointMap[key];
        }

        private Segment CreateLineSegement(Point p1, Point p2)
        {
            string key1 = CreatePairKey(p1, p2);
            string key2 = CreatePairKey(p2, p1);
            if (!segmentMap.ContainsKey(key1) && !segmentMap.ContainsKey(key2))
            {
                Segment segment = new Segment(p1, p2);
                segments.Add(segment);
                segmentMap.Add(key1, segment);
                segmentMap.Add(key2, segment);
                return segment;
            }
            else if (segmentMap.ContainsKey(key1))
                return segmentMap[key1];
            else
                return segmentMap[key2];
        }

        private Vertex CreateVertex(Point p, List<Segment> segments)
        {
            string key = CreatePointKey(p.X, p.Y);
            if (!vertexMap.ContainsKey(key))
            {
                Vertex vertex = new Vertex(p, segments);
                vertices.Add(vertex);
                vertexMap.Add(key, vertex);
                return vertex;
            }
            else return vertexMap[key];
        }

        private Path CreatePath(Vertex v1, Vertex v2, List<Point> points)
        {
            string key1 = CreatePairKey(v1.Point, v2.Point);
            string key2 = CreatePairKey(v2.Point, v1.Point);
            if (!pathMap.ContainsKey(key1) && !pathMap.ContainsKey(key2))
            {
                Path path = new Path(v1, v2, points);
                paths.Add(path);
                pathMap.Add(key1, path);
                pathMap.Add(key2, path);
                return path;
            }
            else if (pathMap.ContainsKey(key1))
                return pathMap[key1];
            else
                return pathMap[key2];
        }

        private void FindSegments()
        {
            DateTime start = DateTime.Now;
            for (int y = 0; y < img.Height; y++)
                for (int x = 0; x < img.Width; x++)
                    if (IsColored(x, y))
                    {
                        Point p = CreatePoint(x, y);
                        FindNeighborSegments(p);
                    }
            TimeSpan span = DateTime.Now - start;
            UpdateStats();
            Log("Done finding segments in " + span.TotalMilliseconds + "ms!");
        }

        private void FindVertices()
        {
            DateTime start = DateTime.Now;
            foreach (Point p in points)
            {
                List<Segment> connected = ListNeighborSegments(p);
                if (connected.Count != 2)
                    CreateVertex(p, connected);
            }
            TimeSpan span = DateTime.Now - start;
            UpdateStats();
            Log("Done finding vertices in " + span.TotalMilliseconds + "ms!");
        }

        private void FindPaths()
        {
            DateTime start = DateTime.Now;
            foreach (Vertex v in vertices)
                foreach (Segment s in v.Segments)
                {
                    List<Point> pointList = new List<Point>();
                    pointList.Add(v.Point);
                    FindEndVertex(v, v.Point, s, pointList);
                }
            foreach (Vertex v in vertices)
            {
                List<Path> neighborPaths = ListNeighborPaths(v);
                string key = CreatePointKey(v.Point.X, v.Point.Y);
                vertexPathMap.Add(key, neighborPaths);
            }
            TimeSpan span = DateTime.Now - start;
            UpdateStats();
            Log("Done finding paths in " + span.TotalMilliseconds + "ms!");
        }

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
            UpdateStats();
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

        private void FindNeighborSegments(Point point)
        {
            int x = point.X, y = point.Y;
            for (int i=0; i<neighbors.Length; i++)
            {
                Point n = new Point(point.X + neighbors[i].X, point.Y + neighbors[i].Y);
                if (IsColored(n.X, n.Y))
                {
                    Point np = CreatePoint(n.X, n.Y);
                    CreateLineSegement(point, np);
                }
            }
        }

        private void FindEndVertex(Vertex startVertex, Point fromPoint, Segment nextSegment, List<Point> pointList)
        {
            Point nextPoint;
            if (nextSegment.P1 == fromPoint)
                nextPoint = nextSegment.P2;
            else if (nextSegment.P2 == fromPoint)
                nextPoint = nextSegment.P1;
            else
                throw new Exception("UH-OH");

            string key = CreatePointKey(nextPoint.X, nextPoint.Y);
            if (vertexMap.ContainsKey(key))
            {
                pointList.Add(nextPoint);
                CreatePath(startVertex, vertexMap[key], pointList);
            }
            else
                foreach (Segment s in segments)
                    if (s != nextSegment && (s.P1 == nextPoint || s.P2 == nextPoint))
                    {
                        pointList.Add(nextPoint);
                        FindEndVertex(startVertex, nextPoint, s, pointList);
                    }
        }

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

        private List<Path> ListNeighborPaths(Vertex v)
        {
            List<Path> neighbors = new List<Path>();
            foreach (Path p in paths)
                if (p.V1 == v || p.V2 == v)
                    neighbors.Add(p);
            return neighbors;
        }

        private void buttonOriginal_Click(object sender, EventArgs e)
        {
            mapPanel.BackgroundImage = img;
        }

        private void buttonDrawSegments_Click(object sender, EventArgs e)
        {
            Bitmap copy = new Bitmap(mapPanel.BackgroundImage);
            Graphics g = Graphics.FromImage(copy);
            foreach (Segment s in segments)
            {
                g.DrawLine(Pens.Red, s.P1, s.P2);
            }
            mapPanel.BackgroundImage = copy;
        }

        private void buttonDrawVertices_Click(object sender, EventArgs e)
        {
            Bitmap copy = new Bitmap(mapPanel.BackgroundImage);
            Graphics g = Graphics.FromImage(copy);
            foreach (Vertex v in vertices)
            {
                g.FillRectangle(Brushes.Green, new Rectangle(v.Point.X - 2, v.Point.Y - 2, 4, 4));
            }
            mapPanel.BackgroundImage = copy;
        }

        private void buttonDrawPaths_Click(object sender, EventArgs e)
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

        private void buttonDrawShortPaths_Click(object sender, EventArgs e)
        {
            Bitmap copy = new Bitmap(mapPanel.BackgroundImage);
            Graphics g = Graphics.FromImage(copy);
            Random random = new Random();
            List<Path> shorts = ListShortPaths();
            foreach (Path p in shorts)
                g.DrawLines(new Pen(Color.Red, 3), p.Points.ToArray());
            mapPanel.BackgroundImage = copy;
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
            FindSegments();
            FindVertices();
            FindPaths();
            CleanPaths();
        }
    }
}
