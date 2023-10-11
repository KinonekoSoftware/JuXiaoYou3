using System.Collections.ObjectModel;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Presentations;

namespace Acorisoft.FutureGL.MigaDB.Data.DataParts
{
    public class PartOfPresentation : PartOfManifest
    {
        public PartOfPresentation()
        {
            Id = Constants.IdOfPresentationPart;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<Presentation> Blocks { get; init; }
    }
}