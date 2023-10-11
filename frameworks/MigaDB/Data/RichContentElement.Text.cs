namespace Acorisoft.FutureGL.MigaDB.Data
{
    public class TextContentElement : RichContentElement
    {
        private string _header;
        private string _content;

        /// <summary>
        /// 获取或设置 <see cref="Content"/> 属性。
        /// </summary>
        public string Content
        {
            get => _content;
            set => SetValue(ref _content, value);
        }
        
        /// <summary>
        /// 获取或设置 <see cref="Header"/> 属性。
        /// </summary>
        public string Header
        {
            get => _header;
            set => SetValue(ref _header, value);
        }
    }
}