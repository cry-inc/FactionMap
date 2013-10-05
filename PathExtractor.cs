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
            vertexPaths[v1Key].Remove(path);
            if (vertexPaths[v1Key].Count == 0)
            {
                ve.Vertices.Remove(path.V1);
                ve.VertexMap.Remove(v1Key);
                vertexPaths.Remove(v1Key);
            }
            vertexPaths[v2Key].Remove(path);
            if (vertexPaths[v2Key].Count == 0)
            {
                ve.Vertices.Remove(path.V2);
                ve.VertexMap.Remove(v2Key);
                vertexPaths.Remove(v2Key);
            }
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

        private void RebuildVerticesFromPaths()
        {
            ve.Vertices.Clear();
            ve.VertexMap.Clear();
            foreach (Path p in paths)
            {
                int v1Key = ve.GetVertexKey(p.V1);
                if (!ve.VertexMap.ContainsKey(v1Key))
                {
                    ve.Vertices.Add(p.V1);
                    ve.VertexMap.Add(v1Key, p.V1);
                }
                int v2Key = ve.GetVertexKey(p.V2);
                if (!ve.VertexMap.ContainsKey(v2Key))
                {
                    ve.Vertices.Add(p.V2);
                    ve.VertexMap.Add(v2Key, p.V2);
                }
            }
        }

        private void RebuildPaths()
        {
            pathMap.Clear();
            vertexPaths.Clear();
            for (int i = 0; i < paths.Count; i++)
            {
                long key = GetPathKey(paths[i]);
                if (!pathMap.ContainsKey(key))
                {
                    pathMap.Add(key, paths[i]);
                    InsertVertexPath(paths[i].V1, paths[i]);
                    InsertVertexPath(paths[i].V2, paths[i]);
                }
                else
                    paths.RemoveAt(i--);
            }
        }

        private void MergeVertices(List<Vertex> mergeList)
        {
            Vertex newVertex = GetCentroid(mergeList);
            for (int i = 0; i < paths.Count; i++)
            {
                bool foundV1 = false, foundV2 = false;
                for (int j = 0; j < mergeList.Count; j++)
                {
                    if (paths[i].V1 == mergeList[j])
                        foundV1 = true;
                    if (paths[i].V2 == mergeList[j])
                        foundV2 = true;
                }
                if (foundV1 && foundV2)
                {
                    paths.RemoveAt(i--);
                    continue;
                }
                else if (foundV1)
                {
                    if (paths[i].Points[0] == paths[i].V1.Point)
                        paths[i].Points.Insert(0, newVertex.Point);
                    else
                        paths[i].Points.Add(newVertex.Point);
                    paths[i].V1 = newVertex;
                }
                else if (foundV2)
                {
                    if (paths[i].Points[0] == paths[i].V2.Point)
                        paths[i].Points.Insert(0, newVertex.Point);
                    else
                        paths[i].Points.Add(newVertex.Point);
                    paths[i].V2 = newVertex;
                }
            }
            RebuildVerticesFromPaths();
            RebuildPaths();
        }

        private void MergePaths(Path p1, Path p2, Vertex sharedVertex, Vertex start, Vertex end)
        {
            List<Point> points = new List<Point>();
            if (p1.Points[0] != start.Point)
                p1.Points.Reverse();
            points.AddRange(p1.Points);
            if (p2.Points[p2.Points.Count - 1] != end.Point)
                p2.Points.Reverse();
            points.AddRange(p2.Points);
            Path newPath = new Path(start, end, points);
            long key = GetPathKey(newPath);
            paths.Add(newPath);
            pathMap.Add(key, newPath);
            InsertVertexPath(start, newPath);
            InsertVertexPath(end, newPath);
            RemovePath(p1);
            RemovePath(p2);
        }

        public int MergeConsecutivePaths()
        {
            int merged = 0;
            for (int i = 0; i < ve.Vertices.Count; i++)
            {
                int key = ve.GetVertexKey(ve.Vertices[i]);
                List<Path> candiates = vertexPaths[key];
                if (candiates.Count == 2)
                {
                    Vertex start = candiates[0].V1 == ve.Vertices[i] ? candiates[0].V2 : candiates[0].V1;
                    Vertex end = candiates[1].V1 == ve.Vertices[i] ? candiates[1].V2 : candiates[1].V1;
                    long newPathKey = GetPathKey(start, end);
                    if (!pathMap.ContainsKey(newPathKey))
                    {
                        MergePaths(candiates[0], candiates[1], ve.Vertices[i], start, end);
                        i = -1;
                        merged++;
                    }
                }
            }
            return merged;
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
