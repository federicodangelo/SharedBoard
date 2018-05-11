using MetroLog;
using SharedBoard.Model;
using SharedBoard.Model.Controls;
using SharedBoard.View.Controls;
using SharedBoard.ViewModel;
using SharedBoard.ViewModel.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;

namespace SharedBoard.View
{
    public sealed partial class BoardView : UserControl
    {
        private ILogger log = LogManagerFactory.DefaultLogManager.GetLogger<BoardView>();

        private IBoardControlView selectedControlView;

        private readonly List<IBoardControlView> boardControlViews = new List<IBoardControlView>();

        public Rect Bounds => new Rect(0, 0, ActualWidth, ActualHeight);

        public ScrollViewer ScrollViewer { get; set; }

        public float ZoomFactor => (ScrollViewer?.ZoomFactor).GetValueOrDefault(1.0f);

        public Point VisibleCenter
        {
            get
            {
                var x = Bounds.Width / 2 + (-ScrollViewer?.ScrollableWidth / 2 + ScrollViewer?.HorizontalOffset) / ZoomFactor;
                var y = Bounds.Height / 2 + (-ScrollViewer?.ScrollableHeight / 2 + ScrollViewer?.VerticalOffset) / ZoomFactor;

                return new Point(x.GetValueOrDefault(), y.GetValueOrDefault());
            }
        }

        public BoardViewModel ViewModel { get; }

        public BoardView()
        {
            this.InitializeComponent();

            ViewModel = new BoardViewModel(new Board());

            InitManualBindings();
        }

        public void SetTopBoardControlView(IBoardControlView boardControlView)
        {
            var maxZIndex = boardControlViews.Where(x => x != boardControlView).Aggregate(0, (acc, bc) => Math.Max(acc, Canvas.GetZIndex((Windows.UI.Xaml.UIElement)bc.Control)));

            Canvas.SetZIndex((Windows.UI.Xaml.UIElement)boardControlView.Control, maxZIndex + 1);
            Canvas.SetZIndex(selectedControlTools, maxZIndex + 2);
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
                    if (item is BoardControlViewModel)
                    {
                        AddBoardControlViewModel(item as BoardControlViewModel);
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

        private IBoardControlView AddBoardControlViewModel(BoardControlViewModel boardControlViewModel)
        {
            var boardControlView = BoardControlViewFactory.BuildBoardControlView(boardControlViewModel.BoardControl);

            InitBoardControlView(boardControlView, boardControlViewModel);

            AddBoardControlView(boardControlView);

            SetTopBoardControlView(boardControlView);

            return boardControlView;
        }

        private void InitBoardControlView(IBoardControlView boardControlView, BoardControlViewModel boardControlViewModel)
        {
            boardControlView.Init(this, boardControlViewModel);
        }

        private void AddBoardControlView(IBoardControlView boardControlView)
        {
            mainCanvas.Children.Add((Windows.UI.Xaml.UIElement)boardControlView.Control);

            boardControlViews.Add(boardControlView);
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
            mainCanvas.Children.Remove((Windows.UI.Xaml.UIElement)boardControlView.Control);
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
                var maxZIndex = boardControlViews.Aggregate(0, (acc, bc) => Math.Max(acc, Canvas.GetZIndex((Windows.UI.Xaml.UIElement)bc.Control)));
                Canvas.SetZIndex(selectedControlTools, maxZIndex + 1);
            }
            else
            {
                selectedControlTools.Hide();
            }
        }

        private void Board_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Focus(FocusState.Pointer);
            ViewModel.LastPointerPosition = e.GetPosition(mainCanvas);
            ViewModel.SelectedBoardControlViewModel = null;
        }

        private void Board_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            Focus(FocusState.Pointer);
            ViewModel.LastPointerPosition = e.GetPosition(mainCanvas);
        }

        private void Board_LayoutUpdated(object sender, object e)
        {
            ViewModel.VisibleCenter = VisibleCenter;
        }

        private void OnFileDragOver(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = DataPackageOperation.Copy;

            if (e.DragUIOverride != null)
            {
                e.DragUIOverride.Caption = "Add file";
                e.DragUIOverride.IsContentVisible = true;
            }

            //this.AddFilePanel.Visibility = Visibility.Visible;
        }

        private void OnFileDragLeave(object sender, DragEventArgs e)
        {
            //this.AddFilePanel.Visibility = Visibility.Collapsed;
        }

        private async void OnFileDrop(object sender, DragEventArgs e)
        {
            ViewModel.LastPointerPosition = e.GetPosition(mainCanvas);

            if (e.DataView.Contains(StandardDataFormats.StorageItems))
            {
                var items = await e.DataView.GetStorageItemsAsync();
                if (items.Count > 0)
                {
                    foreach (var file in items.OfType<StorageFile>())// .Select(storageFile => new AppFile { Name = storageFile.Name, File = storageFile }))
                    {
                        try
                        {
                            if (file.FileType.ToLower() == ".txt")
                            {
                                var buffer = await FileIO.ReadBufferAsync(file);

                                using (var reader = DataReader.FromBuffer(buffer))
                                {
                                    byte[] fileContent = new byte[reader.UnconsumedBufferLength];
                                    reader.ReadBytes(fileContent);
                                    string text = Encoding.UTF8.GetString(fileContent, 0, fileContent.Length);

                                    var stickyNoteViewModel = ViewModel.AddStickyNote(ViewModel.LastPointerPosition) as StickyNoteViewModel;

                                    stickyNoteViewModel.Text = text;
                                }
                            }
                            else if (file.FileType.ToLower() == ".jpg" || file.FileType.ToLower() == ".png")
                            {
                                var thumbnail = await file.GetScaledImageAsThumbnailAsync(ThumbnailMode.PicturesView, (uint)BoardImage.DefaultSize.Width);

                                var bitmapImage = new BitmapImage();
                                await bitmapImage.SetSourceAsync(thumbnail);

                                var boardImageViewModel = ViewModel.AddBoardImage(ViewModel.LastPointerPosition) as BoardImageViewModel;

                                boardImageViewModel.Width = Math.Max(bitmapImage.PixelWidth, 100);
                                boardImageViewModel.Height = Math.Max(bitmapImage.PixelHeight, 100);

                                boardImageViewModel.Image = bitmapImage;
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error($"Error loading file {file.Name}", ex);
                        }

                    }
                }
            }

            //this.AddFilePanel.Visibility = Visibility.Collapsed;
        }
    }
}
