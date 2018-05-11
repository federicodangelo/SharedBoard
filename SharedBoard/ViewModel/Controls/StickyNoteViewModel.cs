using SharedBoard.Model.Controls;

namespace SharedBoard.ViewModel.Controls
{
    public class StickyNoteViewModel : BoardControlViewModel
    {
        public StickyNote StickyNote => BoardControl as StickyNote;

        public string Text
        {
            get => StickyNote.Text;
            set => SetProperty(StickyNote.Text, value, (v) => StickyNote.Text = v);
        }

        public StickyNoteViewModel(StickyNote boardControl, BoardViewModel boardViewModel) : base(boardControl, boardViewModel)
        {
        }
    }
}
