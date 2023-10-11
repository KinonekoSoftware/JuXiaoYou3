using System.Windows.Controls;
using Acorisoft.FutureGL.Forest.Controls;

namespace ViewHost.Views
{
    [Connected(View = typeof(ModuleBlockView), ViewModel = typeof(ModuleBlockViewModel))]
    public partial class ModuleBlockView:ForestUserControl 
    {
        public ModuleBlockView()
        {
            InitializeComponent();
        }
    }
}