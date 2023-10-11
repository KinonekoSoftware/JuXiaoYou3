using System.Windows;
using System.Windows.Controls;
using Acorisoft.FutureGL.Forest.Utils;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    public partial class UniverseAlbumView : UserControl
    {
        public UniverseAlbumView()
        {
            InitializeComponent();
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= OnLoaded;
            this.ViewModel<UniverseViewModel>()
                .AlbumButtonUpdateHandler = OnAlbumButtonUpdated;
        }

        private void OnAlbumButtonUpdated()
        {
            Up.IsEnabled =
                Down.IsEnabled =
                    Remove.IsEnabled = 
                        Edit.IsEnabled =
                            Open.IsEnabled = this.ViewModel<UniverseViewModel>().SelectedAlbum is not null;
            
            
            Up.CommandParameter =
                Down.CommandParameter =
                    Remove.CommandParameter = 
                        Edit.CommandParameter =
                            Open.CommandParameter = this.ViewModel<UniverseViewModel>().SelectedAlbum;
        }
    }
}