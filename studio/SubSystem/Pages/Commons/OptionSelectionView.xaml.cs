using Acorisoft.FutureGL.Forest.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons
{
    [Connected(View = typeof(OptionSelectionView), ViewModel = typeof(OptionSelectionViewModel))]
    public partial class OptionSelectionView:ForestUserControl 
    {
        public OptionSelectionView()
        {
            InitializeComponent();
        }
    }
}