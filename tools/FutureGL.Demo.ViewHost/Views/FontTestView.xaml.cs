using System.Windows.Controls;
using Acorisoft.FutureGL.Forest.Controls;

namespace Acorisoft.FutureGL.Demo.ViewHost.Views
{

    [Connected(View = typeof(FontTestView), ViewModel = typeof(FontTestViewModel))]
    public partial class FontTestView:ForestUserControl 
    {
        public FontTestView()
        {
            InitializeComponent();
        }
    }
}