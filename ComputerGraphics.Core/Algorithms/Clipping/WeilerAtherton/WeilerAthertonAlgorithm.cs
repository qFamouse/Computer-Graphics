using ComputerGraphics.Core.Algorithms.Rasterization.Primitives;
using System.Collections.Generic;
using System.Linq;

namespace ComputerGraphics.Core.Algorithms.Clipping.WeilerAtherton
{
    public class WeilerAthertonAlgorithm
    {
        public static IEnumerable<CustomLine> Clip(CustomPolygon polygon, CustomPolygon clipPolygon)
        {
            var intersectionPoints = polygon.IntersectWith(clipPolygon).ToList();

            if (!intersectionPoints.Any())
            {
                if (clipPolygon.HasPointInside(polygon.Points[0]))
                {
                    return polygon.Lines;
                }

                return null;
            }

            var polygonPoints = IntersectionEnrich(polygon, intersectionPoints);
            var borderPoints = IntersectionEnrich(clipPolygon, intersectionPoints);

            var inOut = InOutIntersection(intersectionPoints, polygon, clipPolygon);

            return GetClippingPolygon(polygonPoints, inOut);
        }

        private static IList<CustomPoint> IntersectionEnrich(CustomPolygon polygon, IEnumerable<CustomPoint> intersection)
        {
            IList<CustomPoint> points = new List<CustomPoint>(polygon.Points);

            var intersectionArray = intersection as CustomPoint[] ?? intersection.ToArray();
            for (var i = 0; i < points.Count; i++)
            {
                var line = new CustomLine(points[i], points[(i + 1) % points.Count]);
                foreach (var point in intersectionArray.Where(point => line.HasPointOnLine(point)))
                {
                    points.Insert(++i, point);
                }
            }

            return points;
        }

        private static IList<(CustomPoint, bool)> InOutIntersection(List<CustomPoint> intersection, CustomPolygon polygon,
            CustomPolygon clipPolygon)
        {
            var isOutside = !clipPolygon.HasPointInside(polygon.Points[0]);
            var result = new List<(CustomPoint, bool)>(intersection.Count);
            foreach (var point in intersection)
            {
                result.Add((point, isOutside));
                isOutside = !isOutside;
            }

            return result;
        }

        private static IEnumerable<CustomLine> GetClippingPolygon(IList<CustomPoint> enrichPolygon,
            IList<(CustomPoint, bool)> inOutIntersection)
        {
            var clipping = new List<CustomLine>();
            var points = enrichPolygon;

            var intersectIndex = inOutIntersection.IndexOf(inOutIntersection
                .First(p => p.Item2));
            var start = inOutIntersection[intersectIndex].Item1;

            var index = points.IndexOf(start);
            var current = start;
            CustomPoint next;
            do
            {
                index = (index + 1) % points.Count;
                next = points[index];
                clipping.Add(new CustomLine(current, next));
                current = next;
                if (IsIntersection(next, inOutIntersection))
                {
                    intersectIndex = (intersectIndex + 2) % inOutIntersection.Count;
                    current = inOutIntersection[intersectIndex].Item1;
                    next = current;
                    index = points.IndexOf(current);
                }
            } while (!start.Equals(next));

            return clipping;
        }

        private static bool IsIntersection(int index, IList<CustomPoint> points,
            IEnumerable<(CustomPoint, bool)> inOutIntersection)
        {
            var point = points[index];
            return inOutIntersection
                .ToList().Exists(pair => pair.Item1.Equals(point));
        }

        private static bool IsIntersection(CustomPoint point,
            IEnumerable<(CustomPoint, bool)> inOutIntersection)
        {
            return inOutIntersection
                .ToList().Exists(pair => pair.Item1.Equals(point));
        }
    }
}