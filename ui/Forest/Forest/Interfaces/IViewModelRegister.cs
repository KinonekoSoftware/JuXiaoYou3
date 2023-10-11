using Acorisoft.FutureGL.Forest.Models;

namespace Acorisoft.FutureGL.Forest.Interfaces
{
    public interface IViewModelRegister
    { 
        void Register(IViewInstaller collection);
    }

    public interface IViewInstaller
    {
        void Install(BindingInfo info);
        void Install(IEnumerable<BindingInfo> bindingInfos);
        void Install(IBindingInfoProvider provider);
        void Install(IEnumerable<IBindingInfoProvider> providers);
    }
}
