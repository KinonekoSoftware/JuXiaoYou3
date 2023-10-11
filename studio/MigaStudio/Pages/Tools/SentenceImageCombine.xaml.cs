using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Tools
{

    [Connected(View = typeof(SentenceImageCombinePage), ViewModel = typeof(SentenceImageCombineViewModel))]
    public partial class SentenceImageCombinePage
    {
        public SentenceImageCombinePage()
        {
            InitializeComponent();
        }
    }
}