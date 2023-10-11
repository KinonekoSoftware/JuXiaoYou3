using Acorisoft.FutureGL.MigaUtils;

namespace Acorisoft.FutureGL.MigaStudio.Models
{
    public class RepositorySetting : ObservableObject
    {
        private string _lastRepository;

        /// <summary>
        /// 所有添加的世界古纳
        /// </summary>
        public ObservableCollection<RepositoryCache> Repositories { get; init; }

        /// <summary>
        /// 最后一次打开的世界观
        /// </summary>
        public string LastRepository
        {
            get => _lastRepository;
            set => SetValue(ref _lastRepository, value);
        }
    }
}