using SharedBoard.View.Controls;
using SharedBoard.ViewModel.Utils;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace SharedBoard.View
{
    public sealed partial class SelectedControlToolsView : UserControl
    {
        private long topPropertyChangedToken;
        private long leftPropertyChangedToken;

        public BoardView Board { get; set; }
        public IBoardControlView BoardControlView { get; private set; }

        public ICommand StartEditSelectedItemCommand => new DelegateCommand(() => BoardControlView.StartEdit());

        public SelectedControlToolsView()
        {
            this.InitializeComponent();
        }

        public void Show(IBoardControlView boardControlView)
        {
            Hide();

            this.BoardControlView = boardControlView;

            Visibility = Visibility.Visible;

            UpdatePosition();

            RegisterCallbacks();
        }

        public void Hide()
        {
            Visibility = Visibility.Collapsed;

            if (BoardControlView != null)
                UnregisterCallbacks();

            BoardControlView = null;
        }

        private void RegisterCallbacks()
        {
            BoardControlView.Control.SizeChanged += BoardControl_SizeChanged;
            topPropertyChangedToken = BoardControlView.Control.RegisterPropertyChangedCallback(Canvas.TopProperty, (a, b) => UpdatePosition());
            leftPropertyChangedToken = BoardControlView.Control.RegisterPropertyChangedCallback(Canvas.LeftProperty, (a, b) => UpdatePosition());
        }

        private void UnregisterCallbacks()
        {
            BoardControlView.Control.UnregisterPropertyChangedCallback(Canvas.TopProperty, topPropertyChangedToken);
            BoardControlView.Control.UnregisterPropertyChangedCallback(Canvas.LeftProperty, leftPropertyChangedToken);
            BoardControlView.Control.SizeChanged -= BoardControl_SizeChanged;
        }

        private void UpdatePosition()
        {
            var boardControlBounds = BoardControlView.VisibleBounds;

            Canvas.SetTop(this, boardControlBounds.Top - 2);
            Canvas.SetLeft(this, boardControlBounds.Left - 2);
            this.Width = boardControlBounds.Width + 4;
            this.Height = boardControlBounds.Height + 4;
        }

        private void Button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = true;
        }

        private void Button_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            e.Handled = true;
        }

        private void BoardControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdatePosition();
        }
    }
}
