using System;
using System.Collections.Generic;
using System.Drawing;

namespace MapExtractor
{
    class Path
    {
        public Vertex V1, V2;
        public List<Point> Points;

        public Path(Vertex v1, Vertex v2, List<Point> p)
        { V1 = v1; V2 = v2; Points = p; }

        public double Length()
        {
            double length = 0;
            for (int i = 0; i < Points.Count - 1; i++)
            {
                int xd = Points[i].X - Points[i + 1].X;
                int yd = Points[i].Y - Points[i + 1].Y;
                length += Math.Sqrt(xd * xd + yd * yd);
            }
            return length;
        }
    }
}
