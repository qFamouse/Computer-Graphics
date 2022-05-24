using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace ComputerGraphics.Core.Algorithms.AffineTransformations
{
    public class AffineTranslation : IAffineTransformation
    {
        private readonly double _v1;
        private readonly double _v2;

        public AffineTranslation(double v1, double v2)
        {
            _v1 = v1;
            _v2 = v2;
        }
        public Matrix<double> GetTransformation()
        {
            return Matrix.Build.DenseOfArray(new[,]
            {
                {1, 0, 0},
                {0, 1, 0},
                {_v1, _v2, 1}
            });
        }
    }
}