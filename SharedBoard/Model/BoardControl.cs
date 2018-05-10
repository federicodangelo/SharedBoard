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
        private Point position;
        private Size size;

        public Point Position { get => position; set => position = value; }
        public Size Size { get => size; set => size = value; }
    }
}
