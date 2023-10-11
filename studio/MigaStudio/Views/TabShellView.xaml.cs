using System.Windows.Input;
using Acorisoft.FutureGL.Forest.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Views
{
    [Connected(View = typeof(TabShellView), ViewModel = typeof(WorldViewController))]
    public partial class TabShellView
    {
        public TabShellView()
        {
            InitializeComponent();
        }

        public ShellCore ViewModel => (ShellCore)DataContext;
    }
}