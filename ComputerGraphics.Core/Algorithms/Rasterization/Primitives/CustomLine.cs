using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;

namespace ComputerGraphics.Core.Algorithms.Rasterization.Primitives
{
    public struct CustomLine
    {
        public CustomPoint P1 { get; set; }
        public CustomPoint P2 { get; set; }

        public CustomLine(CustomPoint p1, CustomPoint p2)
        {
            P1 = p1;
            P2 = p2;
        }

        public IEnumerable<CustomPoint> IntersectWith(CustomPolygon polygon)
        {
            var thisLine = this;

            var x1 = P1.X;
            var y1 = P1.Y;
            var x2 = P2.X;
            var y2 = P2.Y;

            var points = polygon.Lines
                .Select(line => line.IntersectWith(thisLine))
                .Where(p => p.HasValue)
                .Select(p => p.Value)
                .OrderBy(point =>
                {
                    if (!(Math.Abs(x1 - x2) < 0.0001)) return (point.X - x1) / (x2 - x1);

                    if (Math.Abs(y1 - y2) < 0.0001)
                    {
                        return 0;
                    }

                    return (point.Y - y1) / (y2 - y1);
                });
            return points;
        }

        public CustomPoint? IntersectWith(CustomLine line)
        {
            return IntersectWith(line.P1, line.P2);
        }

        private CustomPoint? IntersectWith(CustomPoint p1, CustomPoint p2)
        {
            var x1 = P1.X;
            var y1 = P1.Y;
            var x2 = P2.X;
            var y2 = P2.Y;

            var x3 = p1.X;
            var y3 = p1.Y;
            var x4 = p2.X;
            var y4 = p2.Y;

            var s1_x = x2 - x1;
            var s1_y = y2 - y1;
            var s2_x = x4 - x3;
            var s2_y = y4 - y3;

            double s, t;
            double d = -s2_x * s1_y + s1_x * s2_y;

            s = (-s1_y * (x1 - x3) + s1_x * (y1 - y3)) / d;
            t = (s2_x * (y1 - y3) - s2_y * (x1 - x3)) / d;

            if (!(s >= 0) || !(s <= 1) || !(t >= 0) || !(t <= 1)) return null;

            // Collision detected
            var x = x1 + t * s1_x;
            var y = y1 + t * s1_y;
            return new CustomPoint(x, y);
        }

        public bool HasPointOnLine(CustomPoint p)
        {
            var a = P1;
            var b = P2;

            var crossProduct = (p.Y - a.Y) * (b.X - a.X)
                                 - (p.X - a.X) * (b.Y - a.Y);
            if (Math.Abs(crossProduct) > 0.1)
            {
                return false;
            }

            var dotProduct = (p.X - a.X) * (b.X - a.X)
                                + (p.Y - a.Y) * (b.Y - a.Y);
            if (Math.Abs(dotProduct) < 0.1)
            {
                return false;
            }

            var squaredLengthBa = (b.X - a.X) * (b.X - a.X)
                                     + (b.Y - a.Y) * (b.Y - a.Y);
            return !(dotProduct > squaredLengthBa);
        }
    }
}