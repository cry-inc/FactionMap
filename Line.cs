using System;
using System.Drawing;

namespace MapExtractor
{
    class Line
    {
        public Point P1, P2;

        public Line(Point p1, Point p2)
        {
            P1 = p1;
            P2 = p2;
        }

        public double PointDistance(Point p)
        {
            return PointDistance(P1, P2, p);
        }

        public static double PointDistance(Point v, Point w, Point p)
        {
            double l2 = DistanceSquared(v, w);
            if (l2 == 0.0) return Distance(p, v);
            double t = Dot(Vector(p, v), Vector(w, v)) / l2;
            if (t < 0.0) return Distance(p, v);
            else if (t > 1.0) return Distance(p, w);
            PointF tmp = Multiply(Vector(w, v), t);
            PointF projection = new PointF(v.X + tmp.X, v.Y + tmp.Y);
            return Distance(p, projection);
        }

        public static PointF Vector(PointF v, PointF w)
        {
            return new PointF(v.X - w.X, v.Y - w.Y);
        }

        public static double Dot(PointF v, PointF w)
        {
            return v.X * w.X + v.Y * w.Y;
        }

        public static double DistanceSquared(PointF v, PointF w)
        {
            PointF n = Vector(v, w);
            return (n.X * n.X + n.Y * n.Y);
        }

        public static double Distance(PointF v, PointF w)
        {
            return Math.Sqrt(DistanceSquared(v, w));
        }

        public static PointF Multiply(PointF p, double v)
        {
            return new PointF(p.X * (float)v, p.Y * (float)v);
        }
    }
}
