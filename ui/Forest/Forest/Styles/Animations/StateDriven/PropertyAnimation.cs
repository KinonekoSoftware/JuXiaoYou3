using System.Windows.Media.Animation;

namespace Acorisoft.FutureGL.Forest.Styles.Animations
{
    public class PropertyAnimation : StateDrivenAnimation
    {
        private Storyboard _storyboard;

        /// <summary>
        /// 
        /// </summary>
        public override void NextState()
        {
            _storyboard?.Stop(TargetElement);
            _storyboard = null;
            
            foreach (var argument in Timelines)
            {
                argument.NextState();
            }
        }

        /// <summary>
        /// 进入下一个状态
        /// </summary>
        /// <param name="nextState">下一个状态</param>
        public override void NextState(VisualState nextState)
        {
            _storyboard?.Stop(TargetElement);
            _storyboard = new Storyboard();

            foreach (var argument in Timelines)
            {
                var animation = argument.NextState(nextState);

                if (animation is not null)
                {
                    Storyboard.SetTarget(animation, TargetElement);
                    _storyboard.Children.Add(animation);
                }
            }

            TargetElement.BeginStoryboard(_storyboard);
        }

        /// <summary>
        /// 目标元素。
        /// </summary>
        public FrameworkElement TargetElement { get; init; }

        /// <summary>
        /// 所有参数
        /// </summary>
        public IReadOnlyList<IPropertyAnimationRunner> Timelines { get; init; }
    }
}