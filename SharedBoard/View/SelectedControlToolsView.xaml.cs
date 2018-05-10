using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace SharedBoard.View
{
    public sealed partial class SelectedControlToolsView : UserControl
    {
        private BoardView board;
        private IBoardControlView boardControlView;
        private long topPropertyChangedToken;
        private long leftPropertyChangedToken;
        
        public BoardView Board { set => board = value; get => board; }

        public SelectedControlToolsView()
        {
            this.InitializeComponent();
        }

        public void Show(IBoardControlView boardControlView)
        {
            Hide();

            this.boardControlView = boardControlView;

            Visibility = Visibility.Visible;

            UpdatePosition();

            RegisterCallbacks();
        }

        private void RegisterCallbacks()
        {
            boardControlView.Control.SizeChanged += BoardControl_SizeChanged;
            topPropertyChangedToken = boardControlView.Control.RegisterPropertyChangedCallback(Canvas.TopProperty, (a, b) => UpdatePosition());
            leftPropertyChangedToken = boardControlView.Control.RegisterPropertyChangedCallback(Canvas.LeftProperty, (a, b) => UpdatePosition());
        }

        private void UnregisterCallbacks()
        {
            boardControlView.Control.UnregisterPropertyChangedCallback(Canvas.TopProperty, topPropertyChangedToken);
            boardControlView.Control.UnregisterPropertyChangedCallback(Canvas.LeftProperty, leftPropertyChangedToken);
            boardControlView.Control.SizeChanged -= BoardControl_SizeChanged;
        }

        private void BoardControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            var boardControlBounds = boardControlView.VisibleBounds;

            Canvas.SetTop(this, boardControlBounds.Top - 2);
            Canvas.SetLeft(this, boardControlBounds.Left - 2);
            this.Width = boardControlBounds.Width + 4;
            this.Height = boardControlBounds.Height + 4;
        }

        public void Hide()
        {
            Visibility = Visibility.Collapsed;

            if (boardControlView != null)
                UnregisterCallbacks();

            boardControlView = null;
        }

        private void OnEditButtonClick(object sender, RoutedEventArgs e)
        {
            boardControlView.StartEdit();
        }

        private void Button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = true;
        }

        private void Button_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            e.Handled = true;
        }
    }
}
