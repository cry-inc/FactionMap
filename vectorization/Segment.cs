using System;
using System.Drawing;

namespace MapExtractor
{
    class Segment
    {
        public Point P1, P2;

        public Segment(Point p1, Point p2)
        { P1 = p1; P2 = p2; }

        public override string ToString()
        {
            return P1 + " - " + P2;
        }
    }
}
