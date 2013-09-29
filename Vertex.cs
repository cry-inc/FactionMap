using System;
using System.Collections.Generic;
using System.Drawing;

namespace MapExtractor
{
    class Vertex
    {
        public Point Point;
        public List<Segment> Segments;

        public Vertex(Point p, List<Segment> s)
        { Point = p; Segments = s; }
    }
}
