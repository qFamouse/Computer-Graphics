using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ComputerGraphics.Utils.Rasterization.Primitives.Line
{
    internal static class BresenhamsLineAlgorithm
    {
        public static (int x, int y)[] GetPixels((double x, double y) start, (double x, double y) end)
        {
            return GetPixels(((int)start.x, (int)start.y), ((int)end.x, (int)end.y));
        }
        /// <summary>
        /// Bresenham's line algorithm. 
        /// Line drawing algorithm that determines the points of an n-dimensional raster that should be 
        /// selected in order to form a close approximation to a straight line between two points.
        /// source: https://en.wikipedia.org/wiki/Bresenham%27s_line_algorithm
        /// </summary>
        /// <param name="start">Starting point of the line</param>
        /// <param name="end">Point of the end of the line</param>
        /// <returns>Array of pixel coordinates for building a line</returns>
        public static (int x, int y)[] GetPixels((int x, int y) start, (int x, int y) end)
        {
            int dx = Math.Abs(end.x - start.x);
            int sx = start.x < end.x ? 1 : -1;

            var dy = -Math.Abs(end.y - start.y);
            var sy = start.y < end.y ? 1 : -1;

            var error = dx + dy;

            var pixels = new LinkedList<(int x, int y)>();
            int x = start.x, y = start.y;

            while (true)
            {
                pixels.AddLast((x, y));

                if (x == end.x && y == end.y)
                {
                    break;
                }

                var e2 = 2 * error;

                if (e2 >= dy)
                {
                    if (x == end.x)
                    {
                        break;
                    }

                    error = error + dy;
                    x += sx;
                }

                if (e2 <= dx)
                {
                    if (y == end.y)
                    {
                        break;
                    }

                    error += + dx;
                    y += sy;
                }
            }

            return pixels.ToArray();
        }
    }
}
