using SharedBoard.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SharedBoard
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private const float StartingZoom = 0.75f;

        public MainPage()
        {
            this.InitializeComponent();
            board.ScrollViewer = scrollViewer;
        }

        private void MainCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            ZoomToCenter(StartingZoom);
        }
        
        private void CreateStickyNote_Click(object sender, RoutedEventArgs e)
        {
            var scrollCenter = board.VisibleCenter;
            
            board.AddStickyNote(new Point(scrollCenter.X - StickyNote.DefaultSize.Width / 2, scrollCenter.Y - StickyNote.DefaultSize.Height / 2)).StartEdit(true);
        }

        private void ZoomIn_Click(object sender, RoutedEventArgs e)
        {
            ZoomTo(scrollViewer.ZoomFactor * 1.5f);
        }

        private void ZoomOut_Click(object sender, RoutedEventArgs e)
        {
            ZoomTo(scrollViewer.ZoomFactor * 0.75f);
        }

        private void ZoomToCenter(float zoom)
        {
            if (zoom > scrollViewer.MaxZoomFactor)
                zoom = scrollViewer.MaxZoomFactor;
            else if (zoom < scrollViewer.MinZoomFactor)
                zoom = scrollViewer.MinZoomFactor;

            var contentWidth = scrollViewer.ExtentWidth / scrollViewer.ZoomFactor;
            var contentHeight = scrollViewer.ExtentHeight / scrollViewer.ZoomFactor;

            var currentX = contentWidth / 2;
            var currentY = contentHeight / 2;

            var newScrollableWidth = Math.Max(contentWidth * zoom - scrollViewer.ViewportWidth, 0f);
            var newScrollableHeight = Math.Max(contentHeight * zoom - scrollViewer.ViewportHeight, 0f);

            var newHorizontalOffsetX = (currentX - contentWidth / 2) * zoom + newScrollableWidth / 2;
            var newHorizontalOffsetY = (currentY - contentHeight / 2) * zoom + newScrollableHeight / 2;

            scrollViewer.ChangeView(newHorizontalOffsetX, newHorizontalOffsetY, zoom);
        }

        private void ZoomTo(float zoom)
        {
            if (zoom > scrollViewer.MaxZoomFactor)
                zoom = scrollViewer.MaxZoomFactor;
            else if (zoom < scrollViewer.MinZoomFactor)
                zoom = scrollViewer.MinZoomFactor;

            var contentWidth = scrollViewer.ExtentWidth / scrollViewer.ZoomFactor;
            var contentHeight = scrollViewer.ExtentHeight / scrollViewer.ZoomFactor;

            var currentX = contentWidth / 2 + (-scrollViewer.ScrollableWidth / 2 + scrollViewer.HorizontalOffset) / scrollViewer.ZoomFactor;
            var currentY = contentHeight / 2 + (-scrollViewer.ScrollableHeight / 2 + scrollViewer.VerticalOffset) / scrollViewer.ZoomFactor;

            var newScrollableWidth = Math.Max(contentWidth * zoom - scrollViewer.ViewportWidth, 0f);
            var newScrollableHeight = Math.Max(contentHeight * zoom - scrollViewer.ViewportHeight, 0f);

            var newHorizontalOffsetX = (currentX - contentWidth / 2) * zoom + newScrollableWidth / 2;
            var newHorizontalOffsetY = (currentY - contentHeight / 2) * zoom + newScrollableHeight / 2;

            scrollViewer.ChangeView(newHorizontalOffsetX, newHorizontalOffsetY, zoom);
        }
    }
}
