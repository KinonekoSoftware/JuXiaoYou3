namespace Acorisoft.FutureGL.MigaDB.Data.Metadatas
{
    public enum MetadataKind : int
    {
        /// <summary>
        /// 独立的
        /// </summary>
        String,
        Service,
        
        /// <summary>
        /// 在分组里面的
        /// </summary>
        Text,
        Color,
        Degree,
        Heart,
        Star,
        Switch,
        Progress,
        Likability,
        Rate,
        Rarity,
        RadarChart,
        HistogramChart,
        File,
        Music,
        Image,
        Audio,
        Video,
        Reference
    }
}