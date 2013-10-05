using System;
using System.Collections.Generic;
using System.Drawing;

namespace MapExtractor
{
    class PolygonExtractor
    {
        private Bitmap bmp;
        public List<Edge> Edges = new List<Edge>();
        public List<HalfEdge> HalfEdges = new List<HalfEdge>();
        public List<Node> Nodes = new List<Node>();
        public Dictionary<int, Node> NodeMap = new Dictionary<int, Node>();
        private List<Polygon> polygons;

        public List<Polygon> Polygons
        { get { return (polygons == null) ? ExtractPolygons() : polygons; } }

        public PolygonExtractor(List<Path> paths, Bitmap image)
        {
            bmp = image;
            foreach (Path p in paths)
            {
                Node node1, node2;
                int node1Key = GetNodeKey(p.V1.Point.X, p.V1.Point.Y);
                if (NodeMap.ContainsKey(node1Key))
                    node1 = NodeMap[node1Key];
                else
                {
                    node1 = new Node(p.V1.Point.X, p.V1.Point.Y);
                    Nodes.Add(node1);
                    NodeMap.Add(node1Key, node1);
                }
                int node2Key = GetNodeKey(p.V2.Point.X, p.V2.Point.Y);
                if (NodeMap.ContainsKey(node2Key))
                    node2 = NodeMap[node2Key];
                else
                {
                    node2 = new Node(p.V2.Point.X, p.V2.Point.Y);
                    Nodes.Add(node2);
                    NodeMap.Add(node2Key, node2);
                }
                Edge edge = new Edge(node1, node2, p.Points);
                Edges.Add(edge);
                HalfEdges.Add(edge.HalfEdges[0]);
                HalfEdges.Add(edge.HalfEdges[1]);
            }
        }

        public List<Polygon> ExtractPolygons()
        {
            polygons = new List<Polygon>();
            foreach (Node n in Nodes)
            {
                // TODO: finsish
                /*
                foreach (HalfEdge h in n.Outgoing.HalfEdges.Unvisited)
                {
                    List<HalfEdge> halfEdges = new List<HalfEdge>();
                    // follow the most "right" halfegdes until loop is closed at n
                    // mark all visited halfedges as used/visited
                    Polygon polygon = new Polygon();
                    polygon.Edges = halfEdges;
                    polygons.Add(polygon);
                }
                */
            }
            return polygons;
        }

        public int GetNodeKey(Node n)
        { return GetNodeKey(n.X, n.Y); }

        public int GetNodeKey(int x, int y)
        { return y * bmp.Width + x ; }
    }
}
