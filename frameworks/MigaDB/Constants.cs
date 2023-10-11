namespace Acorisoft.FutureGL.MigaDB
{
    public class Constants
    {

     /*
            * Database Const Fields : string
            * 数据库常量字段
            */
        internal const string DatabaseFileName      = "main.mgdb";
        internal const string DatabaseIndexFileName = "index.mgidx";


        /*
         * Database Const Fields : Int
         * 数据库常量字段（非字符串）
         */
        internal const int DatabaseSize = 8 * 1048576;
        internal const int MinVersion   = 0;
        internal const int MaxVersion   = 512;


        /*
         * DataPart Fields
         * 数据库引擎字段
         */
        internal const string IdOfAlbumPart                  = "__Album";
        internal const string IdOfPlaylistPart               = "__Playlist";
        internal const string IdOfRelationship_CharacterPart = "__Relationship_Character";
        internal const string IdOfBasicPart                  = "__Basic";
        internal const string IdOfPresentationPart           = "__Presentation";
        internal const string IdOfSurveyPart                 = "__Survey";
        internal const string IdOfPrototypePart              = "__Prototype";
        internal const string IdOfStickyNote                 = "__StickyNote";
        internal const string IdOfSentence                   = "__Sentence";
        internal const string IdOfAppraise                   = "__Appraise";
        internal const string IdOfMarkdownPart               = "__Markdown";
        internal const string IdOfRtfPart                    = "__Rtf";


        /*
         * Database Engine Fields
         * 数据库引擎字段
         */

        //
        // Base
        internal const string PropertyCollectionName = "_props";
        internal const string BookmarkName           = "_bookmark";

        //
        // File
        internal const string Name_FileTable  = "file";
        internal const string Name_MusicTable = "file_music";

        //
        // Universe
        internal const string Name_Elemental    = "elemental";
        internal const string Name_Technology   = "tech";
        internal const string Name_Terminology  = "terminology";
        internal const string Name_Timeline     = "timeline";
        internal const string Name_Appraise     = "appraise";
        internal const string Name_Prototype    = "prototype";
        internal const string Name_Preshape     = "preshape";
        internal const string Name_Sentence     = "sentence";
        internal const string Name_SpaceConcept = "space";
        
        internal const string Name_MeasuringUnit     = "unit";
        internal const string Name_Faith             = "faith";
        internal const string Name_Market            = "market";
        internal const string Name_Chat_Channel      = "social_channel";
        internal const string Name_Chat_ChannelCache = "social_ch";
        internal const string Name_Chat_Character    = "social_character";
        internal const string Name_Chat_Thread       = "social_thread";
        internal const string Name_Chat_Upvote       = "social_upvote";

        //
        // Rel
        internal const string Name_Relationship_SpaceConcept = "rel_space";
        internal const string Name_Relationship_Character    = "rel_ch";
        internal const string Name_Relationship_Technology   = "rel_tech";
        internal const string Name_Relationship_Elemental    = "rel_elemental";

        internal const string Name_Directory      = "dir";
        internal const string Name_Concept        = "concept";
        internal const string Name_Keyword        = "keywords";
        internal const string Name_ModuleTemplate = "module";

        //
        // Cache
        internal const string Name_Cache_ModuleTemplate = "idx_module";
        internal const string Name_CacheMetadata       = "idx_meta";
        internal const string Name_Cache_Document       = "idx_doc";
        internal const string Name_Cache_Compose        = "idx_compose";

        //
        //
        internal const string Name_Document = "doc";
        internal const string Name_Object   = "obj";
        internal const string Name_Compose  = "compose";


        /*
         * Folder Name
         * 文件夹名字
         */
        internal const string ImageFolderName = "Image";
        internal const string MusicFolderName = "Music";
        internal const string AudioFolderName = "Audio";

        /*
         * LiteDB Fields
         * 数据库字段
         */
        public const string LiteDB_IdField   = "_id";
        public const string LiteDB_NameField = "name";
        public const string LiteDB_ParentId  = "pid";

        /*
         * DatabaseVersion
         * 当前数据库版本
         */
        public const int DatabaseMinimumVersion = 1;
        public const int DatabaseCurrentVersion = 1;
    }
}