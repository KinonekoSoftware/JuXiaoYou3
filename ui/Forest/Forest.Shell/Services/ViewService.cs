using Acorisoft.FutureGL.Forest.Controls;
using Acorisoft.FutureGL.Forest.Interfaces;

namespace Acorisoft.FutureGL.Forest.Services
{
    public class ViewService : IViewService, IViewServiceAmbient
    {
        private ViewHostBase _viewHost;
        public void Route<TViewModel>()  where TViewModel : IViewModel
        {
            Route<TViewModel>(new RoutingEventArgs());
        }

        public void Route<TViewModel>(RoutingEventArgs parameter) where TViewModel : IViewModel
        {
            var viewModel = Xaml.GetViewModel<TViewModel>();
            Route(viewModel, RoutingEventArgs.Empty);
        }

        public void Route(IViewModel viewModel)
        {
            Route(viewModel, RoutingEventArgs.Empty);
        }

        public void Route(IViewModel viewModel, RoutingEventArgs parameter)
        {
            viewModel.Startup(parameter);
            _viewHost.ViewModel = viewModel;
            
        }

        public void SetServiceProvider(ViewHostBase host) => _viewHost = host;
    }
}