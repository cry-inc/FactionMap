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

        public override string ToString()
        {
            return V1 + " - " + V2;
        }

        public void Simplify(double eps)
        {
            Points = SimplifyLine(Points, eps);
        }

        private static List<Point> SimplifyLine(List<Point> points, double eps)
        {
            Line l = new Line(points[0], points[points.Count - 1]);
            double maxDist = 0;
            int maxIndex = 0;
            for (int i = 1; i < points.Count - 1; i++) {
                double dist = l.PointDistance(points[i]);
                if (dist > maxDist)
                {
                    maxDist = dist;
                    maxIndex = i;
                }
            }
            if (maxDist > eps)
            {
                List<Point> newPoints = new List<Point>();
                List<Point> list1 = points.GetRange(0, maxIndex);
                List<Point> list2 = points.GetRange(maxIndex, points.Count - maxIndex);
                list1 = SimplifyLine(list1, eps);
                list2 = SimplifyLine(list2, eps);
                newPoints.AddRange(list1);
                newPoints.RemoveAt(newPoints.Count - 1);
                newPoints.AddRange(list2);
                return newPoints;
            }
            else
            {
                List<Point> newPoints = new List<Point>();
                newPoints.Add(points[0]);
                newPoints.Add(points[points.Count - 1]);
                return newPoints;
            }
        }
    }
}
