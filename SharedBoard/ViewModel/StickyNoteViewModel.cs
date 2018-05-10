using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedBoard.Model;

namespace SharedBoard.ViewModel
{
    public class StickyNoteViewModel : BoardControlViewModel
    {
        public StickyNote StickyNote { get; }

        public string Text
        {
            get => StickyNote.Text;
            set => SetProperty(StickyNote.Text, value, (v) => StickyNote.Text = v);
        }

        public StickyNoteViewModel(StickyNote stickyNote) : base(stickyNote)
        {
            this.StickyNote = stickyNote;
        }
    }
}
