using Windows.Foundation;
using Windows.UI.Xaml.Media.Imaging;

namespace SharedBoard.Model.Controls
{
    public class BoardImage : BoardControl
    {
        public static readonly Size DefaultSize = new Size(320, 400);
        public string ImageUrl { get; set; } = "https://loremflickr.com/320/400";
        public BitmapImage Image { get; set; }
    }
}
