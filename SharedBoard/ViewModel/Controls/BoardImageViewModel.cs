using SharedBoard.Model.Controls;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace SharedBoard.ViewModel.Controls
{
    public class BoardImageViewModel : BoardControlViewModel
    {
        public BoardImage BoardImage => BoardControl as BoardImage;
        private BitmapImage finalImage_;

        public string ImageURL
        {
            get => BoardImage.ImageUrl;
            set
            {
                if (SetProperty(BoardImage.ImageUrl, value, (v) => BoardImage.ImageUrl = v))
                {
                    FinalImage = null;
                    LoadImageAsync();
                }
            }
        }

        public BitmapImage Image
        {
            get => BoardImage.Image;
            set
            {
                if (SetProperty(BoardImage.Image, value, (v) => BoardImage.Image = v))
                {
                    FinalImage = null;
                    OnPropertyChanged(nameof(FinalImage));
                }
            }
        }

        public BitmapImage FinalImage
        {
            get
            {
                LoadImageAsync();
                return finalImage_;
            }
            private set
            {
                SetProperty(ref finalImage_, value);
            }
        }

        private void LoadImageAsync()
        {
            if (finalImage_ != null)
                return;

            if (Image != null)
            {
                FinalImage = Image;
            }
            else
            {
                FinalImage = new BitmapImage(new Uri(ImageURL, UriKind.Absolute));
            }
        }

        public BoardImageViewModel(BoardImage boardControl, BoardViewModel boardViewModel) : base(boardControl, boardViewModel)
        {
        }
    }
}
