using Acorisoft.FutureGL.Forest.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    [Connected(View = typeof(HomePage), ViewModel = typeof(HomeViewModel))]
    public partial class HomePage:ForestUserControl 
    {
        public HomePage()
        {
            InitializeComponent();
        }
    }
}