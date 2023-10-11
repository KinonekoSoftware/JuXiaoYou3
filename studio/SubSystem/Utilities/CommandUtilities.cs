using CommunityToolkit.Mvvm.Input;

namespace Acorisoft.FutureGL.MigaStudio.Utilities
{
    public static class CommandUtilities
    {
        public static RelayCommand<T> CreateSelectedCommand<T>(Action<T> callback)
        {
            return new RelayCommand<T>(x => callback?.Invoke(x));
        }
    }
}