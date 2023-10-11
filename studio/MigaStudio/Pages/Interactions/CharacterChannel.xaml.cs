using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Interactions
{

    [Connected(View = typeof(CharacterChannelPage), ViewModel = typeof(CharacterChannelViewModel))]
    public partial class CharacterChannelPage
    {
        public CharacterChannelPage()
        {
            InitializeComponent();
        }
    }
}