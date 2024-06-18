using NLog;
using SaprArtTest.UI.Models;
using SaprArtTest.UI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace SaprArtTest.UI.Services
{
    public class RectangleService
    {
        private readonly RectangleRepository _rectangleRepository;
        private readonly ILogger _logger;

        public RectangleService(RectangleRepository rectangleRepository, ILogger logger)
        {
            _rectangleRepository = rectangleRepository;
            _logger = logger;
        }

        public void UpdateMainRectangle(Color? color = null)
        {
            try
            {
                List<Rectangle> rectangles;

                if (color == null)
                {
                    rectangles = _rectangleRepository.Rectangles;
                }
                else
                {
                    rectangles = _rectangleRepository.Rectangles.Where(rect => rect.Color == color).ToList();
                }

                var mainRectangle = _rectangleRepository.MainRectangle ?? throw new Exception("Not created Main Rectangle");

                List<Point> points = new List<Point>();

                foreach (var rectangle in rectangles)
                {
                    if (checkPointInRectangle(rectangle.TopLeft, mainRectangle)) points.Add(rectangle.TopLeft);
                    if (checkPointInRectangle(rectangle.TopRight, mainRectangle)) points.Add(rectangle.TopRight);
                    if (checkPointInRectangle(rectangle.BottomLeft, mainRectangle)) points.Add(rectangle.BottomLeft);
                    if (checkPointInRectangle(rectangle.BottomRight, mainRectangle)) points.Add(rectangle.BottomRight);
                }

                var pointsX = points.Select(p => p.X).ToList();
                var pointsY = points.Select(p => p.Y).ToList();

                double minX = pointsX.Min();
                double maxX = pointsX.Max();
                double minY = pointsY.Min();
                double maxY = pointsY.Max();

                mainRectangle.TopLeft = new Point(minX, minY);
                mainRectangle.BottomRight = new Point(maxX, maxY); 

                _logger.Info($"Update main rectangle ({mainRectangle}) with color: {color}");
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString() );
            }
            
        }

        private bool checkPointInRectangle(Point point, Rectangle rectangle)
        {
            if (point.X >= rectangle.TopLeft.X && point.X <= rectangle.TopRight.X)
            {
                if (point.Y >= rectangle.TopLeft.Y && point.Y <= rectangle.BottomLeft.Y)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
