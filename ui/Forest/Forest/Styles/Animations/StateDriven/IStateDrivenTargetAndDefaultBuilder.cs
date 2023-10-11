namespace Acorisoft.FutureGL.Forest.Styles.Animations
{
    /// <summary>
    /// <see cref="IStateDrivenTargetAndDefaultBuilder"/> 接口表示一个目标默认值构建器。
    /// </summary>
    public interface IStateDrivenTargetAndDefaultBuilder : ICompletedSource<IStateDrivenTargetBuilder, StateDrivenAnimation>
    {
        /// <summary>
        /// 设置属性。
        /// </summary>
        /// <param name="property">目标属性。</param>
        /// <param name="value">目标值。</param>
        /// <returns>返回构造器。</returns>
        IStateDrivenTargetAndDefaultBuilder Set(DependencyProperty property, object value);
        
        /// <summary>
        /// 动画构造器上下文。
        /// </summary>
        public IStateDrivenAnimatorBuilder AnimatorContext { get;init; }

        void Finish();
    }
}