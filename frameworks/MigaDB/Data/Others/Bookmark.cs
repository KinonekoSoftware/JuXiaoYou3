namespace Acorisoft.FutureGL.MigaDB.Data.Others
{
    public class Bookmark : StorageUIObject
    {
        private string _title;
        private string _link;
        private string _tag;
        

        /// <summary>
        /// 获取或设置 <see cref="Tag"/> 属性。
        /// </summary>
        public string Tag
        {
            get => _tag;
            set => SetValue(ref _tag, value);
        }
        
        /// <summary>
        /// 链接
        /// </summary>
        public string Link
        {
            get => _link;
            set => SetValue(ref _link, value);
        }
        
        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get => _title;
            set => SetValue(ref _title, value);
        }
    }
}