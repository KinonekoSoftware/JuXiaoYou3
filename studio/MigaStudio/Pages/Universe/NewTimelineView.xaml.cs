using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Universe
{
    [Connected(View = typeof(NewTimelineView), ViewModel = typeof(NewTimelineViewModel))]
    public partial class NewTimelineView
    {
        public NewTimelineView()
        {
            InitializeComponent();
        }
    }
}