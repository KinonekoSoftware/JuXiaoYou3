namespace Acorisoft.Miga.Doc
{
    public enum EntityID
    {
        Channel,
        ChannelIndex,
        ComposeIndex,
        DocumentIndex,
        Document,
        Group,
        Keyword,
        Sight,
        KeywordMapping,
        Moniker,
        Organization,
        Relationship,
        Conversation,
        Timeline
    }
    
    public static class Constants
    {
        
        public static void AddMany<T>(this ICollection<T> source, IEnumerable<T> target, bool clear = false)
        {
            if (source is null || target is null)
            {
                return;
            }

            if (source.IsReadOnly)
            {
                return;
            }

            if (clear)
            {
                source.Clear();
            }

            foreach (var item in target)
            {
                source.Add(item);
            }
        }
        
        //
        // fileName
        public const string main_database = "main.mgdb";
        public const string index_file    = "index.migaidx";

        //
        // collection name
        public const string cn_modules          = "mods";
        public const string cn_meow             = "meow";
        public const string cn_prop             = "props";
        public const string cn_index            = "idx";
        public const string cn_index_compose    = "idx_compose";
        public const string cn_index_channel    = "idx_ch";
        public const string cn_document         = "doc";
        public const string cn_channel          = "ch";
        public const string cn_message          = "msgs";
        public const string cn_inspiration      = "ins";
        public const string cn_compose          = "compose";
        public const string cn_keyword          = "tags";
        public const string cn_sight            = "sights";
        public const string cn_keywordMapping   = "rel_tags";
        public const string cn_characterMapping = "rel_ch";
        public const string cn_org              = "org";
        public const string cn_group            = "group";
        public const string cn_moniker          = "monikers";

        //
        // folder name
        public const string folder_modules = "Modules";

        //
        // field
        public const string fieldName_id = "_id";
    }
}