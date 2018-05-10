using SharedBoard.Model;
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
        void Init(BoardView boardView, BoardControl boardControl);

        BoardView BoardView { get; }

        BoardControl BoardControl { get; }

        Control Control { get; }

        Rect Bounds { get; }

        bool Selected { get; set; }

        void StartEdit();

        void StopEdit();
    }
}
