using SharedBoard.Model;
using SharedBoard.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace SharedBoard.View
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
