using System;
using System.Drawing;
using System.Collections.Generic;

namespace MapExtractor
{
    class Edge
    {
        public List<Point> Points;
        public Node[] Nodes;
        public HalfEdge[] HalfEdges;
        
        public Edge(Node n1, Node n2, List<Point> points)
        {
            Points = points;
            HalfEdges = new HalfEdge[] { new HalfEdge(n1, n2, this), new HalfEdge(n2, n1, this) };
            Nodes = new Node[] { n1, n2 };
        }
    }
}
