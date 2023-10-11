namespace Acorisoft.FutureGL.Forest.Controls
{
    public abstract class ForestUserControl : UserControl
    {
        protected ForestUserControl()
        {
            Loaded   += OnLoadedIntern;
            Unloaded += OnUnloadedIntern;
        }


        protected TViewModel ViewModel<TViewModel>() where TViewModel : IViewModel
        {
            return (TViewModel)DataContext;
        }

        #region Loaded / Unloaded

        private void OnLoadedIntern(object sender, RoutedEventArgs e)
        {
            ViewModel<IViewModel>()?.Start();
            OnLoaded(sender, e);
        }

        protected virtual void OnUnloaded(object sender, RoutedEventArgs e)
        {
        }

        protected virtual void OnLoaded(object sender, RoutedEventArgs e)
        {
        }

        private void OnUnloadedIntern(object sender, RoutedEventArgs e)
        {
            Loaded   -= OnLoadedIntern;
            Unloaded -= OnUnloadedIntern;
            
            //
            // 不要加Stop命令，因为View的退出并不意味着ViewModel结束了生命周期
            // ViewModel<IViewModel>()?.Stop();
            OnUnloaded(sender, e);
        }

        #endregion
    }
}