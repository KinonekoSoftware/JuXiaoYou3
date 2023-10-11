namespace Acorisoft.FutureGL.Forest.Styles.Animations
{
    public class StateDrivenTargetBuilder : IStateDrivenTargetAndDefaultBuilder, IStateDrivenTargetBuilder
    {
        #region IStateDrivenTargetBuilder

        private static PropertyPath FromProperty(DependencyProperty property) => new PropertyPath("(0)", property);
        private static PropertyPath FromProperty(params object[] property) => new PropertyPath("(0).(1)", property);

        private T Capture<T>(T builder) where T : IStateDrivenAnimationBuilder
        {
            builder.Completed += OnBuilderCompleted;
            builder.Context   =  this;
            PendingList.Add(builder);
            return builder;
        }

        public IStateDrivenAnimationBuilder<Color?> Color(DependencyProperty property, Duration duration)
        {
            return Capture(new ColorPropertyBuilder
            {
                Duration     = duration,
                PropertyPath = FromProperty(property)
            });
        }

        public IStateDrivenAnimationBuilder<Color?> Color(Duration duration, params object[] property)
        {
            return Capture(new ColorPropertyBuilder
            {
                Duration     = duration,
                PropertyPath = FromProperty(property)
            });
        }

        public IStateDrivenAnimationBuilder<Color?> Foreground(Duration duration)
        {
            return Capture(new ColorPropertyBuilder
            {
                Duration     = duration,
                PropertyPath = new PropertyPath("(TextElement.Foreground).(SolidColorBrush.Color)")
            });
        }

        public IStateDrivenAnimationBuilder<double?> Double(DependencyProperty property, Duration duration)
        {
            return Capture(new DoublePropertyBuilder
            {
                Duration     = duration,
                PropertyPath = FromProperty(property)
            });
        }
        
        public IStateDrivenAnimationBuilder<double?> Double(Duration duration, params object[] property)
        {
            return Capture(new DoublePropertyBuilder
            {
                Duration     = duration,
                PropertyPath = FromProperty(property)
            });
        }

        public IStateDrivenAnimationBuilder<object> Object(DependencyProperty property, Duration duration)
        {
            return Capture(new ObjectPropertyBuilder
            {
                Duration     = duration,
                PropertyPath = FromProperty(property),
            });
        }

        public IStateDrivenAnimationBuilder<object> Object(Duration duration, params object[] property)
        {
            return Capture(new ObjectPropertyBuilder
            {
                Duration     = duration,
                PropertyPath = FromProperty(property)
            });
        }

        public IStateDrivenAnimationBuilder<Thickness?> Thickness(DependencyProperty property, Duration duration)
        {
            return Capture(new ThicknessPropertyBuilder
            {
                Duration     = duration,
                PropertyPath = FromProperty(property)
            });
        }
        
        public IStateDrivenAnimationBuilder<Thickness?> Thickness(Duration duration, params object[] property)
        {
            return Capture(new ThicknessPropertyBuilder
            {
                Duration     = duration,
                PropertyPath = FromProperty(property)
            });
        }
        

        #endregion

        public IStateDrivenTargetAndDefaultBuilder Set(DependencyProperty property, object value)
        {
            SetterCollection.Add(new Setter
            {
                Property = property,
                Value = value
            });
            return this;
        }

        private void OnBuilderCompleted(IStateDrivenAnimationBuilder sender, IPropertyAnimationRunner result)
        {
            sender.Completed -= OnBuilderCompleted;

            if (result is not null)
            {
                ResultCollection.Add(result);
            }

            PendingList.Remove(sender);
        }

        public Animator Finish()
        {
            if (ResultCollection is not null)
            {
                if (PendingList.Count > 0)
                {
                    // ReSharper disable once ForCanBeConvertedToForeach
                    for (var i = 0; i < PendingList.Count; i++)
                    {
                        var pending = PendingList[i];
                        pending.Finish();
                    }
                }

                var result = new PropertyAnimation
                {
                    Timelines     = new List<IPropertyAnimationRunner>(ResultCollection),
                    TargetElement = TargetElement
                };

                Completed?.Invoke(this, ResultCollection.Count > 0 ? result : null);
            }
            else
            {
                Completed?.Invoke(this, new PropertySetter
                {
                    TargetElement = TargetElement,
                    Setters = new List<Setter>(SetterCollection),
                });
            }

            return AnimatorContext.Finish();
        }

        void IStateDrivenTargetAndDefaultBuilder.Finish() => Finish();
        
        public IList<IStateDrivenAnimationBuilder> PendingList { get; init; }

        /// <summary>
        /// 构造器列表。
        /// </summary>
        public IList<IPropertyAnimationRunner> ResultCollection { get; init; }
        
        /// <summary>
        /// 
        /// </summary>
        public IList<Setter> SetterCollection { get; init; }
        
        /// <summary>
        /// 
        /// </summary>
        public IStateDrivenAnimatorBuilder AnimatorContext { get; init; }

        /// <summary>
        /// 
        /// </summary>
        public FrameworkElement TargetElement { get; init; }
        
        /// <summary>
        /// 事件
        /// </summary>
        public event Action<IStateDrivenTargetBuilder, StateDrivenAnimation> Completed;
    }
}