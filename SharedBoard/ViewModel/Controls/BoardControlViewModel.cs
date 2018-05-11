using SharedBoard.Model.Controls;
using SharedBoard.ViewModel.Utils;

namespace SharedBoard.ViewModel.Controls
{
    public class BoardControlViewModel : BindableBase
    {
        public BoardControl BoardControl { get; }
        public BoardViewModel BoardViewModel { get; }

        public double X
        {
            get => BoardControl.X;
            set => SetProperty(BoardControl.X, ClampXtoBoardBounds(value), (v) => BoardControl.X = v);
        }

        public double Y
        {
            get => BoardControl.Y;
            set => SetProperty(BoardControl.Y, ClampYtoBoardBounds(value), (v) => BoardControl.Y = v);
        }

        public double Width
        {
            get => BoardControl.Width;
            set => SetProperty(BoardControl.Width, value, (v) => BoardControl.Width = v);
        }

        public double Height
        {
            get => BoardControl.Height;
            set => SetProperty(BoardControl.Height, value, (v) => BoardControl.Height = v);
        }

        private double ClampXtoBoardBounds(double x)
        {
            if (x + Width > BoardViewModel.Width)
                x = BoardViewModel.Width - Width;

            if (x < 0)
                x = 0;

            return x;
        }

        private double ClampYtoBoardBounds(double y)
        {
            if (y + Height > BoardViewModel.Height)
                y = BoardViewModel.Height - Height;

            if (y < 0)
                y = 0;

            return y;
        }

        public BoardControlViewModel(BoardControl boardControl, BoardViewModel boardViewModel)
        {
            BoardControl = boardControl;
            BoardViewModel = boardViewModel;
        }
    }
}
