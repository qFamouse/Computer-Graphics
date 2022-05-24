using ComputerGraphics.Core.Algorithms.Rasterization.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ComputerGraphics.Core.Algorithms.Rasterization.RasterisationAlgorithms
{
    public interface IRasterisationAlgorithm
    {
        void DrawLine(WriteableBitmap bitmap, CustomLine line, Color color);
    }
}