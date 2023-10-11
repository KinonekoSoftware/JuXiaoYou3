using System.Windows.Controls;

namespace Acorisoft.FutureGL.Demo.ViewHost.Views
{
    [Connected(View = typeof(EmptyStateView), ViewModel = typeof(EmptyStateViewModel))]
    public partial class EmptyStateView : UserControl
    {
        public EmptyStateView()
        {
            InitializeComponent();
        }
    }
}