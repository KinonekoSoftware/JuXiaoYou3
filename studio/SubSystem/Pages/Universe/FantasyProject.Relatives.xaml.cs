using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Universe
{

    [Connected(View = typeof(FantasyProjectRelativesPage), ViewModel = typeof(FantasyProjectRelativesViewModel))]
    public partial class FantasyProjectRelativesPage
    {
        public FantasyProjectRelativesPage()
        {
            InitializeComponent();
        }
    }
}