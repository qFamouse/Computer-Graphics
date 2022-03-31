using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerGraphics.UI.Utils.Rasterization.Primitives.Ellipse
{
    internal static class CircleBresenham
    {
        /// <summary>
        /// Bresenham’s circle drawing algorithm
        /// source: https://ru.wikipedia.org/wiki/Алгоритм_Брезенхэма
        /// </summary>
        /// <param name="R">Radius</param>
        /// <param name="X1">Center of the circle by x</param>
        /// <param name="Y1">Center of the circle by y</param>
        /// <param name="drawpixel">Action where pixels will be sent to</param>
        public static void Draw(int R, int X1, int Y1, Action<int, int> drawpixel)
        {
            int x = 0;
            int y = R;
            int delta = 1 - 2 * R;

            while (y >= x)
            {
                drawpixel(X1 + x, Y1 + y);
                drawpixel(X1 + x, Y1 - y);
                drawpixel(X1 - x, Y1 + y);
                drawpixel(X1 - x, Y1 - y);
                drawpixel(X1 + y, Y1 + x);
                drawpixel(X1 + y, Y1 - x);
                drawpixel(X1 - y, Y1 + x);
                drawpixel(X1 - y, Y1 - x);

                int error = 2 * (delta + y) - 1;

                if ((delta < 0) && (error <= 0))
                {
                    delta += 2 * ++x + 1;
                    continue;
                }
                if ((delta > 0) && (error > 0))
                {
                    delta -= 2 * --y + 1;
                    continue;
                }
                delta += 2 * (++x - --y);
            }
        }
    }
}