namespace Acorisoft.FutureGL.Forest.Styles.Animations
{
    /// <summary>
    /// <see cref="IStateDrivenAnimatorBuilder"/> 接口表示一个动画构建器。
    /// </summary>
    public interface IStateDrivenAnimatorBuilder
    {
        /// <summary>
        /// 获取目标默认值构造器。
        /// </summary>
        /// <param name="element">目标元素。</param>
        /// <returns>返回目标默认值构造器。</returns>
        IStateDrivenTargetAndDefaultBuilder TargetAndDefault(FrameworkElement element);
        
        /// <summary>
        /// 获取一个新的目标构造器。
        /// </summary>
        /// <returns>返回一个新的目标构造器。</returns>
        IStateDrivenTargetBuilder Target(FrameworkElement element);

        /// <summary>
        /// 构造动画引擎
        /// </summary>
        /// <returns>返回动画引擎。</returns>
        StateDrivenAnimator Finish();
    }
}