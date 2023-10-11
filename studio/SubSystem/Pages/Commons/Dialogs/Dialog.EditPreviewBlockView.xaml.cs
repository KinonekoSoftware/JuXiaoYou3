using System.Windows;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons
{
    [Connected(View = typeof(EditPresentationView), ViewModel = typeof(EditPresentationViewModel))]
    public partial class EditPresentationView
    {
        public EditPresentationView()
        {
            InitializeComponent();
        }

        protected override void OnLoaded(object sender, RoutedEventArgs e)
        {
            ViewModel<EditPresentationViewModel>().TargetElement = Collection;
            base.OnLoaded(sender, e);
        }
    }
}