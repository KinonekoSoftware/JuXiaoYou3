namespace Acorisoft.FutureGL.MigaStudio.Resources.Updaters
{
    //
    // 升级任务
    public class RepositoryProperty
    {
        public string IndexId { get; set; }

        /// <summary>
        /// 获取或设置 <see cref="Avatar"/> 属性。
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// 获取或设置 <see cref="Summary"/> 属性。
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// 获取或设置 <see cref="EnglishName"/> 属性。
        /// </summary>
        public string EnglishName { get; set; }

        public string Author { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public ObservableCollection<string> Backgrounds { get; set; }
    }
}