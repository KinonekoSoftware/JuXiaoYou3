using System.Windows.Controls;
using Acorisoft.FutureGL.MigaStudio.Inspirations;

namespace Acorisoft.FutureGL.MigaStudio.Inspirations
{

    [Connected(View = typeof(InspirationView), ViewModel = typeof(InspirationController))]
    public partial class InspirationView
    {
        public InspirationView()
        {
            InitializeComponent();
        }
    }
}