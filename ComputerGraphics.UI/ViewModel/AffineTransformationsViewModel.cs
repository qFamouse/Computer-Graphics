using ComputerGraphics.Core.Algorithms.Rasterization.Primitives;
using ComputerGraphics.Core.Entities;
using ComputerGraphics.UI.Models;
using ComputerGraphics.UI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ComputerGraphics.UI.ViewModel
{
    internal class AffineTransformationsViewModel : BaseViewModel
    {
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

                ImageContent.Clear();

                var polygonPointsArray = polygonPoints.ToArray();

                ImageContent.DrawPolyline(polygonPointsArray, Colors.Blue);

                DrawingPolygon = new CustomPolygon(polygonPointsArray);
            }

            image.MouseLeftButtonDown += AddPolygonPoint;
            image.MouseRightButtonDown += RegisterPolygon;
        }

        private void Moving()
        {

        }

        private void Scaling()
        {

        }

        private void Rotation()
        {

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
