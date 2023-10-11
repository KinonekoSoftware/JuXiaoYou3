using Acorisoft.FutureGL.Forest.AppModels;
using Acorisoft.FutureGL.Forest.Controls;
using Acorisoft.FutureGL.Forest.Interfaces;

namespace Acorisoft.FutureGL.MigaStudio.Core
{
    public class ViewServiceAdapter : IViewServiceAmbient
    {
        private readonly ViewService _service;
        
        public ViewServiceAdapter()
        {
            _service = new ViewService();
        }
        
        
        public void SetServiceProvider(ViewHostBase host) => _service.SetServiceProvider(host);

        public IViewService Host => _service;
        
        public ITabViewController Controller { get; set; }
    }
}