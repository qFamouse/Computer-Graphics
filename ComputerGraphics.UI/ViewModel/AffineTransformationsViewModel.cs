using ComputerGraphics.Core.Algorithms.AffineTransformations;
using ComputerGraphics.Core.Algorithms.Rasterization.Primitives;
using ComputerGraphics.Core.Entities;
using ComputerGraphics.UI.Models;
using ComputerGraphics.UI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace ComputerGraphics.UI.ViewModel
{
    internal class AffineTransformationsViewModel : BaseViewModel
    {
        #region Keyboard

        delegate void KeyHandler(Key key);
        event KeyHandler OnKeyboard;

        public RelayCommand UpperArrowPresssedCommand => new RelayCommand(obj => OnKeyboard(Key.Up));
        public RelayCommand RightArrowPresssedCommand => new RelayCommand(obj => OnKeyboard(Key.Right));
        public RelayCommand BottomArrowPresssedCommand => new RelayCommand(obj => OnKeyboard(Key.Down));
        public RelayCommand LeftArrowPresssedCommand => new RelayCommand(obj => OnKeyboard(Key.Left));

        #endregion

        public Canvas Canvas => MainCanvas.GetInstance().Canvas;
        public WriteableBitmap ImageContent { get; private set; }
        public CustomPolygon DrawingPolygon { get; set; }
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

        #region List Box

        private CollectionView _tools;

        private ActionTool _selectedTool;
        public CollectionView Tools => _tools;

        public ActionTool SelectedTool
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

        private void DrawPolygon(CustomPolygon polygon)
        {
            ImageContent.Clear();
            foreach (var line in polygon.Lines)
            {
                ImageContent.DrawLine((int)line.P1.X, (int)line.P1.Y, (int)line.P2.X, (int)line.P2.Y, Colors.Blue);
            }
        }

        private void Drawing()
        {
            Image image = NewImage();
            ImageContent.Clear();

            List<int> polygonPoints = new List<int>();

            void AddPolygonPoint(object sender, System.Windows.Input.MouseButtonEventArgs e)
            {
                Point point = e.GetPosition(image);
                int x = (int)point.X;
                int y = (int)point.Y;

                polygonPoints.Add(x);
                polygonPoints.Add(y);

                ImageContent.FillEllipseCentered(x, y, 1, 1, Colors.Blue);
            }

            void RegisterPolygon(object sender, System.Windows.Input.MouseButtonEventArgs e)
            {
                image.MouseLeftButtonDown -= AddPolygonPoint;
                image.MouseRightButtonDown -= RegisterPolygon;

                polygonPoints.AddRange(polygonPoints.Take(2).ToList());

                var polygonPointsArray = polygonPoints.ToArray();

                DrawingPolygon = new CustomPolygon(polygonPointsArray);

                DrawPolygon(DrawingPolygon);
            }

            image.MouseLeftButtonDown += AddPolygonPoint;
            image.MouseRightButtonDown += RegisterPolygon;
        }

        private void Moving()
        {
            Image image = NewImage();
            DrawPolygon(DrawingPolygon);

            void PolygonMoving(Key key)
            {
                switch (key)
                {
                    case Key.Up:
                        DrawingPolygon = AffineTransformationsApplier.Apply(DrawingPolygon, AffineTransformations.Translation(0, -1));
                        break;
                    case Key.Right:
                        DrawingPolygon = AffineTransformationsApplier.Apply(DrawingPolygon, AffineTransformations.Translation(1, 0));
                        break;
                    case Key.Down:
                        DrawingPolygon = AffineTransformationsApplier.Apply(DrawingPolygon, AffineTransformations.Translation(0, 1));
                        break;
                    case Key.Left:
                        DrawingPolygon = AffineTransformationsApplier.Apply(DrawingPolygon, AffineTransformations.Translation(-1, 0));
                        break;
                }

                DrawPolygon(DrawingPolygon);
            }

            OnKeyboard += PolygonMoving;
        }

        private void Scaling()
        {
            Image image = NewImage();
            DrawPolygon(DrawingPolygon);

            image.MouseWheel += (s, e) =>
            {
                if (e.Delta > 0)
                {
                    DrawingPolygon = AffineTransformationsApplier.Apply(DrawingPolygon, AffineTransformations.Dilatation(1.01, 1.01));
                }
                else
                {
                    DrawingPolygon = AffineTransformationsApplier.Apply(DrawingPolygon, AffineTransformations.Dilatation(0.99, 0.99));
                }

                DrawPolygon(DrawingPolygon);
            };
        }

        private void Rotation()
        {
            Image image = NewImage();
            DrawPolygon(DrawingPolygon);

            image.MouseWheel += (s, e) =>
            {
                if (e.Delta > 0)
                {
                    DrawingPolygon = AffineTransformationsApplier.Apply(DrawingPolygon, AffineTransformations.Rotation(-1));
                }
                else
                {
                    DrawingPolygon = AffineTransformationsApplier.Apply(DrawingPolygon, AffineTransformations.Rotation(1));
                }

                DrawPolygon(DrawingPolygon);
            };
        }

        public AffineTransformationsViewModel()
        {
            var tools = new List<ActionTool>()
            {
                new ActionTool()
                {
                    Name = "Drawing",
                    Action = Drawing
                },
                new ActionTool()
                {
                    Name = "Moving",
                    Action = Moving
                },
                new ActionTool()
                {
                    Name = "Scaling",
                    Action = Scaling
                },
                new ActionTool()
                {
                    Name = "Rotation",
                    Action = Rotation
                },
            };

            _tools = new CollectionView(tools);
            ImageContent = BitmapFactory.New(400, 400);
            DrawingPolygon = new CustomPolygon();
        }
    }
}
