using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace ComputerGraphics.Core.Algorithms.AffineTransformations
{
    public interface IAffineTransformation
    {
        Matrix<double> GetTransformation();
    }
}