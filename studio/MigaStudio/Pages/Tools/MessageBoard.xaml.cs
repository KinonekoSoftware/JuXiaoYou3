using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{

    [Connected(View = typeof(MessageBoardPage), ViewModel = typeof(MessageBoardViewModel))]
    public partial class MessageBoardPage
    {
        public MessageBoardPage()
        {
            InitializeComponent();
        }
    }
}