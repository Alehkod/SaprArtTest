using MyRectangle = SaprArtTest.UI.Models.Rectangle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SaprArtTest.UI.Controls
{
    /// <summary>
    /// Interaction logic for DrawingAreaControl.xaml
    /// </summary>
    public partial class DrawingAreaControl : UserControl
    {
        public Point Point { get; set; }

        public DrawingAreaControl()
        {
            InitializeComponent();
        }

        private void Area_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point = e.GetPosition(this);
        }


        public void DrawingRectangle(MyRectangle myRectangle)
        {
            Rectangle rect = new Rectangle()
            {
                //Width = myRectangle.BottomRight.X - myRectangle.TopLeft.X,
                //Height = myRectangle.BottomRight.Y - myRectangle.TopLeft.Y,
                Stroke = new SolidColorBrush(Colors.Black),
                DataContext = myRectangle,
            };

            if (!myRectangle.IsMain)
                rect.Fill = new SolidColorBrush(myRectangle.Color);
            else
            {
                rect.Uid = "Main";
                rect.StrokeThickness = 5;
            }
            rect.SetBinding(Canvas.LeftProperty, new Binding("StartX"));
            rect.SetBinding(Canvas.TopProperty, new Binding("StartY"));
            rect.SetBinding(WidthProperty, new Binding("Width"));
            rect.SetBinding(HeightProperty, new Binding("Height"));

            Area.Children.Add(rect);
            
        }
    }
}
