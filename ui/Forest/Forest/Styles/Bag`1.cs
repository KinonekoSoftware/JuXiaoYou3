namespace Acorisoft.FutureGL.Forest.Styles
{
    /// <summary>
    /// 表示一个大小
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Bag<T>
    {
        /// <summary>
        /// 小尺寸
        /// </summary>
        public T Samll { get; init; }
        
        /// <summary>
        /// 中尺寸
        /// </summary>
        public T Medium { get; init; }
        
        /// <summary>
        /// 大尺寸
        /// </summary>
        public T Large { get; init; }
    }
}