using System;
using System.Collections.Generic;
using System.Drawing;

namespace MapExtractor
{
    class Polygon
    {
        public List<HalfEdge> HalfEdges;
        public List<Edge> Edges;
        public int Id;

        public Polygon(List<HalfEdge> halfEdges, int id)
        {
            HalfEdges = halfEdges;
            Id = id;
            Edges = new List<Edge>();
            foreach (HalfEdge he in HalfEdges)
                Edges.Add(he.Edge);
        }

        public Point[] GetPoints()
        {
            List<Point> points = new List<Point>();
            foreach (HalfEdge he in HalfEdges)
                points.AddRange(he.GetPoints());
            return points.ToArray();
        }

        public bool IsClockWise()
        {
            double sum = 0;
            foreach (HalfEdge he in HalfEdges)
                sum += (he.End.X - he.Start.X) * (he.End.Y + he.Start.Y);
            return sum < 0;
        }
    }
}
