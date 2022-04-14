using ComputerGraphics.Core.Algorithms.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerGraphics.Core.Algorithms.Clipping.CohenSutherland
{
    public static class CohenSutherland
    {
        /// <summary>
        /// Method is clipping the line at the border (This algorithm provides only rectangle border)
        /// </summary>
        /// <param name="line">A line tuple consists of two points that build a line</param>
        /// <param name="border">Tuple consists min/max coordinates of rectangle</param>
        /// <param name="clippedLine">out: Returns tuple withs result of clipping (if TRUE)</param>
        /// <returns>Returns true if 'clippedLine' is in the border. False if there is no line within the border</returns>
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

                return clippedLine == line ? false : Clip(clippedLine, border, out clippedLine);
            }
        }

        /// <summary>
        /// Method is clipping the list of lines at the border (This algorithm provides only rectangle border)
        /// </summary>
        /// <param name="lines">List of lines tuple for buiding lines</param>
        /// <param name="border">Tuple consists min/max coordinates of rectangle</param>
        /// <param name="clippedLines">out: Returns list of clipped lines</param>
        /// <returns>Returns count of clipped lines</returns>
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