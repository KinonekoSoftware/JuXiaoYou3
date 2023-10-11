using System.Windows;
using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Interactions
{
    [Connected(View = typeof(CharacterPickerView), ViewModel = typeof(CharacterPickerViewModel))]
    public partial class CharacterPickerView
    {
        public CharacterPickerView()
        {
            InitializeComponent();
        }
        
        protected override void OnLoaded(object sender, RoutedEventArgs e)
        {
            
            ViewModel<CharacterPickerViewModel>().TargetElement = ListBox;
            base.OnLoaded(sender, e);
        }
    }
}