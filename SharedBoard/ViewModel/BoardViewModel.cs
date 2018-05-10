using SharedBoard.Model;
using SharedBoard.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Foundation;

namespace SharedBoard.ViewModel
{
    public class BoardViewModel : BindableBase
    {
        public Board Board { get; }

        public ObservableCollection<BoardControlViewModel> BoardControls { get; }

        public Point LastPointerPosition { get; set; }

        public Point VisibleCenter { get; set; }

        private BoardControlViewModel selectedBoardControlViewModel;

        public BoardViewModel(Board board)
        {
            Board = board;
            BoardControls = new ObservableCollection<BoardControlViewModel>(Board.Controls.Select(x => BuildBoardControlViewModel(x)));
        }

        private BoardControlViewModel BuildBoardControlViewModel(BoardControl boardControl)
        {
            if (boardControl is StickyNote)
                return new StickyNoteViewModel(boardControl as StickyNote);

            return new BoardControlViewModel(boardControl);
        }

        public BoardControlViewModel SelectedBoardControlViewModel
        {
            get
            {
                return selectedBoardControlViewModel;
            }
            set
            {
                SetProperty(ref selectedBoardControlViewModel, value);
            }
        }

        public ICommand CreateAddStickyNoteCommand
        {
            get
            {
                return new DelegateCommand(() =>
                    {
                        SelectedBoardControlViewModel = AddStickyNote(LastPointerPosition);
                    }
                );
            }
        }

        public ICommand CreateAddStickyNoteToVisibleCenterCommand
        {
            get
            {
                return new DelegateCommand(() =>
                    {
                        SelectedBoardControlViewModel = AddStickyNote(VisibleCenter);
                    }
                );
            }
        }

        public ICommand CreateRemoveSelectedStickyNoteCommand
        {
            get
            {
                return new DelegateCommand(() =>
                    {
                        if (SelectedBoardControlViewModel != null)
                            RemoveBoardControl(SelectedBoardControlViewModel);
                    }
                );
            }
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
            var boardControlViewModel = BuildBoardControlViewModel(boardControl);

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
