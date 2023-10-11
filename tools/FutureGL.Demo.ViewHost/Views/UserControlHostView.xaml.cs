using System.Windows.Controls;
using Acorisoft.FutureGL.Forest.Controls;

namespace Acorisoft.FutureGL.Demo.ViewHost.Views
{
    [Connected(View = typeof(UserControlHostView), ViewModel = typeof(UserControlHostViewModel))]
    public partial class UserControlHostView:ForestUserControl 
    {
        public UserControlHostView()
        {
            InitializeComponent();
        }
    }
}