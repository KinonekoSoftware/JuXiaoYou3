using System.Windows;
using System.Windows.Input;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Relatives
{

    [Connected(View = typeof(CharacterRelationshipPage), ViewModel = typeof(CharacterRelationshipViewModel))]
    public partial class CharacterRelationshipPage
    {
        public CharacterRelationshipPage()
        {
            InitializeComponent();
        }

        private void OnDataSourceDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is not FrameworkElement { DataContext: DocumentCache target })
            {
                return;
            }

            ViewModel<CharacterRelationshipViewModel>().SelectedDocument = target;
        }

        private void DisplayMode_Circle(object sender, RoutedEventArgs e)
        {
            Relationship.LayoutAlgorithmType = "Circular";
        }

        private void DisplayMode_ISOM(object sender, RoutedEventArgs e)
        {
            Relationship.LayoutAlgorithmType = "ISOM";
        }
        
        private void DisplayMode_LinLog(object sender, RoutedEventArgs e)
        {
            Relationship.LayoutAlgorithmType = "LinLog";
        }
        
        private void DisplayMode_KK(object sender, RoutedEventArgs e)
        {
            Relationship.LayoutAlgorithmType = "KK";
        }

        private void Button_reLayout(object sender, RoutedEventArgs e)
        {
            Relationship.Relayout();
        }
    }
}