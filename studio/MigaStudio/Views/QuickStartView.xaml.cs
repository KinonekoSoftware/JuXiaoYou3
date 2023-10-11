using Acorisoft.FutureGL.Forest.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Views
{
    [Connected(View = typeof(QuickStartView), ViewModel = typeof(QuickStartController))]
    public partial class QuickStartView:ForestUserControl 
    {
        public QuickStartView()
        {
            InitializeComponent();
        }

    }
}