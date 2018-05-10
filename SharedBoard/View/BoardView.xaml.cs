using SharedBoard.Model;
using SharedBoard.ViewModel;
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

        public BoardViewModel ViewModel { get; private set; }
        
        public BoardView()
        {
            this.InitializeComponent();

            ViewModel = new BoardViewModel(new Board());

            InitManualBindings();
        }

        private void InitManualBindings()
        {
            selectedControlTools.Board = this;

            //HORRIBLE HACK.. no econtre forma sencilla de hacer este binding, porque para hacer la conversion entre BoardControl y IBoardControlView necesito
            //que el conversor tenga acceso a la lista de boardControlViews, y cuando intente hacerlo con este binding:
            //< local:BoardView SelectedBoardControlView = "{x:Bind ViewModel.SelectedBoardControl, Mode=OneWay, Converter={StaticResource BoardControlConverter}, ConverterParameter={x:Bind BoardControlViews} }" />
            //no compilaba porque no le gustaba el parametro del converter...
            ViewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(ViewModel.SelectedBoardControlViewModel))
                    SetSelectedBoardControlView(FindBoardBoardView(ViewModel.SelectedBoardControlViewModel));
            };

            ViewModel.BoardControls.CollectionChanged += BoardControls_CollectionChanged;
        }

        private void BoardControls_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    if (item is StickyNoteViewModel)
                    {
                        AddStickyNote(item as StickyNoteViewModel);
                    }
                }
            }

            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {
                    if (item is BoardControlViewModel)
                    {
                        RemoveBoardControlView(FindBoardBoardView(item as BoardControlViewModel));
                    }
                }
            }
        }

        private IBoardControlView FindBoardBoardView(BoardControlViewModel boardControlViewModel)
        {
            return boardControlViews.Find(x => x.BoardControlViewModel == boardControlViewModel);
        }
        
        private StickyNoteView AddStickyNote(StickyNoteViewModel stickyNoteViewModel)
        {
            var stickyNoteView = new StickyNoteView();

            InitBoardControlView(stickyNoteView, stickyNoteViewModel);

            AddBoardControlView(stickyNoteView);

            SetTopBoardControlView(stickyNoteView);

            return stickyNoteView;
        }

        private void InitBoardControlView(IBoardControlView boardControlView, BoardControlViewModel boardControlViewModel)
        {
            boardControlView.Init(this, boardControlViewModel);
        }

        private void AddBoardControlView(IBoardControlView boardControlView)
        {
            mainCanvas.Children.Add(boardControlView.Control);

            boardControlViews.Add(boardControlView);
        }

        public void SetTopBoardControlView(IBoardControlView boardControlView)
        {
            var maxZIndex = boardControlViews.Where(x => x != boardControlView).Aggregate(0, (acc, bc) => Math.Max(acc, Canvas.GetZIndex(bc.Control)));

            Canvas.SetZIndex(boardControlView.Control, maxZIndex + 1);
            Canvas.SetZIndex(selectedControlTools, maxZIndex + 2);
        }

        private void RemoveBoardControlView(IBoardControlView boardControlView)
        {
            if (boardControlView == null)
                return;

            if (boardControlView == selectedControlView)
            {
                SetSelectedBoardControlView(null);
            }

            boardControlView.StopEdit();
            boardControlViews.Remove(boardControlView);
            mainCanvas.Children.Remove(boardControlView.Control);
        }

        public void StopAllEdits()
        {
            boardControlViews.ForEach(x => x.StopEdit());
        }

        private void SetSelectedBoardControlView(IBoardControlView value)
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

        private void Board_Tapped(object sender, TappedRoutedEventArgs e)
        {
            StopAllEdits();
            ViewModel.LastPointerPosition = e.GetPosition(mainCanvas);
            ViewModel.SelectedBoardControlViewModel = null;
        }

        private void Board_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            var pos = e.GetPosition(mainCanvas);
            ViewModel.LastPointerPosition = pos;
            var cmd = ViewModel.CreateAddStickyNoteCommand;
            if (cmd.CanExecute(null))
                cmd.Execute(null);
        }

        private void Board_LayoutUpdated(object sender, object e)
        {
            ViewModel.VisibleCenter = VisibleCenter;
        }
    }
}
