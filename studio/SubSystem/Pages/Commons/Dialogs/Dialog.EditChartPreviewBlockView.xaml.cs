using System.Windows;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons
{
    [Connected(View = typeof(EditChartPresentationView), ViewModel = typeof(EditChartPresentationViewModel))]
    public partial class EditChartPresentationView
    {
        public EditChartPresentationView()
        {
            InitializeComponent();
        }

        protected override void OnLoaded(object sender, RoutedEventArgs e)
        {
            base.OnLoaded(sender, e);
        }
    }
}