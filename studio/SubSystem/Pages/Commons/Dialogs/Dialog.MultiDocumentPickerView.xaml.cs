using System.Windows;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons
{
    [Connected(View = typeof(MultiDocumentPickerView), ViewModel = typeof(MultiDocumentPickerViewModel))]
    public partial class MultiDocumentPickerView
    {
        public MultiDocumentPickerView()
        {
            InitializeComponent();
        }

        protected override void OnLoaded(object sender, RoutedEventArgs e)
        {
            
            ViewModel<MultiDocumentPickerViewModel>().TargetElement = ListBox;
            base.OnLoaded(sender, e);
        }
    }
}