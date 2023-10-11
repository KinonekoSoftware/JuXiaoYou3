namespace Acorisoft.FutureGL.MigaDB.Data.DataParts
{
    public class PartOfStickyNote : PartOfEditableDetail
    {
        public PartOfStickyNote()
        {
            Id = Constants.IdOfStickyNote;
        }

        public List<StickyNote> Items { get; set; }
    }

    public class StickyNote : StorageUIObject
    {
        private string _title;
        private string _content;
        private string _intro;
        private DateTime _timeOfModified;
        
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime TimeOfCreated { get; init; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime TimeOfModified
        {
            get => _timeOfModified;
            set => SetValue(ref _timeOfModified, value);
        }

        /// <summary>
        /// 简介
        /// </summary>
        public string Intro
        {
            get => _intro;
            set => SetValue(ref _intro, value);
        }
        
        /// <summary>
        /// 内容
        /// </summary>
        public string Content
        {
            get => _content;
            set => SetValue(ref _content, value);
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