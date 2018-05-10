using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace SharedBoard.Controls
{
    public interface IBoardControl
    {
        void StopEdit();

        void StartEdit();

        bool Selected { get; set; }

        Rect Bounds { get; }
    }
}
