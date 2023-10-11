using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Interactions
{

    [Connected(View = typeof(CharacterContractPage), ViewModel = typeof(CharacterContractViewModel))]
    public partial class CharacterContractPage
    {
        public CharacterContractPage()
        {
            InitializeComponent();
        }
    }
}