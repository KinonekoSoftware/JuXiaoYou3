namespace Acorisoft.FutureGL.MigaDB.Data.FantasyProjects
{
    public abstract class TimelineConcept : StorageUIObject
    {
        public const int    Seed = 100000;
        private      string _name;
        private      string _intro;
        
       public string LastItem { get; set; }
       public string NextItem { get; set; }

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