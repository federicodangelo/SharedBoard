using Windows.Foundation;

namespace SharedBoard.Model.Controls
{
    public class StickyNote : BoardControl
    {
        public static readonly Size DefaultSize = new Size(300, 300);
        public string Text { get; set; } = "Sticky Note";
    }
}
