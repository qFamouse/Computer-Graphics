using ComputerGraphics.Core.Algorithms.Clipping.CohenSutherland;
using ComputerGraphics.UI.Models;
using ComputerGraphics.UI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ComputerGraphics.UI.ViewModel
{
    internal class ClippingViewModel : BaseViewModel
    {
        public Canvas Canvas => MainCanvas.GetInstance().Canvas;

        private List<(int x1, int y1, int x2, int y2)> _defaultPictureLines;
        public RelayCommand SpacePreparingCommand
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    Canvas.Children.Clear();
                    WriteableBitmap writeableBitmap = BitmapFactory.New(400, 400);
                    var image = new Image
                    {
                        Source = writeableBitmap
                    };

                    _defaultPictureLines = new List<(int x1, int y1, int x2, int y2)>()
                    {
                        // F
                        (130, 60, 130, 350), // |
                        (131, 60, 131, 350),
                        (132, 60, 132, 350),
                        (130, 60, 325, 60), // --
                        (130, 61, 325, 61),
                        (130, 62, 325, 62),
                        (130, 180, 250, 180), // -
                        (130, 181, 250, 181),
                        (130, 182, 250, 182),
                        // e
                        (200, 270, 260, 270), // -
                        (200, 271, 260, 271),
                        (200, 272, 260, 272),
                        (200, 270, 200, 350), // ||
                        (201, 270, 201, 350),
                        (202, 270, 202, 350),
                        (260, 270, 260, 312), // |
                        (261, 270, 261, 312),
                        (262, 270, 262, 312),
                        (260, 310, 200, 310), // - 
                        (260, 311, 200, 311),
                        (260, 312, 200, 312),
                        (200, 350, 290, 350), // --
                        (200, 351, 290, 351),
                        (200, 352, 290, 352),
                    };

                    foreach (var line in _defaultPictureLines)
                    {
                        writeableBitmap.DrawLine(line.x1, line.y1, line.x2, line.y2, Colors.Blue);
                    }

                    Canvas.Children.Add(image);

                    image.MouseLeftButtonDown += Image_MouseLeftButtonDown;
                    image.MouseLeftButtonUp += Image_MouseLeftButtonUp;
                    image.MouseLeave += Image_MouseLeave;

                });
            }
        }

        private void Image_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var image = sender as Image;
            var writeableBitmap = image.Source as WriteableBitmap;

            image.MouseMove -= Image_MouseMove;
        }

        private (int x1, int y1, int x2, int y2) _xRayMonitor;

        private void Image_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var image = sender as Image;
            var writeableBitmap = image.Source as WriteableBitmap;

            image.MouseMove += Image_MouseMove;

            Point point = e.GetPosition(image);
            _xRayMonitor.x1 = (int)point.X;
            _xRayMonitor.y1 = (int)point.Y;
        }

        private void Image_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var image = sender as Image;
            var writeableBitmap = image.Source as WriteableBitmap;

            image.MouseMove -= Image_MouseMove;
        }

        private void Image_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var image = sender as Image;
            var writeableBitmap = image.Source as WriteableBitmap;

            writeableBitmap.Clear();

            Point point = e.GetPosition(image);


            if (_xRayMonitor.y1 > point.Y)
            {
                _xRayMonitor.y2 = _xRayMonitor.y1;
                _xRayMonitor.y1 = (int)point.Y;
            }
            else
            {
                _xRayMonitor.y2 = (int)point.Y;
            }

            if (_xRayMonitor.x1 > point.X)
            {
                _xRayMonitor.x2 = _xRayMonitor.x1;
                _xRayMonitor.x1 = (int)point.X;
            }
            else
            {
                _xRayMonitor.x2 = (int)point.X;
            }

            writeableBitmap.Clear();

            writeableBitmap.DrawRectangle(_xRayMonitor.x1, _xRayMonitor.y1, _xRayMonitor.x2, _xRayMonitor.y2, Colors.Black);
            var a = XRayShow();



            foreach (var line in a)
            {
                writeableBitmap.DrawLine(line.x1, line.y1, line.x2, line.y2, Colors.Blue);
            }
        }

        private List<(int x1, int y1, int x2, int y2)> XRayShow()
        {
            CohenSutherland.Clip(_defaultPictureLines, (_xRayMonitor.x1, _xRayMonitor.x2, _xRayMonitor.y1, _xRayMonitor.y2), out List<(int x1, int y1, int x2, int y2)> clippedLines);

            return clippedLines;
        }

        public ClippingViewModel()
        {
            return;

            Canvas.Children.Clear();
            var dimension = 400;
            Canvas.Width = Canvas.Height = dimension;

            WriteableBitmap writeableBitmap = BitmapFactory.New(dimension, dimension);
            var image = new Image
            {
                Source = writeableBitmap
            };
            Canvas.Children.Add(image);

            int[] F = { 130, 60, 130, 350, 131, 60, 131, 350, 132, 60, 132, 350, 130, 60, 325, 60, 130, 61, 325, 61 };

            writeableBitmap.DrawPolyline(F, Colors.Blue);

            return;

            // F
            writeableBitmap.DrawLine(130, 60, 130, 350, Colors.Blue); // |
            writeableBitmap.DrawLine(131, 60, 131, 350, Colors.Blue);
            writeableBitmap.DrawLine(132, 60, 132, 350, Colors.Blue);

            writeableBitmap.DrawLine(130, 60, 325, 60, Colors.Blue); // --
            writeableBitmap.DrawLine(130, 61, 325, 61, Colors.Blue);
            writeableBitmap.DrawLine(130, 62, 325, 62, Colors.Blue);

            writeableBitmap.DrawLine(130, 180, 250, 180, Colors.Blue); // -
            writeableBitmap.DrawLine(130, 181, 250, 181, Colors.Blue);
            writeableBitmap.DrawLine(130, 182, 250, 182, Colors.Blue);


            // e
            writeableBitmap.DrawLine(200, 270, 260, 270, Colors.Blue); // -
            writeableBitmap.DrawLine(200, 271, 260, 271, Colors.Blue);
            writeableBitmap.DrawLine(200, 272, 260, 272, Colors.Blue);

            writeableBitmap.DrawLine(200, 270, 200, 350, Colors.Blue); // ||
            writeableBitmap.DrawLine(201, 270, 201, 350, Colors.Blue);
            writeableBitmap.DrawLine(202, 270, 202, 350, Colors.Blue);

            writeableBitmap.DrawLine(260, 270, 260, 312, Colors.Blue); // |
            writeableBitmap.DrawLine(261, 270, 261, 312, Colors.Blue);
            writeableBitmap.DrawLine(262, 270, 262, 312, Colors.Blue);

            writeableBitmap.DrawLine(260, 310, 200, 310, Colors.Blue); // -
            writeableBitmap.DrawLine(260, 311, 200, 311, Colors.Blue);
            writeableBitmap.DrawLine(260, 312, 200, 312, Colors.Blue);

            writeableBitmap.DrawLine(200, 350, 290, 350, Colors.Blue); // --
            writeableBitmap.DrawLine(200, 351, 290, 351, Colors.Blue);
            writeableBitmap.DrawLine(200, 352, 290, 352, Colors.Blue);



            return;

            (int x1, int y1, int x2, int y2) rect = (100, 100, 200, 200);
            writeableBitmap.DrawRectangle(rect.x1, rect.y1, rect.x2, rect.y2, Colors.Black);

            (int x1, int y1, int x2, int y2) line1 = (90, 220, 220, 140);
            writeableBitmap.DrawLine(line1.x1, line1.y1, line1.x2, line1.y2, Colors.Red);

            (int x1, int y1, int x2, int y2) line2 = (120, 80, 220, 140);
            writeableBitmap.DrawLine(line2.x1, line2.y1, line2.x2, line2.y2, Colors.Red);

            (int x1, int y1, int x2, int y2) line3 = (90, 220, 120, 80);
            writeableBitmap.DrawLine(line3.x1, line3.y1, line3.x2, line3.y2, Colors.Red);

            (int x1, int y1, int x2, int y2) line4 = (50, 50, 50, 200);
            writeableBitmap.DrawLine(line4.x1, line4.y1, line4.x2, line4.y2, Colors.Black);

            var lines = new List<(int x1, int y1, int x2, int y2)>() { line1, line2, line3 };



            int count = CohenSutherland.Clip(lines, (rect.x1, rect.x2, rect.y1, rect.y2), out List<(int x1, int y1, int x2, int y2)> clippedLines);

            writeableBitmap.Clear();
            writeableBitmap.DrawRectangle(rect.x1, rect.y1, rect.x2, rect.y2, Colors.Black);

            foreach (var line in clippedLines)
            {
                writeableBitmap.DrawLine(line.x1, line.y1, line.x2, line.y2, Colors.Red);
            }

            //var status = CohenSutherland.Clip(line, (rect.x1, rect.x2, rect.y1, rect.y2), out (int x1, int y1, int x2, int y2) clip);

            //writeableBitmap.Clear();
            //writeableBitmap.DrawRectangle(rect.x1, rect.y1, rect.x2, rect.y2, Colors.Blue);
            //writeableBitmap.DrawLine(clip.x1, clip.y1, clip.x2, clip.y2, Colors.Red);


            //var space = 100;

            //(int x1, int y1, int x2, int y2) rect = (100, 100, 200, 200);
            //writeableBitmap.DrawRectangle(rect.x1, rect.y1, rect.x2, rect.y2, Colors.Blue);

            //RectangleGeometry a;

            //(int x1, int y1, int x2, int y2) line = (90, 220, 220, 140);
            //writeableBitmap.DrawLine(line.x1, line.y1, line.x2, line.y2, Colors.Red);

            //writeableBitmap.FillEllipseCentered(line.x1, line.y1, 2, 2, Colors.Violet);
            //writeableBitmap.FillEllipseCentered(line.x2, line.y2, 2, 2, Colors.Violet);

            //bCode lineA = GetBcode(line.x1, line.y1, (rect.x1, rect.x2, rect.y1, rect.y2));
            //bCode lineB = GetBcode(line.x2, line.y2, (rect.x1, rect.x2, rect.y1, rect.y2));

            //bCode xor = lineA ^ lineB;


            ////Intersection((3, 6, 6, 2), (5, 1, 5, 5));
            ////Intersection((1, 1, 6, 6), (1, 4, 4, 1));
            //var inter = Intersection((rect.x1, rect.y2, rect.x2, rect.y2), line);
            //writeableBitmap.FillEllipseCentered((int)inter.x, (int)inter.y, 2, 2, Colors.Brown);

            //// NEW
            //writeableBitmap.Clear();

            //writeableBitmap.DrawPolyline(new int[] { 10, 5, 20, 40, 30, 30, 7, 8, 10, 5 }, Colors.Green);

            //return;

            //writeableBitmap.DrawRectangle(rect.x1, rect.y1, rect.x2, rect.y2, Colors.Blue);

            //line = ((int)inter.x, (int)inter.y, 220, 140);
            //writeableBitmap.DrawLine(line.x1, line.y1, line.x2, line.y2, Colors.Red);

            //lineA = GetBcode(line.x1, line.y1, (rect.x1, rect.x2, rect.y1, rect.y2));
            //lineB = GetBcode(line.x2, line.y2, (rect.x1, rect.x2, rect.y1, rect.y2));

            //xor = lineA ^ lineB;

        }


    }
}