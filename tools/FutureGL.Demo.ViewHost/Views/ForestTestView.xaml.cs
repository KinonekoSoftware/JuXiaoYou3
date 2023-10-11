using System.Windows.Controls;

namespace Acorisoft.FutureGL.Demo.ViewHost.Views
{
    [Connected(View = typeof(ForestTestView), ViewModel = typeof(ForestTestViewModel))]
    public partial class ForestTestView : UserControl
    {
        public ForestTestView()
        {
            InitializeComponent();
        }
    }
}