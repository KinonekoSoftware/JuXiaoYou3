using System.Windows.Controls;
using System.Windows.Input;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Tools
{

    [Connected(View = typeof(DocumentRecycleBinPage), ViewModel = typeof(DocumentRecycleBinViewModel))]
    public partial class DocumentRecycleBinPage
    {
        public DocumentRecycleBinPage()
        {
            InitializeComponent();
        }
        

        private void SearchPage_OnKeyUp(object sender, KeyEventArgs e)
        {
            var vm = ViewModel<DocumentGalleryViewModel>();
            
            if (e.Key == Key.Enter)
            {
                vm.SearchPage();
            }
            else if (e.Key == Key.Escape)
            {
                vm.FilterString = string.Empty;
                vm.IsFiltering  = false;
                vm.SearchPage();
            }
        }
    }
}