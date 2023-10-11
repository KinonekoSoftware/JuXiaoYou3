using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Inspirations.Pages
{

    [Connected(View = typeof(HomePage), ViewModel = typeof(HomeViewModel))]
    public partial class HomePage
    {
        public HomePage()
        {
            InitializeComponent();
        }
    }
}