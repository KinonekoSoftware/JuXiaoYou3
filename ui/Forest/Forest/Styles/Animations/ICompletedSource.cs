namespace Acorisoft.FutureGL.Forest.Styles.Animations
{
    public interface ICompletedSource<out TSource, out TResult>
    {
        /// <summary>
        /// 
        /// </summary>
        public event Action<TSource, TResult> Completed;
    }
}