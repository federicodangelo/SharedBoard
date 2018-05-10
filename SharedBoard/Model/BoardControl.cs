using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace SharedBoard.Model
{
    public abstract class BoardControl
    {
        private double x;
        private double y;
        private double width;
        private double height;

        public Point Position { get => new Point(x, y); set { x = value.X; y = value.Y; } }
        public Size Size { get => new Size(width, height); set { width = value.Width; height = value.Height; } }

        public double X { get => x; set => x = value; }
        public double Y { get => y; set => y = value; }
        public double Width { get => width; set => width = value; }
        public double Height { get => height; set => height = value; }
    }
}
