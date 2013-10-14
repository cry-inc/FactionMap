using System;
using System.Collections.Generic;
using System.Drawing;

namespace MapExtractor
{
    class Polygon
    {
        public List<HalfEdge> Edges;

        public Polygon(List<HalfEdge> edges)
        {
            Edges = edges;
        }

        public bool IsInside(int x, int y)
        {
            // TODO: Implement
            return false;
        }

        public Point[] GetPoints()
        {
            List<Point> points = new List<Point>();
            foreach (HalfEdge he in Edges)
                points.AddRange(he.GetPoints());
            return points.ToArray();
        }

        public bool IsClockWise()
        {
            double sum = 0;
            foreach (HalfEdge he in Edges)
                sum += (he.End.X - he.Start.X) * (he.End.Y + he.Start.Y);
            return sum < 0;
        }
    }
}
