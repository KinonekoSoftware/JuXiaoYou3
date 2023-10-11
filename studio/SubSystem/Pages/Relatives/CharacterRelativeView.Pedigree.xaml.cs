using System.Windows;
using System.Windows.Input;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Relatives
{

    [Connected(View = typeof(CharacterPedigreePage), ViewModel = typeof(CharacterPedigreeViewModel))]
    public partial class CharacterPedigreePage
    {
        public CharacterPedigreePage()
        {
            InitializeComponent();
        }
        
        private void OnDataSourceDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is not FrameworkElement { DataContext: DocumentCache target })
            {
                return;
            }

            ViewModel<CharacterPedigreeViewModel>().SelectedDocument = target;
        }

        
        private void Button_reLayout(object sender, RoutedEventArgs e)
        {
            Relationship.Relayout();
        }
    }
}