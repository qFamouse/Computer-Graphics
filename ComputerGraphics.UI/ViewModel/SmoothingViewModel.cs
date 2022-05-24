using ComputerGraphics.Core.Algorithms.Rasterization.Primitives;
using ComputerGraphics.Core.Algorithms.Rasterization.RasterisationAlgorithms;
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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ComputerGraphics.UI.ViewModel
{
    internal class SmoothingViewModel : BaseViewModel
    {
        public Canvas Canvas => MainCanvas.GetInstance().Canvas;
        public bool EnabledSmoothing { get; set; } = false;
        public List<(int x1, int y1, int x2, int y2)> DrawingLines { get; private set; }

        public WriteableBitmap ImageContent { get; private set; }

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
                CustomPoint p1 = new CustomPoint(newLine.x1, newLine.y1);
                CustomPoint p2 = new CustomPoint(newLine.x2, newLine.y2);
                CustomLine line = new CustomLine(p1, p2);

                if (EnabledSmoothing)
                {
                    RasterizationAlgorithms.Wu.DrawLine(ImageContent, line, Colors.Green);
                }
                else
                {
                    ImageContent.DrawLine(newLine.x1, newLine.y1, newLine.x2, newLine.y2, Colors.Purple);
                }
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
        }

        public SmoothingViewModel()
        {
            var tools = new List<ActionTool>()
            {
                new ActionTool()
                {
                    Name = "Drawing",
                    Action = Drawing
                }
            };

            _tools = new CollectionView(tools);
            ImageContent = BitmapFactory.New(400, 400);
            DrawingLines = new List<(int x1, int y1, int x2, int y2)>();
        }
    }
}
