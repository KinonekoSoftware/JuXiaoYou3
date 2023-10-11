namespace Acorisoft.FutureGL.MigaStudio.Pages
{

    [Connected(View = typeof(InTabGalleryPage), ViewModel = typeof(ServiceViewModel))]
    [Connected(View = typeof(InTabGalleryPage), ViewModel = typeof(RelationshipViewModel))]
    [Connected(View = typeof(InTabGalleryPage), ViewModel = typeof(ToolsViewModel))]
    public partial class InTabGalleryPage
    {
        public InTabGalleryPage()
        {
            InitializeComponent();
        }
    }
}