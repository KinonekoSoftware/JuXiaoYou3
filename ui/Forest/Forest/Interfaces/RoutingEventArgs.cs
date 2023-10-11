namespace Acorisoft.FutureGL.Forest
{
    /// <summary>
    /// 参数
    /// </summary>
    public class RoutingEventArgs
    {
        public static readonly RoutingEventArgs Empty = new RoutingEventArgs
        {
            Parameter = new Parameter
            {
                Args = new object[4]
            }
        };
        
        /// <summary>
        /// 来源视图模型，在DialogViewModel中使用！
        /// </summary>
        public IViewModel ViewModelSource { get;internal set; }

        /// <summary>
        /// 关闭对话框处理器，在DialogViewModel中使用！
        /// </summary>
        public Action CloseHandler { get; internal set; }
        
        public object Controller { get; init; }
        public string Id { get; init; }
        public Parameter Parameter { get; init; }
    }

    public class Parameter
    {
        public static readonly Parameter Empty = new Parameter
        {
            Args = new object[4]
        };

        public Parameter()
        {
        }

        public Parameter(object[] args)
        {
            Args = args;
        }

        /// <summary>
        /// 参数
        /// </summary>
        public object[] Args { get; init; }
    }
}