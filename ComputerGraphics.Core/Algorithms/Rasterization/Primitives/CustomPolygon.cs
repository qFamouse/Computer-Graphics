using System.Collections.Generic;
using System.Linq;

namespace ComputerGraphics.Core.Algorithms.Rasterization.Primitives
{
    public struct CustomPolygon
    {
        public List<CustomLine> Lines
        {
            get
            {
                var result = new List<CustomLine>();
                for (var i = 0; i < Points.Count; i++)
                {
                    result.Add(new CustomLine(Points[i], Points[(i + 1) % Points.Count]));
                }

                return result;
            }
        }

        public IReadOnlyList<CustomPoint> Points { get; set; }

        public CustomPolygon(IEnumerable<CustomPoint> points)
        {
            var pointsArray = points as CustomPoint[] ?? points.ToArray();
            Points = new List<CustomPoint>(pointsArray);

            for (var i = 0; i < pointsArray.Length; i++)
            {
                Lines.Add(new CustomLine(pointsArray[i], pointsArray[(i + 1) % pointsArray.Length]));
            }
        }

        public CustomPolygon(int[] points)
        {
            var pointsList = new List<CustomPoint>();
            for (int i = 0; i < points.Length; i+=2)
            {
                pointsList.Add(new CustomPoint(points[i], points[i + 1]));
            }

            var pointsArray = pointsList.ToArray();

            Points = new List<CustomPoint>(pointsArray);

            for (var i = 0; i < pointsArray.Length; i++)
            {
                Lines.Add(new CustomLine(pointsArray[i], pointsArray[(i + 1) % pointsArray.Length]));
            }
        }

        public CustomPolygon(IEnumerable<(double X, double Y)> points)
        {
            var pointsArray = points as (double X, double Y)[] ?? points.ToArray();
            Points = pointsArray.Select(p => new CustomPoint(p.X, p.Y)).ToList();

            for (var i = 0; i < pointsArray.Length; i++)
            {
                var p1 = new CustomPoint(pointsArray[i].X,
                    pointsArray[i].Y);
                var p2 = new CustomPoint(pointsArray[(i + 1) % pointsArray.Length].X,
                    pointsArray[(i + 1) % pointsArray.Length].Y);

                Lines.Add(new CustomLine(p1, p2));
            }
        }

        public IEnumerable<CustomPoint> IntersectWith(CustomPolygon polygon)
        {
            return Lines.SelectMany(line => line.IntersectWith(polygon)).Distinct().Except(Points);
        }

        public bool HasPointInside(CustomPoint point)
        {
            var result = false;

            double minX = Points.Select(p => p.X).Min();
            double maxX = Points.Select(p => p.X).Max();
            double minY = Points.Select(p => p.Y).Min();
            double maxY = Points.Select(p => p.Y).Max();

            if (point.X < minX || point.X > maxX || point.Y < minY || point.Y > maxY)
            {
                return false;
            }

            var j = Points.Count - 1;

            for (var i = 0; i < Points.Count; j = i++)
            {
                if (Points[i].Y > point.Y != Points[j].Y > point.Y &&
                    point.X < (Points[j].X - Points[i].X) *
                        (point.Y - Points[i].Y) / (Points[j].Y - Points[i].Y) + Points[i].X)
                {
                    result = !result;
                }
            }

            return result;
        }

        public void AddVector(double v1, double v2)
        {
            Points = Points.Select(p => p.AddVector(v1, v2)).ToList();
        }
    }
}