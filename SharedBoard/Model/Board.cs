using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedBoard.Model
{
    public class Board
    {
        private readonly List<BoardControl> boardControls = new List<BoardControl>();

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
