using Acorisoft.FutureGL.MigaStudio.Pages.Universe;

namespace Acorisoft.FutureGL.MigaStudio.ViewModels
{
    public class WorldViewController : ShellCore
    {
        protected override void RequireStartupTabViewModel()
        {
            New<FantasyProjectStartupViewModel>();
        }

        public sealed override string Id => AppViewModel.IdOfWorldViewController;
    }
}