namespace Acorisoft.FutureGL.MigaDB.Data.Templates.Presentations
{
    public class PresentationRateData : ObservableObject, IPresentationData
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; init; }
        public double Scale { get; init; }
        public string ValueSourceID { get; init; }
        public bool IsMetadata { get; init; }
    }
    
    public class PresentationLikabilityData : ObservableObject, IPresentationData
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; init; }
        public double Scale { get; init; }
        public string ValueSourceID { get; init; }
        public bool IsMetadata { get; init; }
    }
}