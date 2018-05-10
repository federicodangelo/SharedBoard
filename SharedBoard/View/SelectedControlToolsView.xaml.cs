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
        private IBoardControlView boardControl;
        private long topPropertyChangedToken;
        private long leftPropertyChangedToken;

        public BoardView Board { set => board = value; }

        public SelectedControlToolsView()
        {
            this.InitializeComponent();
        }

        public void Show(IBoardControlView boardControl)
        {
            Hide();

            this.boardControl = boardControl;

            Visibility = Visibility.Visible;

            UpdatePosition();

            boardControl.Control.SizeChanged += BoardControl_SizeChanged;
            topPropertyChangedToken = boardControl.Control.RegisterPropertyChangedCallback(Canvas.TopProperty, (a, b) => UpdatePosition());
            leftPropertyChangedToken = boardControl.Control.RegisterPropertyChangedCallback(Canvas.LeftProperty, (a, b) => UpdatePosition());
        }

        private void BoardControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            var boardControlBounds = boardControl.Bounds;

            Canvas.SetTop(this, boardControlBounds.Top - 2);
            Canvas.SetLeft(this, boardControlBounds.Left - 2);
            this.Width = boardControlBounds.Width + 4;
            this.Height = boardControlBounds.Height + 4;
        }

        public void Hide()
        {
            Visibility = Visibility.Collapsed;

            if (boardControl != null)
            {
                boardControl.Control.UnregisterPropertyChangedCallback(Canvas.TopProperty, topPropertyChangedToken);
                boardControl.Control.UnregisterPropertyChangedCallback(Canvas.LeftProperty, leftPropertyChangedToken);
                boardControl.Control.SizeChanged -= BoardControl_SizeChanged;
            }

            boardControl = null;
        }

        private void OnDeleteButtonClick(object sender, RoutedEventArgs e)
        {
            board.RemoveBoardControlView(boardControl);
        }

        private void OnEditButtonClick(object sender, RoutedEventArgs e)
        {
            boardControl.StartEdit();
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
