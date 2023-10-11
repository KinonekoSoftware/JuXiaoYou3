using System.Reflection;
using Acorisoft.FutureGL.Forest;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Dialogs
{
    public class AutomaticDataProtectionViewModel : CountableDialogVM
    {
        protected override void Finish()
        {
            var ss = Xaml.Get<SystemSetting>();

            var assemblyVersion = Assembly.GetAssembly(typeof(MainWindow))
                                          .GetCustomAttribute<AssemblyInformationalVersionAttribute>();

            var version = assemblyVersion?.InformationalVersion ?? "3.0.0";
            ss.AdvancedSetting
              .ApplicationVersion = version;
            Context.IsUpdated = false;
            ss.Save();
        }

        protected override string Failed()
        {
            return SubSystemString.Unknown;
        }

        public GlobalStudioContext Context { get; init; }
    }
}