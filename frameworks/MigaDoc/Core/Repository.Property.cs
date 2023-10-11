using System.Collections.ObjectModel;

namespace Acorisoft.Miga.Doc.Core
{
    [AttributeUsage(AttributeTargets.Class)]
    public class GeneratedModulesAttribute : Attribute
    {
        
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class LazyAttribute : Attribute
    {
        
    }
    
    public class RepositoryProperty : Document
    {
        public string IndexId { get; set; }
        
        /// <summary>
        /// 获取或设置 <see cref="Avatar"/> 属性。
        /// </summary>
        public string Avatar{ get; set; }

        /// <summary>
        /// 获取或设置 <see cref="Summary"/> 属性。
        /// </summary>
        public string Summary{ get; set; }
        /// <summary>
        /// 获取或设置 <see cref="EnglishName"/> 属性。
        /// </summary>
        public string EnglishName{ get; set; }
        
        public string Author { get; set; }
        public string Email { get; set; }
        public ObservableCollection<string> Backgrounds { get; set; }
    }

    public class RepositoryConfiguration 
    {
        
        /// <summary>
        /// 获取或设置 <see cref="EnableCustomMetadataView"/> 属性。
        /// </summary>
        public bool EnableCustomMetadataView{ get; set; }

        public ObservableCollection<CustomMetadataView> Character { get; set; }
        public ObservableCollection<CustomMetadataView> Skills { get; set; }
        public ObservableCollection<CustomMetadataView> Assets { get; set; }
        public ObservableCollection<CustomMetadataView> Materials { get; set; }
        public ObservableCollection<CustomMetadataView> Maps { get; set; }
        public ObservableCollection<CustomMetadataView> Custom { get; set; }
    }

    public class RepositoryAuthor 
    {

        
        private string _avatar;

        /// <summary>
        /// 获取或设置 <see cref="Avatar"/> 属性。
        /// </summary>
        public string Avatar{ get; set; }
        
        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Name{ get; set; }
    }
    
    

    public class RepositoryInformation
    {
        public int Version { get; init; }
    }

    public class UserCredential
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
    }

}