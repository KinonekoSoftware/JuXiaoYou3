namespace Acorisoft.FutureGL.Forest.Styles.Animations
{
    /// <summary>
    /// 表示一个属性设置器。
    /// </summary>
    public class PropertySetter : StateDrivenAnimation
    {
        public sealed override void NextState(VisualState state)
        {
            if (!StateBasedSetter.TryGetValue(state, out var setterCollection))
            {
                return;
            }

            foreach (var setter in setterCollection)
            {
                TargetElement.SetValue(setter.Property, setter.Value);
            }
        }
        
        public sealed override void NextState()
        {
            foreach (var setter in Setters)
            {
                TargetElement.SetValue(setter.Property, setter.Value);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public IReadOnlyDictionary<VisualState, List<Setter>> StateBasedSetter { get; init; }

        /// <summary>
        /// 
        /// </summary>
        public IReadOnlyCollection<Setter> Setters { get; init; }
        
        /// <summary>
        /// 
        /// </summary>
        public FrameworkElement TargetElement { get; init; }
    }
}