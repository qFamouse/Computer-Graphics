using System.IO;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ComputerGraphics.Models;
using ComputerGraphics.Utils;
using ComputerGraphics.Utils.Images.Bitmap;
using Microsoft.Win32;

namespace ComputerGraphics.ViewModel
{
    internal class OpenSaveViewModel : BaseViewModel
    {
        public Canvas Canvas => MainCanvas.GetInstance().Canvas;

        public RelayCommand SaveCommand
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.FileName = "Image"; // Default file name
                    saveFileDialog.DefaultExt = ".bmp"; // Default file extension
                    saveFileDialog.Filter = "Bitmap Image (.bmp)|*.bmp"; // Filter files by extension

                    bool? result = saveFileDialog.ShowDialog();

                    if (result == true)
                    {
                        using (FileStream fileStream = new FileStream(saveFileDialog.FileName, FileMode.Create, FileAccess.Write))
                        {
                            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap(
                                (int)Canvas.RenderSize.Width,
                                (int)Canvas.RenderSize.Height,
                                96d, 96d,
                                PixelFormats.Pbgra32);

                            renderTargetBitmap.Render(Canvas);

                            BitmapWriter bitmapWriter = new BitmapWriter(renderTargetBitmap);
                            bitmapWriter.Write(fileStream);
                        }
                    }
                });
            }
        }

        public RelayCommand OpenCommand
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    openFileDialog.FileName = "Image"; // Default file name
                    openFileDialog.DefaultExt = ".bmp"; // Default file extension
                    openFileDialog.Filter = "Bitmap Image (.bmp)|*.bmp"; // Filter files by extension

                    if (openFileDialog.ShowDialog() == true)
                    {
                        using (FileStream fileStream = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read))
                        {
                            var bitmapReader = new BitmapReader(fileStream);

                            Canvas.Children.Clear();
                            Canvas.Width = bitmapReader.Width;
                            Canvas.Height = bitmapReader.Height;
                            Canvas.Children.Add(bitmapReader.Image);
                        }
                    }
                });
            }
        }
    }
}