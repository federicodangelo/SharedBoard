using SharedBoard.Model.Controls;
using System;

namespace SharedBoard.ViewModel.Controls
{
    static class BoardControlViewModelFactory
    {
        static public BoardControlViewModel BuildBoardControlViewModel(BoardControl boardControl)
        {
            if (boardControl is StickyNote)
                return new StickyNoteViewModel(boardControl as StickyNote);

            throw new Exception($"Unknown board control type {boardControl.GetType().Name}");
        }
    }
}
