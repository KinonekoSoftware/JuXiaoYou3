using Acorisoft.FutureGL.Forest.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Views
{
    [Connected(View = typeof(LaunchView), ViewModel = typeof(LaunchViewController))]
    public partial class LaunchView:ForestUserControl 
    {
        public LaunchView()
        {
            InitializeComponent();
        }
    }
}