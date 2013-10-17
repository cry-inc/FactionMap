using System;
using System.Drawing;
using System.Text;
using System.Collections.Generic;
using System.Globalization;

namespace MapExtractor
{
    class JsonExport
    {
        private PolygonExtractor pe;
        Bitmap bmp;

        public JsonExport(PolygonExtractor polygonExtractor, Bitmap bitmap)
        {
            pe = polygonExtractor;
            bmp = bitmap;
        }

        public string CreateJson()
        {
            List<string> polygonStrings = new List<string>();
            foreach (Polygon p in pe.Polygons)
                polygonStrings.Add(CreatePolygonString(p));
            return "{\"provinces\":[" + String.Join(",", polygonStrings) + "]}\n";
        }

        private string CreatePolygonString(Polygon p)
        {
            List<string> edgeStrings = new List<string>();
            foreach (HalfEdge he in p.HalfEdges)
                edgeStrings.Add(CreateHalfEdgeString(he, p));
            return "{\"id\": " + p.Id + ", \"edges\": [" + String.Join(",", edgeStrings) + "]}";
        }

        private string CreateHalfEdgeString(HalfEdge he, Polygon p)
        {
            long edgeKey = pe.GetEdgeKey(he.Edge);
            Polygon neighbor = null;
            foreach (Polygon candidate in pe.EdgePolygons[edgeKey])
                if (candidate != p)
                    neighbor = candidate;
            string neighborStr = (neighbor != null) ? neighbor.Id.ToString() : "-1";

            List<string> xp = new List<string>();
            List<string> yp = new List<string>();
            Point[] points = he.GetPoints();
            foreach (Point op in points)
            {
                PointF cp = ConvertPoint(op);
                xp.Add(cp.X.ToString(CultureInfo.InvariantCulture));
                yp.Add(cp.Y.ToString(CultureInfo.InvariantCulture));
            }
            string xPoints = String.Join(",", xp);
            string yPoints = String.Join(",", yp);
            return "{\"neighbor\": " + neighborStr + ", \"xpoints\": [" + xPoints + "], \"ypoints\": [" + yPoints + "]}";
        }

        private PointF ConvertPoint(PointF org)
        {
            return new PointF(
                org.X / bmp.Width,
                org.Y / bmp.Height
            );
        }
    }
}
