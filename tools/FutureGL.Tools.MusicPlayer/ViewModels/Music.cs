using Acorisoft.FutureGL.Forest;

namespace Acorisoft.FutureGL.Tools.MusicPlayer
{
    public class Music : ForestObject
    {
        private string _name;
        private string _author;
        private string _cover;
        
        /// <summary>
        /// 唯一标识符
        /// </summary>
        public string Id { get; init; }

        /// <summary>
        /// 获取或设置 <see cref="Cover"/> 属性。
        /// </summary>
        public string Cover
        {
            get => _cover;
            set => SetValue(ref _cover, value);
        }
        
        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; init; }

        /// <summary>
        /// 获取或设置 <see cref="Author"/> 属性。
        /// </summary>
        public string Author
        {
            get => _author;
            set => SetValue(ref _author, value);
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