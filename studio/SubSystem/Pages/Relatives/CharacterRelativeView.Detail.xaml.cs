using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Acorisoft.FutureGL.Forest.Utils;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Documents
{
    public partial class CharacterRelshipPartView : UserControl
    {
        public CharacterRelshipPartView()
        {
            InitializeComponent();
        }
        
        
        private void OnDataSourceDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is not FrameworkElement { DataContext: DocumentCache target })
            {
                return;
            }

            this.ViewModel<CharacterRelPartViewModel>().SelectedDocument = target;
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