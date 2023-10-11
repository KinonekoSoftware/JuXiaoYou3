using System.Collections.ObjectModel;
using Acorisoft.FutureGL.MigaStudio.ViewModels;

namespace Acorisoft.FutureGL.Demo.ViewHost.ViewModels
{
    [Name("列表测试")]
    public class ForestTestViewModel : TabViewModel
    {
        public ForestTestViewModel()
        {
            Collection = new ObservableCollection<int>(MainWindow.Array);
        }
        
        public ObservableCollection<int> Collection { get; }
    }
}