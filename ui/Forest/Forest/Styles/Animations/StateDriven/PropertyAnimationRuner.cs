using System.Windows.Media.Animation;


namespace Acorisoft.FutureGL.Forest.Styles.Animations
{
    public abstract class PropertyAnimationRuner : IPropertyAnimationRunner
    {
        /// <summary>
        /// 
        /// </summary>
        public abstract void NextState();

        /// <summary>
        /// 进入下一个状态
        /// </summary>
        /// <param name="nextState">下一个状态</param>
        public abstract AnimationTimeline NextState(VisualState nextState);

        /// <summary>
        /// 目标路径
        /// </summary>
        public PropertyPath PropertyPath { get; init; }

        /// <summary>
        /// 时长
        /// </summary>
        public Duration Duration { get; init; }
    }

    public abstract class PropertyAnimationRuner<T> : PropertyAnimationRuner, IPropertyAnimationRunner
    {
        private VisualState _now;
        private VisualState _last;
        private T           _lastValue;
        private T           _nowValue;

        public override void NextState()
        {
            _now       = VisualState.Normal;
            _last      = VisualState.Normal;
            _lastValue = Mapper[_now];
            _nowValue  = Mapper[_now];
        }

        public override AnimationTimeline NextState(VisualState nextState)
        {
            if (!Mapper.TryGetValue(nextState, out var newValue))
            {
                return null;
            }

            //   last       now
            // highlight1 highlight2
            //
            //

            var animation = CreateTimeline(nextState == _last ? _lastValue : _nowValue, newValue, Duration);
            Storyboard.SetTargetProperty(animation, PropertyPath);
            _last      = _now;
            _now       = nextState;
            _lastValue = _nowValue;
            _nowValue  = newValue;

            return animation;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        protected abstract AnimationTimeline CreateTimeline(T from, T to, Duration duration);

        /// <summary>
        /// 映射器
        /// </summary>
        public Dictionary<VisualState, T> Mapper { get; init; }
    }

    public sealed class ColorAnimationRuner : PropertyAnimationRuner<Color?>
    {
        protected override AnimationTimeline CreateTimeline(Color? from, Color? to, Duration duration)
        {
            return new ColorAnimation
            {
                From     = from,
                To       = to,
                Duration = duration
            };
        }
    }
    
    public sealed class ThicknessAnimationRuner : PropertyAnimationRuner<Thickness?>
    {
        protected override AnimationTimeline CreateTimeline(Thickness? from, Thickness? to, Duration duration)
        {
            return new ThicknessAnimation
            {
                From     = from,
                To       = to,
                Duration = duration
            };
        }
    }

    public sealed class ObjectAnimationRuner : PropertyAnimationRuner<object>
    {
        protected override AnimationTimeline CreateTimeline(object from, object to, Duration duration)
        {
            return null;
        }
    }

    public sealed class DoubleAnimationRuner : PropertyAnimationRuner<double?>
    {
        protected override AnimationTimeline CreateTimeline(double? from, double? to, Duration duration)
        {
            return new DoubleAnimation
            {
                From     = from,
                To       = to,
                Duration = duration
            };
        }
    }
}