using System.Collections.ObjectModel;

namespace Acorisoft.FutureGL.MigaDB.Data.Socials
{
    public class SocialThread : StorageUIObject
    {
        private string _content;
        private string _timestamp;
        private string _location;
        private int _viewCount;

        /// <summary>
        /// 浏览次数
        /// </summary>
        public int ViewCount
        {
            get => _viewCount;
            set => SetValue(ref _viewCount, value);
        }
        
        /// <summary>
        /// 发帖人
        /// </summary>
        public string SenderID { get; init; }

        /// <summary>
        /// 时间，例如：昨天 19:31
        /// </summary>
        public string Timestamp
        {
            get => _timestamp;
            set => SetValue(ref _timestamp, value);
        }
        
        /// <summary>
        /// 属地，例如：北京、厕所
        /// </summary>
        public string Location
        {
            get => _location;
            set => SetValue(ref _location, value);
        }
        
        /// <summary>
        /// 获取或设置 <see cref="Content"/> 属性。
        /// </summary>
        public string Content
        {
            get => _content;
            set => SetValue(ref _content, value);
        }
        
        public ObservableCollection<Comment> Comments { get; init; }
    }
}