using System.Collections.ObjectModel;

namespace Acorisoft.Miga.Doc.Channels
{
    public class ChannelPart : FixedDataPart
    {
        public ObservableCollection<ChannelIndex> ChannelIndices { get; set; }
    }
}