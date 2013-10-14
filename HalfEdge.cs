using System;
using System.Drawing;
using System.Collections.Generic;

namespace MapExtractor
{
    class HalfEdge
    {
        public Node Start, End;
        public Edge Edge;

        public HalfEdge(Node start, Node end, Edge edge)
        {
            Start = start;
            End = end;
            Edge = edge;
        }

        public override string ToString()
        {
            return Start + " -> " + End;
        }

        public Point[] GetPoints()
        {
            if (Start.X == Edge.Points[0].X && Start.Y == Edge.Points[0].Y)
                return Edge.Points.ToArray();
            else
            {
                List<Point> copy = new List<Point>(Edge.Points);
                copy.Reverse();
                return copy.ToArray();
            }
        }
    }
}
