using System.Collections.ObjectModel;
using Acorisoft.FutureGL.MigaDB.Services;

namespace Acorisoft.FutureGL.MigaStudio.Models
{
    public class Playlist : ForestObject
    {
        private string _name;

        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetValue(ref _name, value);
        }
        
        /// <summary>
        /// 类型
        /// </summary>
        public ObservableCollection<Music> Items { get; init; }
    }
}