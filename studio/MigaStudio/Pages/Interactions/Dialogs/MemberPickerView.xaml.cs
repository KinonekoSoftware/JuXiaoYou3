using System.Windows;
using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Interactions.Dialogs
{
    [Connected(View = typeof(MemberPickerView), ViewModel = typeof(MemberPickerViewModel))]
    public partial class MemberPickerView
    {
        public MemberPickerView()
        {
            InitializeComponent();
        }
        
        
        protected override void OnLoaded(object sender, RoutedEventArgs e)
        {
            
            ViewModel<MemberPickerViewModel>().TargetElement = ListBox;
            base.OnLoaded(sender, e);
        }
    }
}