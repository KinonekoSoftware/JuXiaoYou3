namespace Acorisoft.FutureGL.MigaDB.Data.DataParts
{
    public class PartOfPlaylist : PartOfEditableDetail
    {
        public PartOfPlaylist()
        {
            Id = Constants.IdOfPlaylistPart;
        }
        
        public bool AutoPlay { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public List<Music> Items { get; init; }
    }
}