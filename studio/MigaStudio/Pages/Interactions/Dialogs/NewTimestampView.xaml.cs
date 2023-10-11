using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Interactions.Dialogs
{
    [Connected(View = typeof(NewTimestampView), ViewModel = typeof(NewTimestampViewModel))]
    public partial class NewTimestampView
    {
        public NewTimestampView()
        {
            InitializeComponent();
        }
    }
}