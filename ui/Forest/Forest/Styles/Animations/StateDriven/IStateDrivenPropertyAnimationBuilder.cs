namespace Acorisoft.FutureGL.Forest.Styles.Animations
{
    /// <summary>
    /// <see cref="IStateDrivenAnimationBuilder"/> 接口表示一个属性动画构造器。
    /// </summary>
    public interface _IStateDrivenAnimationBuilder : IStateDrivenTargetBuilder
    {
        /// <summary>
        /// 目标构造器上下文。
        /// </summary>
        IStateDrivenTargetBuilder TargetContext { get; init; }
    }

    /// <summary>
    /// <see cref="IStateDrivenAnimationBuilder{T}"/> 接口表示一个属性动画构造器。
    /// </summary>
    public interface _IStateDrivenAnimationBuilder<in T> : IStateDrivenAnimationBuilder
    {
        /// <summary>
        /// 下一个状态。
        /// </summary>
        /// <param name="state">指定的状态。</param>
        /// <param name="value">指定的值。</param>
        /// <returns></returns>
        _IStateDrivenAnimationBuilder<T> Next(VisualState state, T value);
    }
}