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
    public sealed partial class StickyNote : UserControl, IBoardControl
    {
        public static readonly Size DefaultSize = new Size(300, 300);

        private Board board;

        private bool selected;

        public Board Board { get => board; set => board = value; }

        public Rect Bounds
        {
            get
            {
                var pos = board.GetChildPosition(this);
                return new Rect(pos.X, pos.Y, ActualWidth, ActualHeight);
            }
        }

        public bool CanBeMoved => !IsEditing;

        public bool IsEditing => textBox.Visibility == Visibility.Visible;

        public bool Selected
        {
            get
            {
                return selected;
            }
            set
            {
                this.selected = value;
            }
        }

        public StickyNote()
        {
            this.InitializeComponent();

            textBox.Visibility = Visibility.Collapsed;
        }


        public void StartEdit()
        {
            StartEdit(false);
        }

        public void StartEdit(bool selectAllText)
        {
            if (IsEditing)
                return;

            board.SelectedControl = this;

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
            textBlock.Text = textBox.Text;
        }

        private void StickyNote_Tapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = true;
            board.SetTopChild(this);
            board.SelectedControl = this;
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
            board.SetTopChild(this);
            board.SelectedControl = this;
        }

        private void StickyNote_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            if (!CanBeMoved)
                return;

            var boardBounds = board.Bounds;
            var bounds = Bounds;

            var delta = e.Delta.Translation;

            delta.X /= board.ZoomFactor;
            delta.Y /= board.ZoomFactor;

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
    }
}
