using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.ViewModels;

namespace Acorisoft.FutureGL.MigaStudio.ViewModels
{
    public abstract class IsolatedViewModel : ViewModelBase, IIsolatedViewModel
    {
        
    }

    public abstract class IsolatedViewModel<TOwner, TDetail> : IsolatedViewModel, IIsolatedViewModel<TOwner, TDetail>
        where TOwner : class, IViewModel
        where TDetail : class
    {
        public TDetail Detail { get; init; }
        public TOwner Owner { get; init; }
    }
}