using ComputerGraphics.Core.Algorithms.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerGraphics.Core.Algorithms.Clipping.CohenSutherland
{
    public struct CohenSutherlandCode
    {
        private const byte INSIDE = 0; // 0000
        private const byte LEFT   = 1; // 0001
        private const byte BOTTOM = 2; // 0010
        private const byte RIGHT  = 4; // 0100
        private const byte TOP    = 8; // 1000

        public byte Code { get; private set; }

        public bool Inside => Code == INSIDE;
        public bool Left => (Code & LEFT) == LEFT;
        public bool Bottom => (Code & BOTTOM) == BOTTOM;
        public bool Right => (Code & RIGHT) == RIGHT;
        public bool Top => (Code & TOP) == TOP;

        public CohenSutherlandCode(
            (int x, int y) point,
            (int xMin, int xMax, int yMin, int yMax) border)
        {
            if ((border.xMin > border.xMax) || (border.yMin > border.yMax))
            {
                throw new ArgumentException("The minimum point turned out to be greater than the maximum");
            }

            Code = DefineCode(point, border);
        }

        public CohenSutherlandCode(byte code) => Code = code;

        /// <summary>
        /// Compute the bit code for a 'point' bounded diagonally by 'border'
        /// </summary>
        /// <param name="point">Investigated point</param>
        /// <param name="border">The boundary relative to which the investigation takes place</param>
        /// <returns>A byte whose bits are filled with status information</returns>
        /// <exception cref="Exception"></exception>
        public static byte DefineCode(
            (int x, int y) point,
            (int xMin, int xMax, int yMin, int yMax) border)
        {
            byte code = INSIDE;

            code |= point.x < border.xMin ? LEFT : INSIDE; // if the point lies to the left of the border
            code |= point.y > border.yMax ? BOTTOM : INSIDE; // below the border
            code |= point.x > border.xMax ? RIGHT : INSIDE; // right of the border
            code |= point.y < border.yMin ? TOP : INSIDE; // above the border

            return code;
        }

        public static bool operator == (CohenSutherlandCode A, CohenSutherlandCode B)
        {
            return A.Code == B.Code;
        }

        public static bool operator !=(CohenSutherlandCode A, CohenSutherlandCode B)
        {
            return A.Code != B.Code;
        }

        public static CohenSutherlandCode operator &(CohenSutherlandCode A, CohenSutherlandCode B)
        {
            return new CohenSutherlandCode((byte)(A.Code & B.Code));
        }

        public static CohenSutherlandCode operator ^(CohenSutherlandCode A, CohenSutherlandCode B)
        {
            return new CohenSutherlandCode((byte)(A.Code ^ B.Code));
        }
    }

    public static class CohenSutherland
    {
        public static bool Clip(
            (int x1, int y1, int x2, int y2) line,
            (int xMin, int xMax, int yMin, int yMax) border,
            out (int x1, int y1, int x2, int y2) clippedLine)
        {
            var pointCodeA = new CohenSutherlandCode((line.x1, line.y1), border);
            var pointCodeB = new CohenSutherlandCode((line.x2, line.y2), border);

            if ((pointCodeA == pointCodeB) && pointCodeA.Inside)
            { // Both points are inside the border
                clippedLine = line;
                return true;
            }
            else if ((pointCodeA & pointCodeB).Inside == false)
            { // Both points are not inside the border
                clippedLine = line;
                return false;
            }
            else
            { // Clipping is necessary
                CohenSutherlandCode clippingCode = pointCodeA ^ pointCodeB;

                var leftBorderLine = (border.xMin, border.yMin, border.xMin, border.yMax);
                var rightBorderLine = (border.xMax, border.yMin, border.xMax, border.yMax);
                var bottomBorderLine = (border.xMin, border.yMax, border.xMax, border.yMax);
                var topBorderLine = (border.xMin, border.yMin, border.xMax, border.yMin);

                (float x, float y) intersectionPoint;
                clippedLine = line;

                if (clippingCode.Left && IntersectionPoint.Find(line, leftBorderLine, out intersectionPoint))
                { // If clipping on the left is necessary 
                    _ = line.x1 < border.xMin ? // swapping points
                        (clippedLine.x1, clippedLine.y1) = ((int)intersectionPoint.x, (int)intersectionPoint.y) :
                        (clippedLine.x2, clippedLine.y2) = ((int)intersectionPoint.x, (int)intersectionPoint.y);
                }
                if (clippingCode.Bottom && IntersectionPoint.Find(line, bottomBorderLine, out intersectionPoint))
                { // If clipping on the bottom is necessary 
                    _ = line.y1 > border.yMax ?
                        (clippedLine.x1, clippedLine.y1) = ((int)intersectionPoint.x, (int)intersectionPoint.y) :
                        (clippedLine.x2, clippedLine.y2) = ((int)intersectionPoint.x, (int)intersectionPoint.y);
                }
                if (clippingCode.Right && IntersectionPoint.Find(line, rightBorderLine, out intersectionPoint))
                { // If clipping on the bottom is necessary 
                    _ = line.x1 > border.xMax ?
                        (clippedLine.x1, clippedLine.y1) = ((int)intersectionPoint.x, (int)intersectionPoint.y) :
                        (clippedLine.x2, clippedLine.y2) = ((int)intersectionPoint.x, (int)intersectionPoint.y);
                }
                if (clippingCode.Top && IntersectionPoint.Find(line, topBorderLine, out intersectionPoint))
                { // If clipping on the bottom is necessary 
                    _ = line.y1 < border.yMin ?
                        (clippedLine.x1, clippedLine.y1) = ((int)intersectionPoint.x, (int)intersectionPoint.y) :
                        (clippedLine.x2, clippedLine.y2) = ((int)intersectionPoint.x, (int)intersectionPoint.y);
                }

                return Clip(clippedLine, border, out clippedLine);
            }
        }

        public static int Clip(
            List<(int x1, int y1, int x2, int y2)> lines,
            (int xMin, int xMax, int yMin, int yMax) border,
            out List<(int x1, int y1, int x2, int y2)> clippedLines)
        {
            clippedLines = new List<(int x1, int y1, int x2, int y2)>();

            foreach (var line in lines)
            {
                if (Clip(line, border, out (int x1, int y1, int x2, int y2) clippedLine))
                {
                    clippedLines.Add(clippedLine);
                }
            }

            return clippedLines.Count;
        }
    }
}