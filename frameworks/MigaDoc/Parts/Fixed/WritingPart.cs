using System.Collections.ObjectModel;

namespace Acorisoft.Miga.Doc.Parts
{
    public class WritingPart : FixedDataPart
    {
        public ObservableCollection<StickyNote> Composes { get; set; }
    }
}