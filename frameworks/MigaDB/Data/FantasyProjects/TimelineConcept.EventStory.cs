namespace Acorisoft.FutureGL.MigaDB.Data.FantasyProjects
{
    public abstract class EventElement : ObservableObject
    {
        
    }

    /// <summary>
    /// 事件描述
    /// </summary>
    public sealed class EventDescription : EventElement
    {
        private string _name;
        private string _intro;

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