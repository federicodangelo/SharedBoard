using SharedBoard.Model;
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
    public sealed partial class BoardView : UserControl
    {
        private ScrollViewer scrollViewer;

        private IBoardControlView selectedControlView;

        private Board board = new Board();

        private readonly List<IBoardControlView> boardControlViews = new List<IBoardControlView>();

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

        public IBoardControlView SelectedBoardControlView
        {
            get
            {
                return selectedControlView;
            }

            set
            {
                if (selectedControlView != value)
                {
                    if (selectedControlView != null)
                        selectedControlView.Selected = false;

                    selectedControlView = value;
                }

                if (selectedControlView != null)
                {
                    selectedControlView.Selected = true;
                    selectedControlTools.Show(selectedControlView);
                    var maxZIndex = boardControlViews.Aggregate(0, (acc, bc) => Math.Max(acc, Canvas.GetZIndex(bc.Control)));
                    Canvas.SetZIndex(selectedControlTools, maxZIndex + 1);
                }
                else
                {
                    selectedControlTools.Hide();
                }
            }
        }

        public BoardView()
        {
            this.InitializeComponent();
            selectedControlTools.Board = this;
        }

        public StickyNoteView AddStickyNote(Point position)
        {
            var stickyNote = new StickyNote
            {
                Position = position,
                Size = StickyNoteView.DefaultSize
            };

            return AddStickyNote(stickyNote);
        }

        public StickyNoteView AddStickyNote(StickyNote stickyNote)
        {
            var stickyNoteView = new StickyNoteView();

            InitBoardControlView(stickyNoteView, stickyNote);

            AddBoardControlView(stickyNoteView);

            SetTopBoardControlView(stickyNoteView);

            return stickyNoteView;
        }

        private void InitBoardControlView(IBoardControlView boardControlView, BoardControl boardControl)
        {
            Canvas.SetLeft(boardControlView.Control, boardControl.Position.X);
            Canvas.SetTop(boardControlView.Control, boardControl.Position.Y);
            boardControlView.Control.Width = boardControl.Size.Width;
            boardControlView.Control.Height = boardControl.Size.Height;

            boardControlView.Init(this, boardControl);
        }

        public void AddBoardControlView(IBoardControlView boardControlView)
        {
            board.AddBoardControl(boardControlView.BoardControl);

            mainCanvas.Children.Add(boardControlView.Control);

            boardControlViews.Add(boardControlView);
        }

        public Point GetBoardControlViewPosition(IBoardControlView boardControlView)
        {
            return new Point(Canvas.GetLeft(boardControlView.Control), Canvas.GetTop(boardControlView.Control));
        }

        public void MoveBoardControlView(IBoardControlView boardControlView, Point position)
        {
            Canvas.SetLeft(boardControlView.Control, position.X);
            Canvas.SetTop(boardControlView.Control, position.Y);
        }

        public void SetTopBoardControlView(IBoardControlView boardControlView)
        {
            var maxZIndex = boardControlViews.Where(x => x != boardControlView).Aggregate(0, (acc, bc) => Math.Max(acc, Canvas.GetZIndex(bc.Control)));

            Canvas.SetZIndex(boardControlView.Control, maxZIndex + 1);
            Canvas.SetZIndex(selectedControlTools, maxZIndex + 2);
        }

        public void RemoveBoardControlView(IBoardControlView boardControlView)
        {
            if (boardControlView == selectedControlView)
            {
                SelectedBoardControlView = null;
            }

            boardControlView.StopEdit();
            boardControlViews.Remove(boardControlView);
            mainCanvas.Children.Remove(boardControlView.Control);

            board.RemoveBoardControl(boardControlView.BoardControl);
        }

        public void StopAllEdits()
        {
            boardControlViews.ForEach(x => x.StopEdit());
        }
        
        private void Board_Tapped(object sender, TappedRoutedEventArgs e)
        {
            StopAllEdits();
            SelectedBoardControlView = null;
        }

        private void Board_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            var pos = e.GetPosition(mainCanvas);
            pos.X -= StickyNoteView.DefaultSize.Width / 2;
            pos.Y -= StickyNoteView.DefaultSize.Height / 2;

            AddStickyNote(pos).StartEdit(true);
        }
    }
}
