using Windows.Foundation;

namespace SharedBoard.Model.Controls
{
    public abstract class BoardControl
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }

        public Point Position { get => new Point(X, Y); set { X = value.X; Y = value.Y; } }
        public Size Size { get => new Size(Width, Height); set { Width = value.Width; Height = value.Height; } }
    }
}
