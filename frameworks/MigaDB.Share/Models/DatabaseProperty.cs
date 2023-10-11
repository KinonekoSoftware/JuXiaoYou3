using System.Collections.ObjectModel;

namespace Acorisoft.FutureGL.MigaDB.Models
{
    public class DatabaseProperty : StorageObject
    {
        public static bool IsValid(DatabaseProperty property) => 
            property is not null &&
            !string.IsNullOrEmpty(property.Author) &&
            !string.IsNullOrEmpty(property.Name) &&
            !string.IsNullOrEmpty(property.ForeignName);
        
        /// <summary>
        /// 世界观名称
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 世界观外文名（拼音或者英文）
        /// </summary>
        public string ForeignName { get; set; }
        
        /// <summary>
        /// 作者名称
        /// </summary>
        public string Author { get; set; }
        
        /// <summary>
        /// 介绍
        /// </summary>
        public string Intro { get; set; }
        
        /// <summary>
        /// 封面
        /// </summary>
        public string Cover { get; set; }
        
        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }
        
        public ObservableCollection<Album> Album { get; set; }
    }
}