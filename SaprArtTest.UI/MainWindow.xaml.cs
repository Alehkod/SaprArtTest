using MyRectangle = SaprArtTest.UI.Models.Rectangle;
using MyPoint = SaprArtTest.UI.Models.Point;
using SaprArtTest.UI.Services;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using SaprArtTest.UI.Repositories;
using NLog;


namespace SaprArtTest.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly RectangleService _rectangleService;
        private readonly RectangleRepository _rectangleRepository;
        private readonly ILogger _logger;
        private MyRectangle _rectangle;

        public MainWindow(RectangleService rectangleService, RectangleRepository rectangleRepository, ILogger logger)
        {
            InitializeComponent();
            _rectangleService = rectangleService;
            _rectangleRepository = rectangleRepository;
            _logger = logger;
        }

        private void CreateRectangle_ButtonClick(object sender, RoutedEventArgs e)
        {           
            HelperLabel.Content = "Select first point:";

            DrawingArea.IsEnabled = true;

            _rectangle = new MyRectangle();            
        }

        private void DrawingArea_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ResultLabel.Content = $"X:{DrawingArea.Point.X}, Y:{DrawingArea.Point.Y}";

            var point = DrawingArea.Point;

            if (_rectangle.TopLeft == null)
            {
                _rectangle.TopLeft = new MyPoint(point.X, point.Y);

                HelperLabel.Content = "Select second point:";
            }
            else if (_rectangle.BottomRight == null)
            {
                try
                {
                    double x;
                    double y;

                    if (_rectangle.TopLeft.X <= point.X)
                    {
                        x = point.X;
                    }
                    else
                    {
                        x = _rectangle.TopLeft.X;
                        _rectangle.TopLeft.X = point.X;
                    }
                    
                    if (_rectangle.TopLeft.Y <= point.Y)
                    {
                        y = point.Y;
                    }
                    else
                    {
                        y = _rectangle.TopLeft.Y;
                        _rectangle.TopLeft.Y = point.Y;
                    }

                    _rectangle.BottomRight = new MyPoint(x, y);

                    DrawingArea.IsEnabled = false;

                    bool isMain = CheckIsMain.IsChecked.Value;

                    if (!isMain)
                    {
                        var colorString = ((TextBlock)ColorBox.SelectedItem).Text;
                        _rectangle.Color = (Color)ColorConverter.ConvertFromString(colorString);
                        DrawingArea.DrawingRectangle(_rectangle);

                        if (_rectangleRepository.MainRectangle != null)
                        {
                            foreach (UIElement child in DrawingArea.Area.Children)
                            {
                                if (child.Uid == "Main")
                                {
                                    DrawingArea.Area.Children.Remove(child);
                                    DrawingArea.DrawingRectangle(_rectangleRepository.MainRectangle);
                                    break;
                                }

                            }
                        }

                        _rectangleRepository.Rectangles.Add(_rectangle);
                        _logger.Info($"Add rectangle {_rectangle}; Color: {_rectangle.Color}");
                    }
                    else
                    {
                        _rectangle.IsMain = true;

                        if (_rectangleRepository.MainRectangle == null)
                        {
                            _rectangleRepository.MainRectangle = _rectangle;
                            DrawingArea.DrawingRectangle(_rectangleRepository.MainRectangle);
                            _logger.Info($"Create main rectangle {_rectangle}");
                        }
                        else
                        {
                            _rectangleRepository.MainRectangle.TopLeft = _rectangle.TopLeft;
                            _rectangleRepository.MainRectangle.BottomRight = _rectangle.BottomRight;
                            _logger.Info($"Update main rectangle {_rectangle}");
                        }
                    }
                    

                    HelperLabel.Content = "Select an action:";
                }
                catch (ArgumentException)
                {
                    _rectangle.BottomRight = null;
                }
            }
        }


        private void UpdateMainRectangle_ButtonClick(object sender, RoutedEventArgs e)
        {
            var allRectangles = CheckAllRectangles.IsChecked.Value;
            Color? color = null;

            if (!allRectangles)
            {
                var colorString = ((TextBlock)ColorBox.SelectedItem).Text;
                color = (Color?)ColorConverter.ConvertFromString(colorString);
            }

            _rectangleService.UpdateMainRectangle(color);
        } 
    }
}
