using System.Linq;
using ComputerGraphics.Core.Algorithms.Rasterization.Primitives;
using MathNet.Numerics.LinearAlgebra.Double;

namespace ComputerGraphics.Core.Algorithms.AffineTransformations
{
    public static class AffineTransformationsApplier
    {
        public static CustomPolygon Apply(CustomPolygon polygon, params IAffineTransformation[] transformations)
        {
            var transformation = transformations.Select(trans => trans.GetTransformation())
                .Aggregate(Matrix.Build.DenseDiagonal(3, 3, 1), (current, m) => current.Multiply(m));
            return new CustomPolygon(polygon.Points.Select(p =>
            {
                var vector = Matrix.Build.DenseOfRows(new[] { new[] { p.X, p.Y, 1 } });
                var point = vector.Multiply(transformation);
                return new CustomPoint(point[0, 0], point[0, 1]);
            }));
        }
    }
}