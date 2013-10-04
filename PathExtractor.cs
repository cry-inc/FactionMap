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

        public long GetPathKey(Path p)
        { return GetPathKey(p.V1, p.V2); }

        public long GetPathKey(Vertex v1, Vertex v2)
        { return SegmentExtractor.GetSegmentKey(v1.Point, v2.Point); }

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
            int nextPointKey = PointExtractor.GetPointKey(nextPoint);
            if (ve.VertexMap.ContainsKey(nextPointKey))
            {
                Vertex endVertex = ve.VertexMap[nextPointKey];
                long pathKey = GetPathKey(startVertex, endVertex);
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
                List<Segment> toCheck = SegmentExtractor.PointSegments[nextPointKey];
                foreach (Segment s in toCheck)
                    if (s != nextSegment && (s.P1 == nextPoint || s.P2 == nextPoint))
                    {
                        pointList.Add(nextPoint);
                        FindEndVertex(startVertex, nextPoint, s, pointList);
                    }
            }
        }

        private void InsertVertexPath(Vertex vertex, Path path)
        {
            int key = VertexExtractor.GetVertexKey(vertex);
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

        public List<Path> FindLoosePaths()
        {
            List<Path> loosePaths = new List<Path>();
            foreach (Path p in paths)
            {
                int v1Key = VertexExtractor.GetVertexKey(p.V1);
                int v2Key = VertexExtractor.GetVertexKey(p.V2);
                int v1PathCount = vertexPaths[v1Key].Count;
                int v2PathCount = vertexPaths[v2Key].Count;
                if (v1PathCount == 1 || v2PathCount == 1)
                    loosePaths.Add(p);
            }
            return loosePaths;
        }

        public int RemoveLooseEnds()
        {
            int removed = 0;
            List<Path> loosePaths = FindLoosePaths();
            while (loosePaths.Count > 0)
            {
                removed += loosePaths.Count;
                foreach (Path p in loosePaths)
                    RemovePath(p);
                loosePaths = FindLoosePaths();
            }
            return removed;
        }

        public void RemovePath(Path path)
        {
            long key = GetPathKey(path);
            paths.Remove(path);
            pathMap.Remove(key);
            int v1Key = VertexExtractor.GetVertexKey(path.V1);
            int v2Key = VertexExtractor.GetVertexKey(path.V2);
            if (vertexPaths.ContainsKey(v1Key))
                vertexPaths[v1Key].Remove(path);
            if (vertexPaths.ContainsKey(v2Key))
                vertexPaths[v2Key].Remove(path);
        }

        public int CollapseVertices(double collapseDist)
        {
            int collapsed = 0;
            for (int i = 0; i < ve.Vertices.Count; i++)
            {
                List<Vertex> neighbors = new List<Vertex>();
                for (int j = 0; j < ve.Vertices.Count; j++)
                    if (i != j)
                    {
                        double dist = ve.Vertices[i].GetDistance(ve.Vertices[j]);
                        if (dist < collapseDist)
                            neighbors.Add(ve.Vertices[j]);
                    }
                if (neighbors.Count > 0)
                {
                    neighbors.Add(ve.Vertices[i]);
                    MergeVertices(neighbors);
                    i = -1;
                    collapsed++;
                }
            }
            return collapsed;
        }

        private Vertex GetCentroid(List<Vertex> vertices)
        {
            int sumX = 0, sumY = 0;
            foreach (Vertex v in vertices)
            {
                sumX += v.Point.X;
                sumY += v.Point.Y;
            }
            Point centroid = new Point(sumX / vertices.Count, sumY / vertices.Count);
            return new Vertex(centroid, new List<Segment>());
        }

        private void MergeVertices(List<Vertex> mergeList)
        {
            Vertex newVertex = GetCentroid(mergeList);
            for (int i = 0; i < mergeList.Count; i++)
            {
                Vertex toDelete = mergeList[i];
                int vertexKey = VertexExtractor.GetVertexKey(toDelete);
                List<Path> affectedPaths = new List<Path>(vertexPaths[vertexKey]);
                ve.Vertices.Remove(toDelete);
                ve.VertexMap.Remove(vertexKey);
                vertexPaths.Remove(vertexKey);
                for (int j = 0; j < affectedPaths.Count; j++)
                {
                    Path path = affectedPaths[j];
                    RemovePath(path);
                    if (path.V1 == toDelete)
                        path.V1 = newVertex;
                    else
                        path.V2 = newVertex;
                    long pathKey = GetPathKey(path);
                    if (path.V1 != path.V2 && !pathMap.ContainsKey(pathKey))
                    {
                        if (path.Points[0] == toDelete.Point)
                            path.Points.Insert(0, newVertex.Point);
                        else
                            path.Points.Add(newVertex.Point);
                        paths.Add(path);
                        pathMap.Add(pathKey, path);
                        InsertVertexPath(path.V1, path);
                        InsertVertexPath(path.V2, path);
                    }
                }
            }
            int newKey = VertexExtractor.GetVertexKey(newVertex);
            ve.Vertices.Add(newVertex);
            ve.VertexMap.Add(newKey, newVertex);
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
