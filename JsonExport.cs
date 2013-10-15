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
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("{");
            sb.AppendLine("\t\"polygons\": [");
            for (int i = 0; i < pe.Polygons.Count; i++)
            {
                Polygon p = pe.Polygons[i];
                sb.AppendLine("\t\t{");
                sb.AppendLine("\t\t\t\"id\": " + p.Id + ",");
                List<string> strPoints = new List<string>();
                foreach (Point op in p.Points)
                {
                    PointF cp = ConvertPoint(op);
                    strPoints.Add("{\"x\": " + cp.X.ToString(CultureInfo.InvariantCulture) + ", \"y\": " + cp.Y.ToString(CultureInfo.InvariantCulture) + "}");
                }
                sb.AppendLine("\t\t\t\"points\": [" + String.Join(", ", strPoints.ToArray()) + "],");
                List<string> strNeighbors = new List<string>();
                foreach (Polygon n in pe.PolygonNeighbors[p.Id])
                    strNeighbors.Add(n.Id.ToString());
                sb.AppendLine("\t\t\t\"neighbors\": [" + String.Join(", ", strNeighbors.ToArray()) + "]");
                sb.Append("\t\t}");
                if (i != pe.Polygons.Count - 1)
                    sb.Append(",");
                sb.AppendLine();
            }
            sb.AppendLine("\t]");
            sb.AppendLine("}");
            return sb.ToString();
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
