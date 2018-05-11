using SharedBoard.Model.Controls;
using System;

namespace SharedBoard.View.Controls
{
    static public class BoardControlViewFactory
    {
        static public IBoardControlView BuildBoardControlView(BoardControl boardControl)
        {
            if (boardControl is StickyNote)
                return new StickyNoteView();

            if (boardControl is BoardImage)
                return new BoardImageView();

            throw new Exception($"Unknown board control type {boardControl.GetType().Name}");
        }
    }
}
