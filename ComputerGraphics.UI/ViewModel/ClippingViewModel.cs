using ComputerGraphics.Core.Algorithms.Clipping.CohenSutherland;
using ComputerGraphics.Core.Algorithms.Clipping.WeilerAtherton;
using ComputerGraphics.Core.Algorithms.Rasterization.Primitives;
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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ComputerGraphics.UI.ViewModel
{
    internal class ClippingViewModel : BaseViewModel
    {
        delegate void KeyHandler(Key key);
        event KeyHandler OnKeyboard;

        public RelayCommand UpperArrowPresssedCommand => new RelayCommand(obj => OnKeyboard(Key.Up));
        public RelayCommand RightArrowPresssedCommand => new RelayCommand(obj => OnKeyboard(Key.Right));
        public RelayCommand BottomArrowPresssedCommand => new RelayCommand(obj => OnKeyboard(Key.Down));
        public RelayCommand LeftArrowPresssedCommand => new RelayCommand(obj => OnKeyboard(Key.Left));

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

        private void LineClipping()
        {
            Image image = NewImage();

            ImageContent.Clear();
            foreach (var line in DrawingLines)
            {
                ImageContent.DrawLine(line.x1, line.y1, line.x2, line.y2, Colors.Blue);
            }

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
            image.MouseDown += (object sender, System.Windows.Input.MouseButtonEventArgs e) =>
            {
                if (e.ChangedButton == MouseButton.Middle)
                {
                    ClearContent();
                }
            };

            // X-Ray
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

            // Save Rectange Points and Drawing this rectangle
            image.MouseRightButtonDown += (s, e) => SaveUpperLeftPoint(e.GetPosition(image));
            image.MouseRightButtonDown += (s, e) => image.MouseMove += MouseMove_SaveLowerRightPoint;
            image.MouseRightButtonDown += (s, e) => image.MouseMove += MouseMove_PreviewXRayRectangle;

            image.MouseRightButtonUp += (s, e) => image.MouseMove -= MouseMove_PreviewXRayRectangle;
            image.MouseRightButtonUp += (s, e) => image.MouseMove -= MouseMove_SaveLowerRightPoint;

            // Keyboard hotkeys
            void XRayRectangleShift(Key key)
            {
                switch (key)
                {
                    case Key.Up:
                        xRayRectangle.y1--;
                        xRayRectangle.y2--;
                        break;
                    case Key.Right:
                        xRayRectangle.x1++;
                        xRayRectangle.x2++;
                        break;
                    case Key.Down:
                        xRayRectangle.y1++;
                        xRayRectangle.y2++;
                        break;
                    case Key.Left:
                        xRayRectangle.x1--;
                        xRayRectangle.x2--;
                        break;
                }

                PreviewXRayRectangle();
            }

            image.MouseLeftButtonUp += (s, e) => OnKeyboard += XRayRectangleShift;
            image.MouseLeftButtonDown += (s, e) => OnKeyboard -= XRayRectangleShift;

            // When leave stop drawing
            image.MouseLeave += (s, e) => image.MouseMove -= MouseMove_PreviewXRayRectangle;
            image.MouseLeave += (s, e) => image.MouseMove -= MouseMove_SaveLowerRightPoint;
        }

        private void PolygonClipping()
        {
            Image image = NewImage();
            ImageContent.Clear();

            List<int> polygonPoints = new List<int>();
            List<int> clipPolygonPoints = new List<int>();

            CustomPolygon polygon = new CustomPolygon();
            CustomPolygon clipPolygon = new CustomPolygon();

            IEnumerable<CustomLine> clippedPolygonLines = null;

            void AddPolygonPoint(object sender, System.Windows.Input.MouseButtonEventArgs e)
            {
                Point point = e.GetPosition(image);
                int x = (int)point.X;
                int y = (int)point.Y;

                polygonPoints.Add(x);
                polygonPoints.Add(y);

                ImageContent.FillEllipseCentered(x, y, 1, 1, Colors.Blue);
            }

            void AddClipPolygonPoint(object sender, System.Windows.Input.MouseButtonEventArgs e)
            {
                Point point = e.GetPosition(image);
                int x = (int)point.X;
                int y = (int)point.Y;

                clipPolygonPoints.Add(x);
                clipPolygonPoints.Add(y);

                ImageContent.FillEllipseCentered(x, y, 1, 1, Colors.Red);
            }

            void PolygonShowing(object sender, MouseWheelEventArgs e)
            {
                ImageContent.Clear();
                if (e.Delta > 0 && clippedPolygonLines != null)
                {
                    foreach (var line in clippedPolygonLines)
                    {
                        ImageContent.DrawLine((int)line.P1.X, (int)line.P1.Y, (int)line.P2.X, (int)line.P2.Y, Colors.Blue);
                    }
                }
                else
                {
                    ImageContent.DrawPolyline(polygonPoints.ToArray(), Colors.Blue);
                    ImageContent.DrawPolyline(clipPolygonPoints.ToArray(), Colors.Red);
                }
            }


            image.MouseLeftButtonDown += AddPolygonPoint;
            image.MouseRightButtonDown += AddClipPolygonPoint;
            image.MouseDown += (object sender, System.Windows.Input.MouseButtonEventArgs e) =>
            {
                if (e.ChangedButton == MouseButton.Middle && polygonPoints.Count > 0 && clipPolygonPoints.Count > 0)
                {
                    image.MouseLeftButtonDown -= AddPolygonPoint;
                    image.MouseRightButtonDown -= AddClipPolygonPoint;

                    // Add First points
                    polygonPoints.AddRange(polygonPoints.Take(2).ToList());
                    clipPolygonPoints.AddRange(clipPolygonPoints.Take(2).ToList());

                    image.MouseWheel += PolygonShowing;

                    ImageContent.Clear();

                    var polygonPointsArray = polygonPoints.ToArray();
                    var clipPolygonPointsArray = clipPolygonPoints.ToArray();

                    ImageContent.DrawPolyline(polygonPointsArray, Colors.Blue);
                    ImageContent.DrawPolyline(clipPolygonPointsArray, Colors.Red);

                    polygon = new CustomPolygon(polygonPointsArray);
                    clipPolygon = new CustomPolygon(clipPolygonPointsArray);

                    clippedPolygonLines = WeilerAthertonAlgorithm.Clip(polygon, clipPolygon);
                }
            };



        }

        #endregion

        private Image NewImage()
        {
            Canvas.Children.Clear();

            ImageContent = BitmapFactory.New((int)Canvas.Width, (int)Canvas.Height);

            Image image = new Image()
            {
                Source = ImageContent,
                Width = Canvas.Width,
                Height = Canvas.Height
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
                    Name = "Line Clipping",
                    Action = LineClipping
                },
                new ClippingTool()
                {
                    Name = "Polyline Clipping",
                    Action = PolygonClipping
                },
            };

            _tools = new CollectionView(tools);
            ImageContent = BitmapFactory.New(400, 400);
            DrawingLines = new List<(int x1, int y1, int x2, int y2)>();
        }
    }
}