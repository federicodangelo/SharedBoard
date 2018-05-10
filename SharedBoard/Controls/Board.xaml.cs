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
    public sealed partial class Board : UserControl
    {
        public Board()
        {
            this.InitializeComponent();
        }

        public void AddStickyNote(Point position)
        {
            var stickyNote = new StickyNote();

            stickyNote.Board = this;

            Canvas.SetLeft(stickyNote, position.X);
            Canvas.SetTop(stickyNote, position.Y);

            mainCanvas.Children.Add(stickyNote);
        }

        private void UserControl_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            AddStickyNote(e.GetPosition(mainCanvas));
        }

        public Point GetChildPosition(Control control)
        {
            return new Point(Canvas.GetLeft(control), Canvas.GetTop(control));
        }

        public void MoveChildTo(Control control, Point position)
        {
            Canvas.SetLeft(control, position.X);
            Canvas.SetTop(control, position.Y);
        }

        public void SetTopChild(Control control)
        {
            var index = 0;
            for (var i = 0; i < mainCanvas.Children.Count; i++)
            {
                var child = mainCanvas.Children[i];

                if (child != control)
                {
                    Canvas.SetZIndex(child, index++);
                }
            }

            Canvas.SetZIndex(control, index);
        }

        public Rect Bounds
        {
            get
            {
                return new Rect(0, 0, ActualWidth, ActualHeight);
            }
        }
    }
}
