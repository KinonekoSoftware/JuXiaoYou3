using Acorisoft.FutureGL.Forest.Models;

namespace Acorisoft.FutureGL.Forest.Interfaces
{
    public interface IBindingInfoProvider
    {
        IEnumerable<BindingInfo> GetBindingInfo();
    }
}