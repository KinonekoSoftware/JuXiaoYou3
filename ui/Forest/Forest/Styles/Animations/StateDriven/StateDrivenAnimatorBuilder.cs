

// ReSharper disable ForCanBeConvertedToForeach

namespace Acorisoft.FutureGL.Forest.Styles.Animations
{
    public class StateDrivenAnimatorBuilder : IStateDrivenAnimatorBuilder
    {
        public IStateDrivenTargetAndDefaultBuilder TargetAndDefault(FrameworkElement element)
        {
            var builder = new StateDrivenTargetBuilder
            {
                PendingList = new List<IStateDrivenAnimationBuilder>(16),
                AnimatorContext = this,
                SetterCollection = new List<Setter>(16),
                TargetElement = element
            };
            builder.Completed += OnTargetAndDefaultBuilderCompleted;
            PendingList.Add(builder);
            return builder;
        }

        public IStateDrivenTargetBuilder Target(FrameworkElement element)
        {
            var builder = new StateDrivenTargetBuilder
            {
                PendingList      = new List<IStateDrivenAnimationBuilder>(16),
                AnimatorContext  = this,
                ResultCollection = new List<IPropertyAnimationRunner>(16),
                TargetElement    = element
            };
            builder.Completed += OnTargetBuilderCompleted;
            PendingList.Add(builder);
            return builder;
        }

        private void OnTargetBuilderCompleted(IStateDrivenTargetBuilder builder, StateDrivenAnimation result)
        {
            builder.Completed -= OnTargetBuilderCompleted;
            if (result is not null)
            {
                Animations.Add(result);
            }
            PendingList.Remove(builder);
        }

        private void OnTargetAndDefaultBuilderCompleted(IStateDrivenTargetBuilder builder, StateDrivenAnimation result)
        {
            builder.Completed -= OnTargetAndDefaultBuilderCompleted;
            if (result is not null)
            {
                FirstState.Add(result);
            }
            PendingList.Remove(builder);
        }

        public StateDrivenAnimator Finish()
        {
            for (var i = 0; i < PendingList.Count; i++)
            {
                var pending = PendingList[i];
                pending.Finish();
            }
            
            return new StateDrivenAnimator
            {
                Animations = new List<StateDrivenAnimation>(Animations),
                FirstState = new List<StateDrivenAnimation>(FirstState)
            };
        }

        public IList<IStateDrivenTargetBuilder> PendingList { get; } = new List<IStateDrivenTargetBuilder>(8);

        /// <summary>
        /// 
        /// </summary>
        public IList<StateDrivenAnimation> FirstState { get; } = new List<StateDrivenAnimation>(16);

        /// <summary>
        /// 
        /// </summary>
        public IList<StateDrivenAnimation> Animations { get; } = new List<StateDrivenAnimation>(16);
    }
}