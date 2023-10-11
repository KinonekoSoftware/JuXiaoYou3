using Acorisoft.FutureGL.Forest.Interfaces;

namespace Acorisoft.FutureGL.Forest.Controls
{
    public class ViewHost : ViewHostBase
    {
        static ViewHost()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ViewHost), new FrameworkPropertyMetadata(typeof(ViewHost)));
        }


        public ViewHost()
        {
            Xaml.Get<IViewServiceAmbient>()
                .SetServiceProvider(this);
        }

        protected override void OnViewModelChanged(ViewModelBase vm)
        {
            base.OnViewModelChanged(vm);
        }
    }
}