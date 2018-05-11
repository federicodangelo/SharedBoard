using SharedBoard.ViewModel.Controls;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace SharedBoard.View.Controls
{
    public sealed partial class StickyNoteView : UserControl, IBoardControlView
    {
        public BoardView BoardView { get; private set; }

        public BoardControlViewModel BoardControlViewModel { get; private set; }

        public StickyNoteViewModel StickyNoteViewModel => BoardControlViewModel as StickyNoteViewModel;

        public Rect VisibleBounds => new Rect(Canvas.GetLeft(this), Canvas.GetTop(this), ActualWidth, ActualHeight);

        public bool CanBeMoved => !IsEditing;

        public bool IsEditing => textBox.Visibility == Visibility.Visible;

        public bool Selected { get; set; }

        public Control Control => this;

        public StickyNoteView()
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

            textBlock.Visibility = Visibility.Collapsed;
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
            textBlock.Visibility = Visibility.Visible;
        }

        private void StickyNote_Tapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = true;
            BoardView.SetTopBoardControlView(this);
            BoardView.ViewModel.SelectedBoardControlViewModel = BoardControlViewModel;
        }

        private void StickyNote_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            e.Handled = true;
            StartEdit();
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            StopEdit();
        }

        private void StickyNote_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            BoardView.SetTopBoardControlView(this);
            BoardView.ViewModel.SelectedBoardControlViewModel = BoardControlViewModel;
        }

        private void StickyNote_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            if (!CanBeMoved)
                return;

            var boardBounds = BoardView.Bounds;
            var bounds = VisibleBounds;

            var delta = e.Delta.Translation;

            delta.X /= BoardView.ZoomFactor;
            delta.Y /= BoardView.ZoomFactor;

            if (bounds.Right + delta.X > boardBounds.Right)
                delta.X = boardBounds.Right - bounds.Right;

            if (bounds.Left + delta.X < boardBounds.Left)
                delta.X = boardBounds.Left - bounds.Left;

            if (bounds.Bottom + delta.Y > boardBounds.Bottom)
                delta.Y = boardBounds.Bottom - bounds.Bottom;

            if (bounds.Top < boardBounds.Top)
                delta.Y = boardBounds.Top - bounds.Top;

            BoardControlViewModel.X = bounds.Left + delta.X;
            BoardControlViewModel.Y = bounds.Top + delta.Y;
        }
    }
}
