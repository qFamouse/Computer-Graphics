using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ComputerGraphics.Core.Algorithms.Rasterization.Primitives;

namespace ComputerGraphics.Core.Algorithms.Rasterization.RasterisationAlgorithms
{
    public class BresenhamAlgorithm : IRasterisationAlgorithm
    {
        // sourse: https://grafika.me/node/9
        public void DrawLine(WriteableBitmap bitmap, CustomLine line, Color color)
        {
            var x0 = (int)line.P1.X;
            var y0 = (int)line.P1.Y;
            var x1 = (int)line.P2.X;
            var y1 = (int)line.P2.Y;

            //Изменения координат
            int dx = (x1 > x0) ? (x1 - x0) : (x0 - x1);
            int dy = (y1 > y0) ? (y1 - y0) : (y0 - y1);
            //Направление приращения
            int sx = (x1 >= x0) ? (1) : (-1);
            int sy = (y1 >= y0) ? (1) : (-1);

            if (dy < dx)
            {
                int d = (dy << 1) - dx;
                int d1 = dy << 1;
                int d2 = (dy - dx) << 1;
                bitmap.SetPixel(x0, y0, color);
                int x = x0 + sx;
                int y = y0;
                for (int i = 1; i <= dx; i++)
                {
                    if (d > 0)
                    {
                        d += d2;
                        y += sy;
                    }
                    else
                        d += d1;
                    bitmap.SetPixel(x, y, color);
                    x += sx;
                }
            }
            else
            {
                int d = (dx << 1) - dy;
                int d1 = dx << 1;
                int d2 = (dx - dy) << 1;
                bitmap.SetPixel(x0, y0, color);
                int x = x0;
                int y = y0 + sy;
                for (int i = 1; i <= dy; i++)
                {
                    if (d > 0)
                    {
                        d += d2;
                        x += sx;
                    }
                    else
                        d += d1;
                    bitmap.SetPixel(x, y, color);
                    y += sy;
                }
            }
        }
    }
}