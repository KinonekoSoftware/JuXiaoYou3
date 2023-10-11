namespace Acorisoft.FutureGL.Forest.Styles.Animations
{
    public interface IStateDrivenAnimationBuilder : ICompletedSource<IStateDrivenAnimationBuilder, IPropertyAnimationRunner>
    {
        /// <summary>
        /// 构建完整的状态。
        /// </summary>
        /// <returns>返回一个目标构建器。</returns>
        IStateDrivenTargetBuilder Finish();

        /// <summary>
        /// 
        /// </summary>
        public IStateDrivenTargetBuilder Context { get; set; }
    }
    
    public interface IStateDrivenAnimationBuilder<in T> : IStateDrivenAnimationBuilder
    {
        /// <summary>
        /// 下一个状态。
        /// </summary>
        /// <param name="state">指定的状态。</param>
        /// <param name="value">指定的值。</param>
        /// <returns></returns>
        IStateDrivenAnimationBuilder<T> Next(VisualState state, T value);
    }
}