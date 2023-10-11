namespace Acorisoft.FutureGL.Forest.Models
{
    public class AsyncJob : ForestObject
    {
        /// <summary>
        /// 
        /// </summary>
        public Action<object> Handler { get; init; }

        /// <summary>
        /// 
        /// </summary>
        public string Title { get; init; }
    }
}