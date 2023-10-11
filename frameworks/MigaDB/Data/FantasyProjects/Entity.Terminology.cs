using System.Collections.ObjectModel;

namespace Acorisoft.FutureGL.MigaDB.Data.FantasyProjects
{
    public class Terminology : StorageUIObject
    {
        private string _name;
        private string _intro;

        public ObservableCollection<RichContentElement> ContentElements { get; init; }

        /// <summary>
        /// 获取或设置 <see cref="Intro"/> 属性。
        /// </summary>
        public string Intro
        {
            get => _intro;
            set => SetValue(ref _intro, value);
        }
        
        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetValue(ref _name, value);
        }
    }
}