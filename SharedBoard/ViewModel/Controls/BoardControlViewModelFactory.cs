using SharedBoard.Model.Controls;
using System;

namespace SharedBoard.ViewModel.Controls
{
    static class BoardControlViewModelFactory
    {
        static public BoardControlViewModel BuildBoardControlViewModel(BoardControl boardControl, BoardViewModel boardViewModel)
        {
            if (boardControl is StickyNote)
                return new StickyNoteViewModel(boardControl as StickyNote, boardViewModel);

            if (boardControl is BoardImage)
                return new BoardImageViewModel(boardControl as BoardImage, boardViewModel);

            throw new Exception($"Unknown board control type {boardControl.GetType().Name}");
        }
    }
}
