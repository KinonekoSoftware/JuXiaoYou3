using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Startup
{

    [Connected(View = typeof(StartupWorldViewPage), ViewModel = typeof(StartupWorldViewViewModel))]
    public partial class StartupWorldViewPage
    {
        public StartupWorldViewPage()
        {
            InitializeComponent();
        }
    }
}