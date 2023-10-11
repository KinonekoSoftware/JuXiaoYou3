namespace Acorisoft.FutureGL.Forest.Services
{
    public delegate FrameworkElement ViewFactory(object owner, object data);

    public class ViewFactoryService : IViewFactoryService
    {
        private readonly Dictionary<Type, ViewFactory> _factory = new Dictionary<Type, ViewFactory>();

        public void Isolate<TOwner, TDetail, TView, TViewModel>()
            where TOwner : class, IViewModel
            where TDetail : class
            where TView : FrameworkElement, new()
            where TViewModel : class, IIsolatedViewModel<TOwner, TDetail>, new()
        {
            FrameworkElement BuildView(object parent, object data)
            {
                if (parent is not TOwner owner)
                {
                    return Missing();
                }

                if (data is not TDetail detail)
                {
                    return Missing();
                }

                return new TView
                {
                    DataContext = new TViewModel
                    {
                        Owner  = owner,
                        Detail = detail
                    }
                };
            }

            _factory.Add(typeof(TDetail), BuildView);
        }

        public void Direct<TOwner, TDetail, TView>()
            where TOwner : class, IViewModel
            where TDetail : class
            where TView : FrameworkElement, new()
        {
            FrameworkElement BuildView(object parent, object data)
            {
                if (parent is not TOwner owner)
                {
                    return Missing();
                }

                return new TView
                {
                    DataContext = owner
                };
            }

            _factory.Add(typeof(TDetail), BuildView);
        }

        public void Manual<TDetail>(ViewFactory factory)
        {
            _factory.Add(typeof(TDetail), factory);
        }


        public FrameworkElement GetView(object owner, object data)
        {
            if (owner is null || data is null)
            {
                return Missing();
            }


            var key = data.GetType();

            return _factory.TryGetValue(key, out var factory) ? factory(owner, data) : Missing();
        }

        private FrameworkElement Missing()
        {
            return MissingViewMapping?.Invoke() ?? new UserControl
            {
                DataContext = "缺失映射"
            };
        }


        public Func<FrameworkElement> MissingViewMapping { get; set; }
    }

    public interface IViewFactoryService
    {
        void Manual<TDetail>(ViewFactory factory);
        
        void Isolate<TOwner, TDetail, TView, TViewModel>()
            where TOwner : class, IViewModel
            where TDetail : class
            where TView : FrameworkElement, new()
            where TViewModel : class, IIsolatedViewModel<TOwner, TDetail>, new();
        
        void Direct<TOwner, TDetail, TView>()
            where TOwner : class, IViewModel
            where TDetail : class
            where TView : FrameworkElement, new();

        FrameworkElement GetView(object owner, object data);
        Func<FrameworkElement> MissingViewMapping { get; set; }
    }
}