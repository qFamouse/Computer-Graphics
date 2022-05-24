using System;

namespace ComputerGraphics.Core.Algorithms.Rasterization.Primitives
{
    public struct CustomPoint
    {
        public double X { get; set; }
        public double Y { get; set; }

        public CustomPoint(double x, double y)
        {
            X = x;
            Y = y;
        }

        public CustomPoint AddVector(double v1, double v2)
        {
            return new CustomPoint(X + v1, Y + v2);
        }

        public override bool Equals(object obj)
        {
            if (obj is CustomPoint point)
            {
                return Math.Abs(X - point.X) < 0.001 && Math.Abs(Y - point.Y) < 0.001;
            }
            return base.Equals(obj);
        }
    }
}