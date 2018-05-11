using SharedBoard.Model.Controls;
using System.Collections.Generic;

namespace SharedBoard.Model
{
    public class Board
    {
        private readonly List<BoardControl> boardControls = new List<BoardControl>();

        public double Width { get; set; } = 9000;
        public double Height { get; set; } = 6000;

        public IReadOnlyCollection<BoardControl> Controls => boardControls.AsReadOnly();
        
        public void AddBoardControl(BoardControl boardControl)
        {
            boardControls.Add(boardControl);
        }

        public void RemoveBoardControl(BoardControl boardControl)
        {
            boardControls.Remove(boardControl);
        }
    }
}
