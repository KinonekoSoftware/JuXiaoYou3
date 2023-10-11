namespace Acorisoft.FutureGL.Forest.Controls
{
    public abstract class ForestTextBoxBase : TextBox, ITextResourceAdapter
    {
        public static readonly DependencyProperty    WatermarkProperty;
        public static readonly DependencyPropertyKey HasTextPropertyKey;
        public static readonly DependencyProperty    HasTextProperty;
        protected const        string                PART_BdName      = "PART_Bd";
        protected const        string                PART_ContentName = "PART_ContentHost";

        static ForestTextBoxBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ForestTextBoxBase), new FrameworkPropertyMetadata(typeof(ForestTextBoxBase)));
            
            WatermarkProperty = DependencyProperty.Register(
                nameof(Watermark),
                typeof(string),
                typeof(ForestTextBoxBase),
                new PropertyMetadata(default(string)));
            
            HasTextPropertyKey = DependencyProperty.RegisterReadOnly(
                nameof(HasText),
                typeof(bool),
                typeof(ForestTextBoxBase),
                new PropertyMetadata(Boxing.False));
            
            HasTextProperty = HasTextPropertyKey.DependencyProperty;
        }

        protected ForestTextBoxBase()
        {
            Finder           =  GetTemplateChild();
            StateMachine = new VisualDFA
            {
                StateChangedHandler = HandleStateChanged
            };
            Loaded           += OnLoadedIntern;
            Unloaded         += OnUnloadedIntern;
            IsEnabledChanged += OnEnableChanged;
            BuildState();
        }

        
        #region Button States

        protected abstract void StopAnimation();
        protected abstract void SetForeground(Brush brush);
        protected abstract void OnInvalidateState();



        private void HandleStateChanged(bool init, VisualState last, VisualState now, VisualStateTrigger value)
        {
            var theme   = ThemeSystem.Instance.Theme;

            // Stop Animation
            StopAnimation();

            if (!init)
            {
                if (IsEnabled)
                {
                    GoToNormalState(theme);
                }
                else
                {
                    GoToDisableState(theme);
                }

                return;
            }

            switch (now)
            {
                default:
                    GoToNormalState(theme);
                    break;
                case VisualState.Highlight1:
                    GoToHighlight1State(theme.Duration.Medium, theme);
                    break;
                case VisualState.Highlight2:
                    GoToHighlight2State(theme.Duration.Medium, theme);
                    break;
                case VisualState.Inactive:
                    GoToDisableState(theme);
                    break;
            }
        }

        protected abstract void GoToNormalState( ForestThemeSystem theme);
        protected abstract void GoToHighlight1State(Duration duration,  ForestThemeSystem theme);
        protected abstract void GoToHighlight2State(Duration duration,  ForestThemeSystem theme);
        protected abstract void GoToDisableState( ForestThemeSystem theme);

        #endregion

        /// <summary>
        /// 构建状态
        /// </summary>
        private void OnEnableChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var oldValue = (bool)e.OldValue;
            var newValue = (bool)e.NewValue;

            if (!StateMachine.HasInitialized)
            {
                return;
            }

            if (oldValue && !newValue)
            {
                StateMachine.NextState(VisualState.Inactive);
            }

            if (!oldValue && newValue)
            {
                StateMachine.NextState(VisualState.Normal);
            }
        }


        private void OnUnloadedIntern(object sender, RoutedEventArgs e)
        {
            Loaded           -= OnLoadedIntern;
            Unloaded         -= OnUnloadedIntern;
            IsEnabledChanged -= OnEnableChanged;
            OnUnloaded(sender, e);
        }

        /// <summary>
        /// 构建状态
        /// </summary>
        private void BuildState()
        {
            StateMachine.AddState(VisualState.Normal, VisualState.Highlight1, VisualStateTrigger.Next);
            StateMachine.AddState(VisualState.Highlight1, VisualState.Highlight2, VisualStateTrigger.Next);
            StateMachine.AddState(VisualState.Highlight2, VisualState.Normal, VisualStateTrigger.Next, false);
            StateMachine.AddState(VisualState.Highlight1, VisualState.Highlight1, VisualStateTrigger.Disabled);
            StateMachine.AddState(VisualState.Highlight2, VisualState.Highlight1, VisualStateTrigger.Disabled);
            StateMachine.AddState(VisualState.Normal, VisualState.Inactive, VisualStateTrigger.Disabled);
        }

        protected virtual void GetTemplateChildOverride(ITemplatePartFinder finder)
        {
            finder.Find<Border>(PART_BdName, x => PART_Bd                 = x)
                  .Find<ScrollViewer>(PART_ContentName, x => PART_Content = x);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected ITemplatePartFinder GetTemplateChild() => new Finder(GetTemplateChild);

        private void OnLoadedIntern(object sender, RoutedEventArgs e)
        {
            OnLoaded(sender, e);
        }

        protected virtual void OnUnloaded(object sender, RoutedEventArgs e)
        {
        }

        protected virtual void OnLoaded(object sender, RoutedEventArgs e)
        {
        }


        protected override void OnGotFocus(RoutedEventArgs e)
        {
            StateMachine.NextState(VisualState.Highlight2);
            base.OnGotFocus(e);
        }


        protected override void OnLostFocus(RoutedEventArgs e)
        {
            if (StateMachine.CurrentState == VisualState.Highlight2)
            {
                StateMachine.NextState(IsMouseOver ? VisualStateTrigger.Back : VisualStateTrigger.Next);
            }

            base.OnLostFocus(e);
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            if (!IsKeyboardFocused)
            {
                StateMachine.NextState(VisualStateTrigger.Next);
            }

            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            if (StateMachine.CurrentState == VisualState.Highlight1 && !IsKeyboardFocused)
            {
                ReleaseMouseCapture();
                StateMachine.ResetState();
            }

            base.OnMouseLeave(e);
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            HasText = !string.IsNullOrEmpty(Text);
            base.OnTextChanged(e);
        }
        

        void ITextResourceAdapter.SetText(string text)
        {
        }

        void ITextResourceAdapter.SetToolTips(string text)
        {
            ToolTip = text;
        }


        public sealed override void OnApplyTemplate()
        {
            GetTemplateChildOverride(Finder);
            Finder.Find();
            StateMachine.NextState();
            OnApplyTemplateOverride();
            base.OnApplyTemplate();
        }
        
        
        
        protected virtual void OnApplyTemplateOverride()
        {
            
        }

        /// <summary>
        /// 模板查找器
        /// </summary>
        protected ITemplatePartFinder Finder { get; }

        /// <summary>
        /// 视觉状态机。
        /// </summary>
        protected VisualDFA StateMachine { get; }

        protected Border PART_Bd { get; set; }
        protected ScrollViewer PART_Content { get; set; }
        

        public bool HasText
        {
            get => (bool)GetValue(HasTextProperty);
            private set => SetValue(HasTextPropertyKey, Boxing.Box(value));
        }

        /// <summary>
        /// 动画工具
        /// </summary>
        public string Watermark
        {
            get => (string)GetValue(WatermarkProperty);
            set => SetValue(WatermarkProperty, value);
        }
    }
}