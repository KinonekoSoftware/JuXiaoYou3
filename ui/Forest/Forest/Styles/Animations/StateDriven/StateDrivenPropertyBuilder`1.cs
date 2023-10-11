namespace Acorisoft.FutureGL.Forest.Styles.Animations
{
    public abstract class StateDrivenPropertyBuilder<TValue> : IStateDrivenAnimationBuilder<TValue>
    {
        public IStateDrivenAnimationBuilder<TValue> Next(VisualState state, TValue value)
        {
            Mapper.TryAdd(state, value);
            return this;
        }

        public IStateDrivenTargetBuilder Finish()
        {
            Completed?.Invoke(this, Mapper.Count > 0 ? CreateAnimation() : null);
            return Context;
        }
        
        
        protected abstract IPropertyAnimationRunner CreateAnimation();

        /// <summary>
        /// 目标路径
        /// </summary>
        public PropertyPath PropertyPath { get; init; }

        /// <summary>
        /// 时长
        /// </summary>
        public Duration Duration { get; init; }
        
        /// <summary>
        /// 
        /// </summary>
        public IStateDrivenTargetBuilder Context { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<VisualState, TValue> Mapper { get; } = new Dictionary<VisualState, TValue>();
        
        /// <summary>
        /// 完成事件。
        /// </summary>
        public event Action<IStateDrivenAnimationBuilder, IPropertyAnimationRunner> Completed;
    }
}