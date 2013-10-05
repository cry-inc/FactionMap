using System;

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
    }
}
