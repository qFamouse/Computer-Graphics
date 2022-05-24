using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace ComputerGraphics.Core.Algorithms.AffineTransformations
{
    public class AffineDilatation : IAffineTransformation
    {
        private readonly double _a;
        private readonly double _b;

        public AffineDilatation(double a, double b)
        {
            _a = a;
            _b = b;
        }
        public Matrix<double> GetTransformation()
        {
            return Matrix.Build.DenseOfArray(new[,]
            {
                {_a, 0, 0},
                {0, _b, 0},
                {0, 0, 1}
            });
        }
    }
}