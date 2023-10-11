using System.Linq;
using Acorisoft.FutureGL.Forest.AppModels;
using Acorisoft.FutureGL.MigaStudio.Core;

namespace Acorisoft.FutureGL.MigaStudio
{
    partial class App
    {
        private static void InstallInMemoryService(IInMemoryServiceHost attachable)
        {
            attachable.Add(new ColorService());
            attachable.Add(new TokenizerService());
        } 
    }
}

namespace Acorisoft.FutureGL.MigaStudio.ViewModels
{

    partial class AppViewModel
    {
        public static ITabViewController Route(GlobalStudioContext context, bool opening)
        {
            if (opening)
            {
                
#if DEBUG

                return context.Controllers.First(x => x is TabShell);
                // return context.Controllers.First(x => x is InteractionController);
#else
                return context.Controllers.First(x => x is TabShell);
#endif
            }
            else
            {

                return context.Controllers.First(x => x is QuickStartController);
            }
        }
    }
}