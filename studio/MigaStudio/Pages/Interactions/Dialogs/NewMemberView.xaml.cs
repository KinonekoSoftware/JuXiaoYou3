using System.Windows;
using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Interactions.Dialogs
{
    [Connected(View = typeof(NewMemberView), ViewModel = typeof(NewMemberViewModel))]
    public partial class NewMemberView
    {
        public NewMemberView()
        {
            InitializeComponent();
        }
        
        
        protected override void OnLoaded(object sender, RoutedEventArgs e)
        {
            
            ViewModel<NewMemberViewModel>().TargetElement = ListBox;
            base.OnLoaded(sender, e);
        }
    }
}