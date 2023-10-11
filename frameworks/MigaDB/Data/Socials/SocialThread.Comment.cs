namespace Acorisoft.FutureGL.MigaDB.Data.Socials
{
    public class Comment : StorageUIObject
    {
        private string _content;
        public string SenderID { get; init; }
        
        /// <summary>
        ///
        /// </summary>
        public string CommentID { get; init; }

        /// <summary>
        /// 获取或设置 <see cref="Content"/> 属性。
        /// </summary>
        public string Content
        {
            get => _content;
            set => SetValue(ref _content, value);
        }
    }
}