namespace Acorisoft.FutureGL.Forest.Models
{
    public class BindingInfo
    {
        public BindingInfo(){}

        public BindingInfo(Type v, Type vm)
        {
            View      = v;
            ViewModel = vm;
        }
        public Type View { get; init; }
        public Type ViewModel { get; init; }
        public bool IsSingleton { get; init; }
    }
}