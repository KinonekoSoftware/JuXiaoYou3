namespace Acorisoft.FutureGL.Forest.Controls.Buttons
{
    public abstract partial class ForestCheckBoxBase : CheckBox, IForestControl
    {
        protected ForestCheckBoxBase()
        {
            Finder = new Finder(GetTemplateChild);
            StateMachine = new VisualDFA
            {
                StateChangedHandler = HandleStateChanged
            };
            Loaded           += OnLoadedIntern;
            Unloaded         += OnUnloadedIntern;
            IsEnabledChanged += OnEnableChanged;
            Checked          += OnCheckedIntern;
            Unchecked        += OnUncheckedIntern;
            BuildState();
        }

        #region Initialize


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

        #endregion

        #region Button States

        protected abstract void StopAnimation();
        protected abstract void SetForeground(Brush brush);
        protected abstract void OnInvalidateState();


        private void HandleStateChanged(bool init, VisualState last, VisualState now, VisualStateTrigger value)
        {
            var palette = Palette;
            var theme   = ThemeSystem.Instance.Theme;

            // Stop Animation
            StopAnimation();

            if (!init)
            {
                if (IsEnabled)
                {
                    GoToNormalState(palette, theme);
                }
                else
                {
                    GoToDisableState(palette, theme);
                }

                return;
            }

            switch (now)
            {
                default:
                    GoToNormalState(palette, theme);
                    break;
                case VisualState.Highlight1:
                    GoToHighlight1State(theme.Duration.Medium, palette, theme);
                    break;
                case VisualState.Highlight2:
                    GoToHighlight2State(theme.Duration.Medium, palette, theme);
                    break;
                case VisualState.Inactive:
                    GoToDisableState(palette, theme);
                    break;
            }
        }

        protected abstract void GoToNormalState(HighlightColorPalette palette, ForestThemeSystem theme);
        protected abstract void GoToHighlight1State(Duration duration, HighlightColorPalette palette, ForestThemeSystem theme);
        protected abstract void GoToHighlight2State(Duration duration, HighlightColorPalette palette, ForestThemeSystem theme);
        protected abstract void GoToDisableState(HighlightColorPalette palette, ForestThemeSystem theme);

        #endregion

        #region Loaded / Unloaded

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

        private void OnUnloadedIntern(object sender, RoutedEventArgs e)
        {
            Checked          -= OnCheckedIntern;
            Unchecked        -= OnUncheckedIntern;
            Loaded           -= OnLoadedIntern;
            Unloaded         -= OnUnloadedIntern;
            IsEnabledChanged -= OnEnableChanged;
            OnUnloaded(sender, e);
        }

        #endregion

        protected abstract void GetTemplateChildOverride(ITemplatePartFinder finder);

        #region IForestControl Members

        public void InvalidateState()
        {
            if (IsLoaded)
            {
                OnInvalidateState();
                StateMachine.GotoState();
            }
        }

        #endregion

        #region ITextResourceAdapter Members

        void ITextResourceAdapter.SetText(string text)
        {
            Content = text;
        }

        void ITextResourceAdapter.SetToolTips(string text)
        {
            ToolTip = text;
        }

        #endregion

        /// <summary>
        /// 模板查找器
        /// </summary>
        protected ITemplatePartFinder Finder { get; }

        /// <summary>
        /// 视觉状态机。
        /// </summary>
        protected VisualDFA StateMachine { get; }
    }

    public abstract partial class ForestIconCheckBoxBase : ForestCheckBoxBase, IForestIconControl
    {
    }
}