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
        private ScrollViewer scrollViewer;

        private IBoardControl selectedControl;

        private readonly List<IBoardControl> boardControls = new List<IBoardControl>();

        public Rect Bounds => new Rect(0, 0, ActualWidth, ActualHeight);

        public ScrollViewer ScrollViewer { get => scrollViewer; set => scrollViewer = value; }

        public float ZoomFactor => scrollViewer.ZoomFactor;

        public Point VisibleCenter
        {
            get
            {
                var x = Bounds.Width / 2 + (-scrollViewer.ScrollableWidth / 2 + scrollViewer.HorizontalOffset) / ZoomFactor;
                var y = Bounds.Height / 2 + (-scrollViewer.ScrollableHeight / 2 + scrollViewer.VerticalOffset) / ZoomFactor;

                return new Point(x, y);
            }
        }

        public IBoardControl SelectedControl
        {
            get
            {
                return selectedControl;
            }

            set
            {
                if (selectedControl != value)
                {
                    if (selectedControl != null)
                        selectedControl.Selected = false;

                    selectedControl = value;
                }

                if (selectedControl != null)
                {
                    selectedControl.Selected = true;
                    selectedControlTools.Show(selectedControl, (Control) selectedControl);
                }
                else
                {
                    selectedControlTools.Hide();
                }
            }
        }

        public Board()
        {
            this.InitializeComponent();
            selectedControlTools.Board = this;
        }

        public StickyNote AddStickyNote(Point position)
        {
            var stickyNote = new StickyNote
            {
                Board = this
            };

            Canvas.SetLeft(stickyNote, position.X);
            Canvas.SetTop(stickyNote, position.Y);

            mainCanvas.Children.Add(stickyNote);

            AddBoardControl(stickyNote);

            SetTopChild(stickyNote);

            return stickyNote;
        }

        private void AddBoardControl(StickyNote stickyNote)
        {
            boardControls.Add(stickyNote);
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

        public void RemoveControl(IBoardControl boardControl)
        {
            if (boardControl == selectedControl)
            {
                SelectedControl = null;
            }

            boardControl.StopEdit();
            boardControls.Remove(boardControl);
            mainCanvas.Children.Remove((Control)boardControl);
        }

        public void StopAllEdits()
        {
            boardControls.ForEach(x => x.StopEdit());
        }
        
        private void Board_Tapped(object sender, TappedRoutedEventArgs e)
        {
            StopAllEdits();
            SelectedControl = null;
        }

        private void Board_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            var pos = e.GetPosition(mainCanvas);
            pos.X -= StickyNote.DefaultSize.Width / 2;
            pos.Y -= StickyNote.DefaultSize.Height / 2;

            AddStickyNote(pos).StartEdit(true);
        }
    }
}
