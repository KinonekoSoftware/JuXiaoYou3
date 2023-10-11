namespace Acorisoft.FutureGL.Forest.Utils
{
    public static class FrameworkElementExtensions
    {
        public static T ViewModel<T>(this FrameworkElement element) where T : class
        {
            return element.DataContext as T;
        }
    }
}