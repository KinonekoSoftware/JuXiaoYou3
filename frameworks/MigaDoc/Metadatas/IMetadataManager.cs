namespace Acorisoft.Miga.Doc.Metadatas
{
    public interface IMetadataManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="metadata"></param>
        void Set(Metadata metadata);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="metaName"></param>
        void Unset(string metaName);
    }
    
    public class CustomMetadataView
    {
        public string Metadata { get; init; }
        public string Color { get; init; }
        public string Title { get; init; }
    }
}