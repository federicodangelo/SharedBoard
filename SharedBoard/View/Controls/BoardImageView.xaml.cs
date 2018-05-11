using SharedBoard.ViewModel.Controls;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace SharedBoard.View.Controls
{
    public sealed partial class BoardImageView : UserControl, IBoardControlView
    {
        public BoardView BoardView { get; private set; }

        public BoardControlViewModel BoardControlViewModel { get; private set; }

        public BoardImageViewModel BoardImageViewModel => BoardControlViewModel as BoardImageViewModel;

        public Rect VisibleBounds => new Rect(Canvas.GetLeft(this), Canvas.GetTop(this), ActualWidth, ActualHeight);

        public bool CanBeMoved => !IsEditing;

        public bool IsEditing => textBox.Visibility == Visibility.Visible;

        public bool Selected { get; set; }

        public Control Control => this;

        public BoardImageView()
        {
            this.InitializeComponent();

            textBox.Visibility = Visibility.Collapsed;
        }

        public void Init(BoardView boardView, BoardControlViewModel boardControlViewModel)
        {
            BoardView = boardView;
            BoardControlViewModel = boardControlViewModel;
        }

        public void StartEdit()
        {
            StartEdit(false);
        }

        public void StartEdit(bool selectAllText)
        {
            if (IsEditing)
                return;

            BoardView.ViewModel.SelectedBoardControlViewModel = BoardControlViewModel;

            image.Visibility = Visibility.Collapsed;
            textBox.Visibility = Visibility.Visible;
            textBox.Focus(FocusState.Pointer);

            if (selectAllText)
                textBox.SelectAll();
        }

        public void StopEdit()
        {
            if (!IsEditing)
                return;

            textBox.Visibility = Visibility.Collapsed;
            image.Visibility = Visibility.Visible;
        }

        private void Control_Tapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = true;
            BoardView.SetTopBoardControlView(this);
            BoardView.ViewModel.SelectedBoardControlViewModel = BoardControlViewModel;
        }

        private void Control_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            e.Handled = true;
            StartEdit();
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            StopEdit();
        }

        private void Control_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            BoardView.SetTopBoardControlView(this);
            BoardView.ViewModel.SelectedBoardControlViewModel = BoardControlViewModel;
        }

        private void Control_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            if (!CanBeMoved)
                return;

            var bounds = VisibleBounds;

            var delta = e.Delta.Translation;

            delta.X /= BoardView.ZoomFactor;
            delta.Y /= BoardView.ZoomFactor;

            BoardControlViewModel.X = bounds.Left + delta.X;
            BoardControlViewModel.Y = bounds.Top + delta.Y;
        }
    }
}
