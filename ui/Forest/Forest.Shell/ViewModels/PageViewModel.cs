namespace Acorisoft.FutureGL.Forest.ViewModels
{
    public abstract class PageViewModel : ViewModelBase
    {
        
        #region Start / OnStart

        public sealed override void Start()
        {
            try
            {
                if (!IsInitialized)
                {
                    OnStart();
                    IsInitialized = true;
                }
            }
            catch
            {
                IsInitialized = false;
            }
        }

        protected virtual void OnStart()
        {
            
        }
        
        protected virtual void OnStart(Parameter parameter)
        {
        }

        #endregion
    }
}