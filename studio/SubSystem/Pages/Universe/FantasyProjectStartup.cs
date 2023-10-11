using System.Linq;
using System.Windows;
using Acorisoft.FutureGL.MigaDB.Data.FantasyProjects;
using Acorisoft.FutureGL.MigaStudio.ViewModels.FantasyProject;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Universe
{
    public partial class FantasyProjectStartupViewModel : TabViewModel
    {
        private ProjectItem      _selectedItem;
        private FrameworkElement _selectedView;
        
        
        public FantasyProjectStartupViewModel()
        {
            ProjectElements  = new ObservableCollection<ProjectItem>();
            DocumentElements = new ObservableCollection<ProjectItem>();
            OtherElements    = new ObservableCollection<ProjectItem>();

            ProjectEngine = Studio.Engine<ProjectEngine>();
            
            CreateProjectItems();
        }

        protected override void OnStart()
        {
            //
            // 默认
            SelectedItem = ProjectElements.First();
            
            //
            // 
            Aggregate();
            
            //
            //
            base.OnStart();
        }

        private void Aggregate()
        {
            
        }

        /// <summary>
        /// 获取或设置 <see cref="SelectedItem"/> 属性。
        /// </summary>
        public ProjectItem SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetValue(ref _selectedItem, value);

                if (value is not null)
                {
                    var expr = _selectedItem.Expression ?? DefaultCreateViewExpr;
                    SelectedView = _selectedItem.View ?? expr(_selectedItem);
                }
            }
        }

        /// <summary>
        /// 获取或设置 <see cref="SelectedView"/> 属性。
        /// </summary>
        public FrameworkElement SelectedView
        {
            get => _selectedView;
            private set => SetValue(ref _selectedView, value);
        }
        
        public ObservableCollection<ProjectItem> ProjectElements { get; }
        public ObservableCollection<ProjectItem> DocumentElements { get; }
        public ObservableCollection<ProjectItem> OtherElements { get; }
        public ProjectEngine ProjectEngine { get; }
    }
}