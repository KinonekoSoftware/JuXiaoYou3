namespace Acorisoft.FutureGL.Forest.Attributes
{
    public enum UniTestLifetime
    {
        Constructor,
        
        /// <summary>
        /// 传参
        /// </summary>
        Startup,
        
        /// <summary>
        /// 启动
        /// </summary>
        Start,
        
        /// <summary>
        /// 运行
        /// </summary>
        Running
    }
    
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public sealed class NullCheckAttribute : Attribute
    {
        public NullCheckAttribute(UniTestLifetime lifetime)
        {
            Lifetime = lifetime;
        }
        
        public UniTestLifetime Lifetime { get;}
    }
}