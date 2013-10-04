using System;
using System.Collections.Generic;
using System.Drawing;

namespace MapExtractor
{
    class Vertex
    {
        public Point Point;
        public List<Segment> Segments;

        public Vertex(Point p, List<Segment> s)
        { Point = p; Segments = s; }

        public double GetDistance(Vertex other)
        {
            double xd = Point.X - other.Point.X;
            double yd = Point.Y - other.Point.Y;
            double squared = xd * xd + yd * yd;
            return Math.Sqrt(squared);
        }

        public override string ToString()
        {
            return Point.ToString();
        }
    }
}
