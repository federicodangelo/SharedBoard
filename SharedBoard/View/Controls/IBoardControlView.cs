using SharedBoard.ViewModel.Controls;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace SharedBoard.View.Controls
{
    public interface IBoardControlView
    {
        void Init(BoardView boardView, BoardControlViewModel boardControlViewModel);

        BoardView BoardView { get; }

        BoardControlViewModel BoardControlViewModel { get; }

        Control Control { get; }

        Rect VisibleBounds { get; }

        bool Selected { get; set; }

        void StartEdit();

        void StopEdit();
    }
}
