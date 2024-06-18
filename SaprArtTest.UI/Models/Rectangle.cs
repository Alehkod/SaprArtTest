using System.ComponentModel;
using System.Windows.Media;

namespace SaprArtTest.UI.Models
{
    public class Rectangle : INotifyPropertyChanged
    {
        private Point _topLeft;
        private Point _bottomRight;
        private Point _bottomLeft;
        private Point _topRight;

        public Point TopLeft
        {
            get
            {
                return _topLeft;
            }
            set
            {
                _topLeft = value;
                StartX = value.X;
                StartY = value.Y;
            }
        }
        public Point BottomRight
        {
            get
            {
                return _bottomRight;
            }
            set
            {
                _bottomRight = value;

                Width = value.X - _topLeft.X;
                Height = value.Y - _topLeft.Y;

                _bottomLeft = new Point(_topLeft.X, _bottomRight.Y);
                _topRight = new Point(_bottomRight.X, _topLeft.Y);
            }
        }

        public Point BottomLeft => _bottomLeft;
        public Point TopRight => _topRight;

        public bool IsMain { get; set; }

        public double StartX
        {
            get { return TopLeft.X; }
            set
            {
                OnPropertyChanged("StartX");
            }
        }
        public double StartY
        {
            get { return TopLeft.Y; }
            set
            {
                OnPropertyChanged("StartY");
            }
        }

        public double Width
        {
            get { return BottomRight.X - TopLeft.X; }
            set
            {
                OnPropertyChanged("Width");
            }
        }
        public double Height
        {
            get { return BottomRight.Y - TopLeft.Y; }
            set
            {
                OnPropertyChanged("Height");
            }
        }

        internal Color Color { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string v)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(v));
            }
        }

        public override string ToString()
        {
            return $"{TopLeft}; {BottomRight}; {IsMain};";
        }
    }
}
