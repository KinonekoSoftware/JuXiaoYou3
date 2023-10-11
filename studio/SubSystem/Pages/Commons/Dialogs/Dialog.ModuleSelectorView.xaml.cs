using System.Windows;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons
{
    [Connected(View = typeof(ModuleSelectorView), ViewModel = typeof(ModuleSelectorViewModel))]
    public partial class ModuleSelectorView
    {
        public ModuleSelectorView()
        {
            InitializeComponent();
        }

        protected override void OnLoaded(object sender, RoutedEventArgs e)
        {
            ViewModel<ModuleSelectorViewModel>().TargetElement = Collection;
            base.OnLoaded(sender, e);
        }
    }
}