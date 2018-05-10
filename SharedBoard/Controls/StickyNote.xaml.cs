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

namespace SharedBoard.Controls
{
    public sealed partial class StickyNote : UserControl
    {
        private Board board;

        public Board Board { get => board; set => board = value; }

        public Rect Bounds {
            get
            {
                var pos = board.GetChildPosition(this);
                return new Rect(pos.X, pos.Y, ActualWidth, ActualHeight);
            }
        }

        public StickyNote()
        {
            this.InitializeComponent();

            textBox.Visibility = Visibility.Collapsed;
        }

        private void StickyNote_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            e.Handled = true;
            textBlock.Visibility = Visibility.Collapsed;
            textBox.Visibility = Visibility.Visible;
            textBox.Focus(FocusState.Pointer);
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            textBox.Visibility = Visibility.Collapsed;
            textBlock.Visibility = Visibility.Visible;
            textBlock.Text = textBox.Text;
        }


        private void StickyNote_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            var boardBounds = board.Bounds;
            var bounds = Bounds;

            var delta = e.Delta.Translation;

            if (bounds.Right + delta.X > boardBounds.Right)
                delta.X = boardBounds.Right - bounds.Right;

            if (bounds.Left + delta.X < boardBounds.Left)
                delta.X = boardBounds.Left - bounds.Left;

            if (bounds.Bottom + delta.Y > boardBounds.Bottom)
                delta.Y = boardBounds.Bottom - bounds.Bottom;

            if (bounds.Top < boardBounds.Top)
                delta.Y = boardBounds.Top - bounds.Top;

            board.MoveChildTo(this, new Point(bounds.Left + delta.X, bounds.Top + delta.Y));
        }

        private void StickyNote_Tapped(object sender, TappedRoutedEventArgs e)
        {
            board.SetTopChild(this);
        }

        private void StickyNote_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            board.SetTopChild(this);
        }
    }
}
