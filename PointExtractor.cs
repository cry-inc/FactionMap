using System;
using System.Collections.Generic;
using System.Drawing;

namespace MapExtractor
{
    class PointExtractor
    {
        private Bitmap bmp;
        private List<Point> points;
        private Dictionary<int, Point> pointMap;

        public PointExtractor(Bitmap binaryImage)
        { bmp = binaryImage; }

        public Bitmap Image
        { get { return bmp; } }

        public List<Point> Points
        { get { return points != null ? points : ExtractPoints(); } }

        public Dictionary<int, Point> PointMap
        { get { return pointMap != null ? pointMap : BuildPointMap(); } }

        public int GetPointKey(int x, int y)
        { return y * bmp.Width + x; }

        public int GetPointKey(Point p)
        { return p.Y * bmp.Width + p.X; }

        public List<Point> ExtractPoints()
        {
            points = new List<Point>();
            for (int y = 0; y < bmp.Height; y++)
                for (int x = 0; x < bmp.Width; x++)
                {
                    Color c = bmp.GetPixel(x, y);
                    if (c.R != 255)
                        points.Add(new Point(x, y));
                }
            return points;
        }

        public Dictionary<int, Point> BuildPointMap()
        {
            if (points == null)
                ExtractPoints();

            pointMap = new Dictionary<int, Point>();
            foreach (Point p in points)
            {
                int key = GetPointKey(p);
                pointMap.Add(key, p);
            }
            return pointMap;
        }
    }
}
