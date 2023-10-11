using System.Collections.ObjectModel;

namespace Acorisoft.FutureGL.MigaDB.Data.Templates.Modules
{
    public class ModulePreset : ObservableObject
    {
        private string _name;
        private DocumentType _type;
        
        [BsonId]
        public string Id { get; init; }

        /// <summary>
        /// 获取或设置 <see cref="Type"/> 属性。
        /// </summary>
        public DocumentType Type
        {
            get => _type;
            set => SetValue(ref _type, value);
        }
        
        /// <summary>
        /// 清单
        /// </summary>
        public ObservableCollection<ModuleTemplateCache> Templates { get; init; }

        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetValue(ref _name, value);
        }

        public sealed override string ToString() => Name;
    }
}