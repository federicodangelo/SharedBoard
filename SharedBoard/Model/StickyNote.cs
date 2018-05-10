using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedBoard.Model
{
    public class StickyNote : BoardControl
    {
        private string text = "Sticky Note";
        public string Text { get => text; set => text = value; }
    }
}
