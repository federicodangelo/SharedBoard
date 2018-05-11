using SharedBoard.Model;
using SharedBoard.Model.Controls;
using SharedBoard.ViewModel.Controls;
using SharedBoard.ViewModel.Utils;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Windows.Foundation;

namespace SharedBoard.ViewModel
{
    public class BoardViewModel : BindableBase
    {
        private Point _lastPointerPosition;
        private Point _visibleCenter;
        private BoardControlViewModel _selectedBoardControlViewModel;

        public Board Board { get; }

        public ObservableCollection<BoardControlViewModel> BoardControls { get; }
        
        public double Width
        {
            get => Board.Width;
            set => SetProperty(Board.Width, value, (v) => Board.Width = v);
        }

        public double Height
        {
            get => Board.Height;
            set => SetProperty(Board.Height, value, (v) => Board.Height = v);
        }

        public Point LastPointerPosition
        {
            get => _lastPointerPosition;
            set => SetProperty(ref _lastPointerPosition, value);
        }

        public Point VisibleCenter
        {
            get => _visibleCenter;
            set => SetProperty(ref _visibleCenter, value);
        }

        public BoardControlViewModel SelectedBoardControlViewModel
        {
            get => _selectedBoardControlViewModel;
            set => SetProperty(ref _selectedBoardControlViewModel, value);
        }

        public ICommand AddStickyNoteCommand => new DelegateCommand(() => SelectedBoardControlViewModel = AddStickyNote(LastPointerPosition));

        public ICommand AddStickyNoteToVisibleCenterCommand => new DelegateCommand(() => SelectedBoardControlViewModel = AddStickyNote(VisibleCenter));

        public ICommand AddBoardImageCommand => new DelegateCommand(() => SelectedBoardControlViewModel = AddBoardImage(LastPointerPosition));

        public ICommand AddBoardImageToVisibleCenterCommand => new DelegateCommand(() => SelectedBoardControlViewModel = AddBoardImage(VisibleCenter));

        public ICommand RemoveSelectedBoardControlCommand => new DelegateCommand(() => RemoveBoardControl(SelectedBoardControlViewModel));

        public BoardViewModel(Board board)
        {
            Board = board;
            BoardControls = new ObservableCollection<BoardControlViewModel>(Board.Controls.Select(x => BoardControlViewModelFactory.BuildBoardControlViewModel(x, this)));
        }

        private BoardControlViewModel AddBoardImage(Point position)
        {
            var boardImage = new BoardImage
            {
                Position = new Point(position.X - BoardImage.DefaultSize.Width / 2, position.Y - BoardImage.DefaultSize.Height / 2),
                Size = BoardImage.DefaultSize
            };

            return AddBoardControl(boardImage);
        }

        private BoardControlViewModel AddStickyNote(Point position)
        {
            var stickyNote = new StickyNote
            {
                Position = new Point(position.X - StickyNote.DefaultSize.Width / 2, position.Y - StickyNote.DefaultSize.Height / 2),
                Size = StickyNote.DefaultSize
            };

            return AddBoardControl(stickyNote);
        }

        private BoardControlViewModel AddBoardControl(BoardControl boardControl)
        {
            var boardControlViewModel = BoardControlViewModelFactory.BuildBoardControlViewModel(boardControl, this);

            Board.AddBoardControl(boardControl);
            BoardControls.Add(boardControlViewModel);
            OnPropertyChanged(nameof(BoardControls));

            return boardControlViewModel;
        }

        private void RemoveBoardControl(BoardControlViewModel boardControlViewModel)
        {
            if (SelectedBoardControlViewModel == boardControlViewModel)
                SelectedBoardControlViewModel = null;

            Board.RemoveBoardControl(boardControlViewModel.BoardControl);
            BoardControls.Remove(boardControlViewModel);
            OnPropertyChanged(nameof(BoardControls));
        }
    }
}
