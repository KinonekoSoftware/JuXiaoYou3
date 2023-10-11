using System.Windows.Controls;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Tools.ModuleEditor.ViewModels;

namespace Acorisoft.FutureGL.Tools.ModuleEditor.Views
{
    public class TabShellBindingProxy : BindingProxy<TabShell>{}
    
    public partial class TabShellView : UserControl
    {
        public TabShellView()
        {
            InitializeComponent();
        }
    }
}