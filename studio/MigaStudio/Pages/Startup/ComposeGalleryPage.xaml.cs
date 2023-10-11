using System.Windows.Input;
using Acorisoft.FutureGL.Forest.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{

    [Connected(View = typeof(ComposeGalleryPage), ViewModel = typeof(ComposeGalleryViewModel))]
    public partial class ComposeGalleryPage
    {
        public ComposeGalleryPage()
        {
            InitializeComponent();
        }


        private void SearchPage_OnKeyUp(object sender, KeyEventArgs e)
        {
            var vm = ViewModel<ComposeGalleryViewModel>();

            if (string.IsNullOrEmpty(vm.FilterString))
            {
                return;
            }

            if (e.Key == Key.Enter)
            {
                vm.SearchPage();
            }
            else if (e.Key == Key.Escape)
            {
                (sender as ClosableSingleLine).Text = string.Empty;
                vm.FilterString = string.Empty;
                vm.IsFiltering = false;
                vm.SearchPage();
            }
        }
    }
}