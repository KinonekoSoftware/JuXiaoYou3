namespace Acorisoft.FutureGL.Forest.Styles.Animations
{
    /// <summary>
    /// <see cref="StateDrivenAnimator"/> 基于状态的动画引擎。
    /// </summary>
    public class StateDrivenAnimator : Animator, IStateDrivenAnimator
    {
        /// <summary>
        /// 进入初始状态
        /// </summary>
        public sealed override void NextState()
        {
            foreach (var animation in Animations)
            {
                animation.NextState();
            }
            
            foreach (var animation in FirstState)
            {
                animation.NextState();
            }
        }

        /// <summary>
        /// 进入下一个状态
        /// </summary>
        /// <param name="state">下一个状态</param>
        public sealed override void NextState(VisualState state)
        {
            foreach (var animation in Animations)
            {
                animation.NextState(state);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public IList<StateDrivenAnimation> FirstState { get;init; }
        
        /// <summary>
        /// 
        /// </summary>
        public IList<StateDrivenAnimation> Animations { get; init; }
    }
}