using System;
using System.Collections.Generic;
using System.Drawing;

namespace MapExtractor
{
    class VertexExtractor
    {
        private SegmentExtractor se;
        private List<Vertex> vertices;
        private Dictionary<int, Vertex> vertexMap;

        public VertexExtractor(SegmentExtractor segmentExtractor)
        { se = segmentExtractor; }

        public PointExtractor PointExtractor
        { get { return se.PointExtractor; } }

        public SegmentExtractor SegmentExtractor
        { get { return se; } }

        public List<Vertex> Vertices
        { get { return vertices != null ? vertices : ExtractVertices(); } }

        public Dictionary<int, Vertex> VertexMap
        { get { return vertexMap != null ? vertexMap : GetVertexMap(); } }

        public int GetVertexKey(Vertex v)
        { return GetVertexKey(v.Point); }

        public int GetVertexKey(Point p)
        { return PointExtractor.GetPointKey(p); }

        public List<Vertex> ExtractVertices()
        {
            vertices = new List<Vertex>();
            vertexMap = new Dictionary<int, Vertex>();
            foreach (Point p in se.PointExtractor.Points)
            {
                int key = GetVertexKey(p);
                List<Segment> connectedSegments = se.PointSegments[key];
                if (connectedSegments.Count != 2)
                {
                    Vertex vertex = new Vertex(p, connectedSegments);
                    vertices.Add(vertex);
                    vertexMap.Add(key, vertex);
                }
            }
            return vertices;
        }

        private Dictionary<int, Vertex> GetVertexMap()
        {
            if (vertexMap == null)
                ExtractVertices();
            return vertexMap;
        }
    }
}
