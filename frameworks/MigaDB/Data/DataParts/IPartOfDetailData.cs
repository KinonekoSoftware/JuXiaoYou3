namespace Acorisoft.FutureGL.MigaDB.Data.DataParts
{
    public interface IPartOfDetailData
    {
        string Key { get; }
    }

    public class PartOfDetailStringData : IPartOfDetailData
    {
        public string Key { get; init; }
        public string Value { get; set; }
    }
    
    
    public class PartOfDetailListData : IPartOfDetailData
    {
        public string Key { get; init; }
        public List<string> Items { get; set; }
    }
    
    
    public class PartOfDetailObjectData : IPartOfDetailData
    {
        public string Key { get; init; }
        public Dictionary<string , string> Buckets { get; set; }
    }
}