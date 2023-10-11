using System.Windows;
using System.Windows.Controls;
using Acorisoft.FutureGL.MigaStudio.Pages.Documents.Share;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons
{
    public partial class ShareView : UserControl
    {
        private bool _initialize;
        public ShareView()
        {
            InitializeComponent();
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is not DocumentEditorBase dc)
            {
                return;
            }

            if (_initialize)
            {
                return;
            }
            
            
            dc.Preshapes
              .ForEach(x =>
              {
                  ((PreshapeViewModelBase)x.DataContext).Owner = dc;
                  Preshape.Items
                          .Add(new Acorisoft.FutureGL.Forest.UI.Tools.TabItem
                          {
                              Header  = x.Tag,
                              Content = x,
                          });
              });
            _initialize = true;
        }
    }
}