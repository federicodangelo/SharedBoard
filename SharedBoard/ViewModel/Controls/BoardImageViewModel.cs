using SharedBoard.Model.Controls;

namespace SharedBoard.ViewModel.Controls
{
    public class BoardImageViewModel : BoardControlViewModel
    {
        public BoardImage BoardImage => BoardControl as BoardImage;

        public string ImageURL
        {
            get => BoardImage.ImageUrl;
            set => SetProperty(BoardImage.ImageUrl, value, (v) => BoardImage.ImageUrl= v);
        }

        public BoardImageViewModel(BoardImage boardControl, BoardViewModel boardViewModel) : base(boardControl, boardViewModel)
        {
        }
    }
}
