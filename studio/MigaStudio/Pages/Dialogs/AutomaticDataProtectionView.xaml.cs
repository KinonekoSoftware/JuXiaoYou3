using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Dialogs
{
    [Connected(View = typeof(AutomaticDataProtectionView), ViewModel = typeof(AutomaticDataProtectionViewModel))]
    public partial class AutomaticDataProtectionView
    {
        public AutomaticDataProtectionView()
        {
            InitializeComponent();
        }
    }
}