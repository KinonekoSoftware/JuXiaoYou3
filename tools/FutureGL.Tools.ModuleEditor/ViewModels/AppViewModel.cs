using Acorisoft.FutureGL.MigaStudio.ViewModels;

namespace Acorisoft.FutureGL.Tools.ModuleEditor.ViewModels
{
    public class AppViewModel : TabBaseAppViewModel
    {
        public AppViewModel()
        {
            Controller        = new TabShell();
            CurrentController = Controller;
        }
    }
}