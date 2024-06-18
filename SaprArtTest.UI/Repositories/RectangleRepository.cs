using SaprArtTest.UI.Models;
using System.Collections.Generic;

namespace SaprArtTest.UI.Repositories
{
    public class RectangleRepository
    {
        public List<Rectangle> Rectangles { get; set; } = new List<Rectangle>();

        public Rectangle MainRectangle { get; set; }
    }
}
