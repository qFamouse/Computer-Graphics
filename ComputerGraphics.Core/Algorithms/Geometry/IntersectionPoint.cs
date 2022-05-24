using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerGraphics.Core.Algorithms.Geometry
{
    public static class IntersectionPoint
    {
        public static bool Find(
            (int x1, int y1, int x2, int y2) line1, 
            (int x1, int y1, int x2, int y2) line2,
            out (float x, float y) intersectionPoint)
        {
            float p0_x = line1.x1;
            float p0_y = line1.y1;
            float p1_x = line1.x2;
            float p1_y = line1.y2;
            float p2_x = line2.x1;
            float p2_y = line2.y1;
            float p3_x = line2.x2;
            float p3_y = line2.y2;

            float s1_x, s1_y, s2_x, s2_y;
            s1_x = p1_x - p0_x;
            s1_y = p1_y - p0_y;
            s2_x = p3_x - p2_x;
            s2_y = p3_y - p2_y;

            float s, t;
            float d = -s2_x * s1_y + s1_x * s2_y;
            s = (-s1_y * (p0_x - p2_x) + s1_x * (p0_y - p2_y)) / d;
            t = (s2_x * (p0_y - p2_y) - s2_y * (p0_x - p2_x)) / d;

            if (s >= 0 && s <= 1 && t >= 0 && t <= 1)
            { // Collision detected
                float x = (p0_x + (t * s1_x));
                float y = ((p0_y + (t * s1_y)));

                intersectionPoint = (x, y);
                return true;
            }

            intersectionPoint = default;
            return false;
        }
    }
}