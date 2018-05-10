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
        private Control control;
        private long topPropertyChangedToken;
        private long leftPropertyChangedToken;

        public BoardView Board { set => board = value; }

        public SelectedControlToolsView()
        {
            this.InitializeComponent();
        }

        public void Show(IBoardControlView boardControl, Control control)
        {
            Hide();

            this.boardControl = boardControl;
            this.control = control;

            Visibility = Visibility.Visible;

            UpdatePosition();

            control.SizeChanged += BoardControl_SizeChanged;
            topPropertyChangedToken = control.RegisterPropertyChangedCallback(Canvas.TopProperty, (a, b) => UpdatePosition());
            leftPropertyChangedToken = control.RegisterPropertyChangedCallback(Canvas.LeftProperty, (a, b) => UpdatePosition());
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

            if (control != null)
            {
                control.UnregisterPropertyChangedCallback(Canvas.TopProperty, topPropertyChangedToken);
                control.UnregisterPropertyChangedCallback(Canvas.LeftProperty, leftPropertyChangedToken);
                control.SizeChanged -= BoardControl_SizeChanged;
            }

            boardControl = null;
            control = null;
        }

        private void OnDeleteButtonClick(object sender, RoutedEventArgs e)
        {
            board.RemoveControl(boardControl);
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
