using SharedBoard.Model.Controls;
using SharedBoard.ViewModel.Utils;

namespace SharedBoard.ViewModel.Controls
{
    public class BoardControlViewModel : BindableBase
    {
        public BoardControl BoardControl { get; }

        public double X
        {
            get => BoardControl.X;
            set => SetProperty(BoardControl.X, value, (v) => BoardControl.X = v);
        }

        public double Y
        {
            get => BoardControl.Y;
            set => SetProperty(BoardControl.Y, value, (v) => BoardControl.Y = v);
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

        public BoardControlViewModel(BoardControl boardControl)
        {
            BoardControl = boardControl;
        }
    }
}
