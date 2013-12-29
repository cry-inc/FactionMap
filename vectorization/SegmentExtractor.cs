using System;
using System.Collections.Generic;
using System.Drawing;

namespace MapExtractor
{
    class SegmentExtractor
    {
        private static Point[] neighbors = {
            new Point( 1,  0), new Point(-1,  0),
            new Point( 0,  1), new Point( 0, -1),
            new Point( 1,  1), new Point(-1, -1),
            new Point(-1,  1), new Point( 1, -1),                             
        };

        private PointExtractor pe;
        private List<Segment> segments;
        private Dictionary<long, Segment> segmentMap;
        private Dictionary<int, List<Segment>> pointSegments;

        public SegmentExtractor(PointExtractor pointExtractor)
        { pe = pointExtractor; }

        public PointExtractor PointExtractor
        { get { return pe; } }

        public List<Segment> Segments
        { get { return segments != null ? segments : ExtractSegments(); } }

        public Dictionary<int, List<Segment>> PointSegments
        { get { return pointSegments != null ? pointSegments : GetPointSegments(); } }

        public long GetSegmentKey(Segment s)
        { return GetSegmentKey(s.P1, s.P2); }

        public long GetSegmentKey(Point p1, Point p2)
        {
            int p1Key = pe.GetPointKey(p1);
            int p2Key = pe.GetPointKey(p2);
            long multiplicator = pe.Image.Width * pe.Image.Height;
            int min = p1Key < p2Key ? p1Key : p2Key;
            int max = p1Key > p2Key ? p1Key : p2Key;
            return min * multiplicator + max;
        }

        public List<Segment> ExtractSegments()
        {
            Dictionary<int, Point> pointMap = pe.BuildPointMap();
            segments = new List<Segment>();
            segmentMap = new Dictionary<long, Segment>();
            pointSegments = new Dictionary<int, List<Segment>>();
            foreach (Point p1 in pe.Points)
            {
                int p1Key = pe.GetPointKey(p1);
                List<Segment> connectedSegments = new List<Segment>();
                foreach (Point n in neighbors)
                {
                    Point p2 = new Point(p1.X + n.X, p1.Y + n.Y);
                    int p2Key = pe.GetPointKey(p2);
                    if (pointMap.ContainsKey(p2Key))
                    {
                        long segmentKey = GetSegmentKey(p1, p2);
                        if (!segmentMap.ContainsKey(segmentKey))
                        {
                            Segment segment = new Segment(p1, p2);
                            segments.Add(segment);
                            segmentMap.Add(segmentKey, segment);
                            connectedSegments.Add(segment);
                        }
                        else connectedSegments.Add(segmentMap[segmentKey]);
                    }
                }
                pointSegments.Add(p1Key, connectedSegments);
            }
            return segments;
        }

        private Dictionary<int, List<Segment>> GetPointSegments()
        {
            if (pointSegments == null)
                ExtractSegments();
            return pointSegments;
        }
    }
}
