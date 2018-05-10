using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace SharedBoard.Model
{
    public class StickyNote : BoardControl
    {
        public static readonly Size DefaultSize = new Size(300, 300);

        private string text = "Sticky Note";
        public string Text { get => text; set => text = value; }
    }
}
