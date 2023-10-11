namespace Acorisoft.FutureGL.Forest.Styles.Animations
{
    /// <summary>
    /// <see cref="Acorisoft.FutureGL.Forest.Styles.Animations.StateDrivenAnimation"/> 表示一个基于状态的动画。
    /// </summary>
    public abstract class StateDrivenAnimation  : IStateDrivenAnimator
    {
        /// <summary>
        /// 进入初始状态
        /// </summary>
        public abstract void NextState();
        
        /// <summary>
        /// 进入下一个状态
        /// </summary>
        /// <param name="state">下一个状态</param>
        public abstract void NextState(VisualState state);
    }
}