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
        public Dictionary<int, List<HalfEdge>> NodeHalfEdges = new Dictionary<int, List<HalfEdge>>();

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
                AddEdge(edge);
            }
        }

        private void AddEdge(Edge e)
        {
            Edges.Add(e);
            AddHalfEdge(e.HalfEdges[0]);
            AddHalfEdge(e.HalfEdges[1]);
        }

        private void AddHalfEdge(HalfEdge he)
        {
            HalfEdges.Add(he);
            int key = GetNodeKey(he.Start);
            if (NodeHalfEdges.ContainsKey(key))
                NodeHalfEdges[key].Add(he);
            else
            {
                List<HalfEdge> list = new List<HalfEdge>();
                list.Add(he);
                NodeHalfEdges.Add(key, list);
            }
        }

        public List<Polygon> ExtractPolygons()
        {
            Dictionary<long,bool> vistedHalfEdges = new Dictionary<long,bool>();
            polygons = new List<Polygon>();
            foreach (Node n in Nodes)
            {
                int nodeKey = GetNodeKey(n);
                foreach (HalfEdge he in NodeHalfEdges[nodeKey])
                {
                    long halfEdgeKey = GetHalfEdgeKey(he);
                    if (!vistedHalfEdges.ContainsKey(halfEdgeKey))
                    {
                        List<HalfEdge> edgesList = new List<HalfEdge>();
                        HalfEdge currentHalfEdge = he;
                        long currentKey = GetHalfEdgeKey(currentHalfEdge);
                        edgesList.Add(currentHalfEdge);
                        vistedHalfEdges.Add(currentKey, true);
                        bool deadEnd = false;
                        while (currentHalfEdge.End != n && !deadEnd)
                        {
                            currentHalfEdge = FindNextUnvisitedHalfEdge(currentHalfEdge, vistedHalfEdges);
                            if (currentHalfEdge == null)
                            {
                                deadEnd = true;
                                break;
                            }
                            currentKey = GetHalfEdgeKey(currentHalfEdge);
                            edgesList.Add(currentHalfEdge);
                            vistedHalfEdges.Add(currentKey, true);
                        }
                        Polygon polygon = new Polygon(edgesList);
                        if (!deadEnd && polygon.IsClockWise())
                            polygons.Add(polygon);
                        else
                        {
                            foreach (HalfEdge used in edgesList)
                            {
                                long key = GetHalfEdgeKey(used);
                                vistedHalfEdges.Remove(key);
                            }
                        }
                    }
                }
            }
            return polygons;
        }

        public HalfEdge FindNextUnvisitedHalfEdge(HalfEdge halfEdge, Dictionary<long,bool> vistedHalfEdges)
        {
            HalfEdge nextHalfEdge = FindNextHalfEdge(halfEdge);
            if (nextHalfEdge == null)
                return null;
            long nextHalfEdgeKey = GetHalfEdgeKey(nextHalfEdge);
            if (vistedHalfEdges.ContainsKey(nextHalfEdgeKey))
                return null;
            else
                return nextHalfEdge;
        }

        private double FixAngle(double angle)
        {
            while (angle < 0) angle += Math.PI * 2;
            while (angle > Math.PI * 2) angle -= Math.PI * 2;
            return angle;
        }

        private HalfEdge FindNextHalfEdge(HalfEdge inEdge)
        {
            int nodeKey = GetNodeKey(inEdge.End);
            List<HalfEdge> outgoingEdges = new List<HalfEdge>(NodeHalfEdges[nodeKey]);
            int counterPart = -1;
            for (int i=0; i<outgoingEdges.Count; i++)
                if (outgoingEdges[i].Edge == inEdge.Edge)
                    counterPart = i;
            double inAngle = FixAngle(GetHalfEdgeAngle(outgoingEdges[counterPart]));
            outgoingEdges.RemoveAt(counterPart);
            double[] angles = new double[outgoingEdges.Count];
            int greater = -1, smaller = -1;
            for (int i=0; i<angles.Length; i++)
            {
                angles[i] = FixAngle(GetHalfEdgeAngle(outgoingEdges[i]));
                if (angles[i] > inAngle)
                    greater = i;
                if (angles[i] < inAngle)
                    smaller = i;
            }
            if (greater >= 0)
            {
                for (int i = 0; i<angles.Length; i++)
                        if (angles[i] > inAngle && angles[i] < angles[greater])
                            greater = i;
                return outgoingEdges[greater];
            }
            if (smaller >= 0)
            {
                for (int i = 0; i < angles.Length; i++)
                        if (angles[i] < inAngle && angles[i] < angles[smaller])
                            smaller = i;
                return outgoingEdges[smaller];
            }
            return null;
        }

        private double GetHalfEdgeAngle(HalfEdge he)
        {
            var points = he.Edge.Points;
            Line line;
            if (he.Start.X == points[0].X && he.Start.Y == points[0].Y)
                line = new Line(new Point(he.Start.X, he.Start.Y), new Point(points[1].X, points[1].Y));
            else
                line = new Line(new Point(he.Start.X, he.Start.Y), new Point(points[points.Count - 2].X, points[points.Count - 2].Y));
            return line.Angle();
        }

        private double GetOutgoingHalfEdgeAngle(HalfEdge he)
        {
            var points = he.Edge.Points;
            Line line;
            if (he.Start.X == points[0].X && he.Start.Y == points[0].Y)
                line = new Line(new Point(he.Start.X, he.Start.Y), new Point(points[1].X, points[1].Y));
            else
                line = new Line(new Point(he.Start.X, he.Start.Y), new Point(points[points.Count - 2].X, points[points.Count - 2].Y));
            return line.Angle();
        }

        public long GetEdgeKey(Node n1, Node n2)
        {
            int k1 = GetNodeKey(n1);
            int k2 = GetNodeKey(n2);
            int min = k1 < k2 ? k1 : k2;
            int max = k1 > k2 ? k1 : k2;
            long multiplicator = bmp.Width * bmp.Height;
            return min * multiplicator + max;
        }

        public long GetHalfEdgeKey(Node start, Node end)
        {
            int startKey = GetNodeKey(start);
            int endKey = GetNodeKey(end);
            long multiplicator = bmp.Width * bmp.Height;
            return startKey * multiplicator + endKey;
        }

        public long GetEdgeKey(Edge e)
        { return GetEdgeKey(e.Nodes[0], e.Nodes[1]); }

        public long GetHalfEdgeKey(HalfEdge he)
        { return GetHalfEdgeKey(he.Start, he.End); }

        public int GetNodeKey(Node n)
        { return GetNodeKey(n.X, n.Y); }

        public int GetNodeKey(int x, int y)
        { return y * bmp.Width + x ; }
    }
}
