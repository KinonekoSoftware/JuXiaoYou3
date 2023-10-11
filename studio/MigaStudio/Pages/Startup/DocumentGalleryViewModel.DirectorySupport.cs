using System.Linq;
using Acorisoft.FutureGL.MigaDB.Data.Keywords;
using Acorisoft.FutureGL.MigaUtils.Collections;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    partial class DocumentGalleryViewModel
    {
        private bool               _isDirectoryPaneOpen;
        private DirectorySupportUI _selectedDirectory;
        private DirectoryRootUI    _selectedRootDirectory;

        private void OpenDirectoryPaneImpl()
        {
            IsDirectoryPaneOpen = true;
        }

        private void ClearDirectoryModeImpl()
        {
            _selectedDirectory     = null;
            _selectedRootDirectory = null;
            _isDirectoryPaneOpen   = false;
            _isPropertyPaneOpen   = false;
            Directories.Clear();
            RaiseUpdated(nameof(IsPropertyPaneOpen));
            RaiseUpdated(nameof(IsDirectoryPaneOpen));
            RaiseUpdated(nameof(SelectedDirectory));
            RaiseUpdated(nameof(SelectedRootDirectory));
            ForceInvalidateDataSource();
        }
        
        private void Initialize()
        {
            Root.AddMany(KeywordEngine.GetDirectoryRoot()
                                      .Select(x => new DirectoryRootUI
                                      {
                                          Source   = (DirectoryRoot)x,
                                          Children = new ObservableCollection<DirectorySupportUI>()
                                      }), true);
        }
        
        private void InitializeWhenResume()
        {

            if (Version != KeywordEngine.Version)
            {
                Version = KeywordEngine.Version;
                Root.AddMany(KeywordEngine.GetDirectoryRoot()
                                          .Select(x => new DirectoryRootUI
                                          {
                                              Source   = (DirectoryRoot)x,
                                              Children = new ObservableCollection<DirectorySupportUI>()
                                          }), true);
            }
        }

        private void SetRootDirectory(DirectoryRootUI node)
        {
            
            //
            // 构建目录结构
            KeywordViewModel.CreateSubTree(KeywordEngine, Directories, node.Id);
            
            //
            // 设置当前文档
            SetDirectory(node);
        }

        private void SetDirectory(DirectorySupportUI node)
        {
            if (node is null)
            {
                Directories.Clear();
                return;
            }
            
            var dataSource = KeywordEngine.GetDirectoryMapping(DocumentEngine, Type, node.Name);
            
            //
            // 添加数据源
            DataSource.AddMany(dataSource, true);
            
            //
            // 重新计算
            OnRequestComputePageCount(DataSource);
            
            // 跳转页面
            JumpPage(PageIndex);
        }
        
        
        public ObservableCollection<DirectoryRootUI> Root { get; }
        public ObservableCollection<DirectorySupportUI> Directories { get; }
        
        

        /// <summary>
        /// 获取或设置 <see cref="SelectedRootDirectory"/> 属性。
        /// </summary>
        public DirectoryRootUI SelectedRootDirectory
        {
            get => _selectedRootDirectory;
            set
            {
                SetValue(ref _selectedRootDirectory, value);
                SetRootDirectory(value);
            }
        }

        /// <summary>
        /// 获取或设置 <see cref="SelectedDirectory"/> 属性。
        /// </summary>
        public DirectorySupportUI SelectedDirectory
        {
            get => _selectedDirectory;
            set
            {
                SetValue(ref _selectedDirectory, value);
                SetDirectory(value);
            }
        }

        /// <summary>
        /// 获取或设置 <see cref="IsDirectoryPaneOpen"/> 属性。
        /// </summary>
        public bool IsDirectoryPaneOpen
        {
            get => _isDirectoryPaneOpen;
            set
            {
                SetValue(ref _isDirectoryPaneOpen, value);
            }
        }
        
        public RelayCommand OpenDirectoryPaneCommand { get; }
        public RelayCommand ClearDirectoryModeCommand { get; }
    }
}