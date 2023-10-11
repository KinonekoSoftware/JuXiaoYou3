namespace Acorisoft.FutureGL.MigaDB.Data.DataParts
{
    public class PartOfAlbum : PartOfEditableDetail
    {
        public PartOfAlbum()
        {
            Id = Constants.IdOfAlbumPart;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public List<Album> Items { get; init; }
        
    }
}