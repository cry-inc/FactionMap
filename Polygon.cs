using System;
using System.Collections.Generic;
using System.Drawing;

namespace MapExtractor
{
    class Polygon
    {
        public int Id;
        public List<HalfEdge> HalfEdges;
        public List<Edge> Edges = new List<Edge>();
        public List<Point> Points = new List<Point>();
        public Point Centroid = new Point(0, 0);
        public int XMin, YMin, XMax, YMax;

        public Polygon(List<HalfEdge> halfEdges, int id)
        {
            HalfEdges = halfEdges;
            Id = id;

            foreach (HalfEdge he in HalfEdges)
                Edges.Add(he.Edge);

            
            foreach (HalfEdge he in HalfEdges)
                Points.AddRange(he.GetPoints());

            XMin = XMax = Points[0].X;
            YMin = YMax = Points[0].Y;
            foreach (Point p in Points)
            {
                Centroid.X += p.X;
                Centroid.Y += p.Y;
                if (p.X < XMin) XMin = p.X;
                if (p.X > XMax) XMax = p.X;
                if (p.Y < YMin) YMin = p.Y;
                if (p.Y > YMax) YMax = p.Y;
            }
            Centroid.X /= Points.Count;
            Centroid.Y /= Points.Count;
        }

        public bool IsClockWise()
        {
            double sum = 0;
            foreach (HalfEdge he in HalfEdges)
                sum += (he.End.X - he.Start.X) * (he.End.Y + he.Start.Y);
            return sum < 0;
        }

        public bool IsInside(Point p)
        {
            if (p.X >= XMin && p.X <= XMax && p.Y > YMin && p.Y < YMax)
            {
                int i, j;
                bool c = false;
                for (i = 0, j = Points.Count - 1; i < Points.Count; j = i++)
                {
                    if ((((Points[i].X <= p.X) && (p.X < Points[j].X)) || ((Points[j].X <= p.X) && (p.X < Points[i].X))) &&
                    (p.Y < (Points[j].Y - Points[i].Y) * (p.X - Points[i].X) / (Points[j].X - Points[i].X) + Points[i].Y))
                        c = !c;
                }
                return c;
            }
            return false;
        }
    }
}
