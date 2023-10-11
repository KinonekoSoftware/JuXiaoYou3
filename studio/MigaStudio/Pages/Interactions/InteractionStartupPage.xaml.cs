using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Interactions
{

    [Connected(View = typeof(InteractionStartup), ViewModel = typeof(InteractionStartupViewModel))]
    public partial class InteractionStartup
    {
        public InteractionStartup()
        {
            InitializeComponent();
        }
    }
}