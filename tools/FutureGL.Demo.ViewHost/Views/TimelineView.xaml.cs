using System.Windows.Controls;

namespace Acorisoft.FutureGL.Demo.ViewHost.Views
{
    [Connected(View = typeof(TimelineView), ViewModel = typeof(TimelineViewModel))]
    public partial class TimelineView : UserControl
    {
        public TimelineView()
        {
            InitializeComponent();
        }
    }
}