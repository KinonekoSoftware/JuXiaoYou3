using System.Collections.ObjectModel;
using Acorisoft.Miga.Doc.Channels;

namespace Acorisoft.Miga.Doc.Entities
{
    public class InspirationPart : FixedDataPart
    {
        public ObservableCollection<ChannelIndex> Channels { get; set; }
    }
}