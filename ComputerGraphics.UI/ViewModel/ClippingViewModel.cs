using ComputerGraphics.Core.Algorithms.Clipping.CohenSutherland;
using ComputerGraphics.Core.Entities;
using ComputerGraphics.UI.Models;
using ComputerGraphics.UI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ComputerGraphics.UI.ViewModel
{
    internal class ClippingViewModel : BaseViewModel
    {
        public Canvas Canvas => MainCanvas.GetInstance().Canvas;

        public WriteableBitmap ImageContent { get; private set; }

        public List<(int x1, int y1, int x2, int y2)> DrawingLines { get; private set; }

        #region List Box

        private CollectionView _tools;

        private ClippingTool _selectedTool;
        public CollectionView Tools => _tools;

        public ClippingTool SelectedTool
        {
            get { return _selectedTool; }
            set
            {
                _selectedTool = value;
                OnPropertyChanged(nameof(SelectedTool));
            }
        }

        public RelayCommand ItemButtonUpCommand
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    SelectedTool.Action.Invoke();
                });
            }
        }

        #endregion

        #region Tools

        private void DrawTool()
        {
            Image image = NewImage();

            (int x1, int y1, int x2, int y2) newLine = (0, 0, 0, 0);

            void StylusDown(Point point)
            {
                newLine.x1 = (int)point.X;
                newLine.y1 = (int)point.Y;
            }

            void StylusUp(Point point)
            {
                newLine.x2 = (int)point.X;
                newLine.y2 = (int)point.Y;

                DrawingLines.Add(newLine);
                ImageContent.DrawLine(newLine.x1, newLine.y1, newLine.x2, newLine.y2, Colors.Blue);
            }

            void ClearContent()
            {
                ImageContent.Clear();
                DrawingLines.Clear();
            }

            image.MouseLeftButtonDown += (object sender, System.Windows.Input.MouseButtonEventArgs e) => StylusDown(e.GetPosition(image));
            image.MouseLeftButtonUp += (object sender, System.Windows.Input.MouseButtonEventArgs e) => StylusUp(e.GetPosition(image));
            image.MouseRightButtonDown += (object sender, System.Windows.Input.MouseButtonEventArgs e) => ClearContent();
        }

        private void RectagleXRayTool()
        {
            Image image = NewImage();

            (int x1, int y1, int x2, int y2) xRayRectangle = (0, 0, 0, 0);

            void SaveUpperLeftPoint(Point point)
            {
                xRayRectangle.x1 = (int)point.X;
                xRayRectangle.y1 = (int)point.Y;
            }

            void SaveLowerRightPoint(Point point)
            {
                if (xRayRectangle.y1 > point.Y)
                {
                    xRayRectangle.y2 = xRayRectangle.y1;
                    xRayRectangle.y1 = (int)point.Y;
                }
                else
                {
                    xRayRectangle.y2 = (int)point.Y;
                }
                
                if (xRayRectangle.x1 > point.X)
                {
                    xRayRectangle.x2 = xRayRectangle.x1;
                    xRayRectangle.x1 = (int)point.X;
                }
                else
                {
                    xRayRectangle.x2 = (int)point.X;
                }
            }

            void PreviewXRayRectangle()
            {
                ImageContent.Clear();
                ImageContent.DrawRectangle(xRayRectangle.x1, xRayRectangle.y1, xRayRectangle.x2, xRayRectangle.y2, Colors.Black);


                if (CohenSutherland.Clip(DrawingLines, (xRayRectangle.x1, xRayRectangle.x2, xRayRectangle.y1, xRayRectangle.y2), out List<(int x1, int y1, int x2, int y2)> clippedLines) > 0)
                {
                    foreach (var line in clippedLines)
                    {
                        ImageContent.DrawLine(line.x1, line.y1, line.x2, line.y2, Colors.Blue);
                    }
                }
            }

            void MouseMove_PreviewXRayRectangle(object sender, System.Windows.Input.MouseEventArgs e) => PreviewXRayRectangle();
            void MouseMove_SaveLowerRightPoint(object sender, System.Windows.Input.MouseEventArgs e) => SaveLowerRightPoint(e.GetPosition(image));

            image.MouseLeftButtonDown += (s, e) => SaveUpperLeftPoint(e.GetPosition(image));
            image.MouseLeftButtonDown += (s, e) => image.MouseMove += MouseMove_SaveLowerRightPoint;
            image.MouseLeftButtonDown += (s, e) => image.MouseMove += MouseMove_PreviewXRayRectangle;

            image.MouseLeftButtonUp += (s, e) => image.MouseMove -= MouseMove_PreviewXRayRectangle;
            image.MouseLeftButtonUp += (s, e) => image.MouseMove -= MouseMove_SaveLowerRightPoint;
            image.MouseLeftButtonUp += (s, e) => PreviewXRayRectangle();

        }

        #endregion

        private Image NewImage()
        {
            Canvas.Children.Clear();

            Image image = new Image()
            {
                Source = ImageContent
            };

            Canvas.Children.Add(image);

            return image;
        }

        public ClippingViewModel()
        {
            var tools = new List<ClippingTool>()
            {
                new ClippingTool()
                {
                    Name = "Draw",
                    Action = DrawTool
                },

                new ClippingTool()
                {
                    Name = "Rectangle X-Ray",
                    Action = RectagleXRayTool
                }
            };

            _tools = new CollectionView(tools);
            ImageContent = BitmapFactory.New(400, 400);
            DrawingLines = new List<(int x1, int y1, int x2, int y2)>();
        }
    }
}