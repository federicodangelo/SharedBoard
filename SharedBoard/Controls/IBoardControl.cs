using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedBoard.Controls
{
    public interface IBoardControl
    {
        void StopEdit();

        void StartEdit();
    }
}
