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
        private const byte LEFT = 1; // 0001
        private const byte BOTTOM = 2; // 0010
        private const byte RIGHT = 4; // 0100
        private const byte TOP = 8; // 1000

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

        public static bool operator ==(CohenSutherlandCode A, CohenSutherlandCode B) => A.Equals(B);

        public static bool operator !=(CohenSutherlandCode A, CohenSutherlandCode B) => !A.Equals(B);

        public static CohenSutherlandCode operator &(CohenSutherlandCode A, CohenSutherlandCode B)
        {
            return new CohenSutherlandCode((byte)(A.Code & B.Code));
        }

        public static CohenSutherlandCode operator ^(CohenSutherlandCode A, CohenSutherlandCode B)
        {
            return new CohenSutherlandCode((byte)(A.Code ^ B.Code));
        }

        public override bool Equals(object obj)
        {
            return obj is CohenSutherlandCode c && Code == c.Code;
        }

        public override int GetHashCode()
        {
            return Code.GetHashCode();
        }
    }
}
