namespace Acorisoft.FutureGL.Forest.Styles.Animations
{
    /// <summary>
    /// 表示一个动画引擎。
    /// </summary>
    public abstract class Animator : IStateDrivenAnimator
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

        class DummyAnimator : Animator
        {
            public override void NextState()
            {
                
            }

            public override void NextState(VisualState state)
            {
            }
        }

        public static Animator CreateDummy()
        {
            return new DummyAnimator();
        }
    }
}