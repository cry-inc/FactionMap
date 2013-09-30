using System;
using System.Collections.Generic;
using System.Drawing;

namespace MapExtractor
{
    class PathExtractor
    {
        private VertexExtractor ve;
        private List<Path> paths;
        private Dictionary<long, Path> pathMap;
        private Dictionary<int, List<Path>> vertexPaths;

        public PathExtractor(VertexExtractor segmentExtractor)
        { ve = segmentExtractor; }

        public PointExtractor PointExtractor
        { get { return ve.PointExtractor; } }

        public SegmentExtractor SegmentExtractor
        { get { return ve.SegmentExtractor; } }

        public VertexExtractor VertexExtractor
        { get { return ve; } }

        public List<Path> Paths
        { get { return paths != null ? paths : ExtractPaths(); } }

        public Dictionary<long, Path> PathMap
        { get { return pathMap != null ? pathMap : GetPathMap(); } }

        public Dictionary<int, List<Path>> VertexPaths
        { get { return vertexPaths != null ? vertexPaths : GetVertexPaths(); } }

        public List<Path> ExtractPaths()
        {
            paths = new List<Path>();
            pathMap = new Dictionary<long, Path>();
            vertexPaths = new Dictionary<int, List<Path>>();
            foreach (Vertex v in ve.Vertices)
                foreach (Segment s in v.Segments)
                {
                    List<Point> pointList = new List<Point>();
                    pointList.Add(v.Point);
                    FindEndVertex(v, v.Point, s, pointList);
                }
            return paths;
        }

        private void FindEndVertex(Vertex startVertex, Point fromPoint, Segment nextSegment, List<Point> pointList)
        {
            Point nextPoint = (nextSegment.P2 == fromPoint) ? nextSegment.P1 : nextSegment.P2;
            int nextPointKey = ve.PointExtractor.GetPointKey(nextPoint);
            if (ve.VertexMap.ContainsKey(nextPointKey))
            {
                Vertex endVertex = ve.VertexMap[nextPointKey];
                long pathKey = ve.SegmentExtractor.GetSegmentKey(startVertex.Point, endVertex.Point);
                if (!pathMap.ContainsKey(pathKey))
                {
                    pointList.Add(nextPoint);
                    Path path = new Path(startVertex, endVertex, pointList);
                    paths.Add(path);
                    pathMap.Add(pathKey, path);
                    InsertVertexPath(startVertex, path);
                    InsertVertexPath(endVertex, path);
                }
            }
            else
            {
                List<Segment> toCheck = ve.SegmentExtractor.PointSegments[nextPointKey];
                foreach (Segment s in toCheck)
                    if (s != nextSegment && (s.P1 == nextPoint || s.P2 == nextPoint))
                    {
                        pointList.Add(nextPoint);
                        // TODO: Remove recursion
                        FindEndVertex(startVertex, nextPoint, s, pointList);
                    }
            }
        }

        private void InsertVertexPath(Vertex vertex, Path path)
        {
            int key = ve.PointExtractor.GetPointKey(vertex.Point);
            List<Path> paths;
            if (vertexPaths.ContainsKey(key))
                paths = vertexPaths[key];
            else
            {
                paths = new List<Path>();
                vertexPaths.Add(key, paths);
            }
            paths.Add(path);
        }

        private Dictionary<long, Path> GetPathMap()
        {
            if (pathMap == null)
                ExtractPaths();
            return pathMap;
        }

        private Dictionary<int, List<Path>> GetVertexPaths()
        {
            if (vertexPaths == null)
                ExtractPaths();
            return vertexPaths;
        }
    }
}
