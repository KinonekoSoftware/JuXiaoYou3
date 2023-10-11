using Acorisoft.FutureGL.Forest.Controls;
using System.Windows.Input;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    [Connected(View = typeof(DocumentGalleryPage), ViewModel = typeof(DocumentGalleryViewModel))]
    public partial class DocumentGalleryPage
    {
        public DocumentGalleryPage()
        {
            InitializeComponent();
        }


        private void SearchPage_OnKeyUp(object sender, KeyEventArgs e)       
        {
            var vm = ViewModel<DocumentGalleryViewModel>();

            if(string.IsNullOrEmpty(vm.FilterString))
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
                vm.IsFiltering  = false;
                vm.SearchPage();
            }
        }
    }
}