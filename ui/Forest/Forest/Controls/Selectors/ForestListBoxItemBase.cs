namespace Acorisoft.FutureGL.Forest.Controls.Selectors
{
    public abstract partial class ForestListBoxItemBase : ListBoxItem, IForestControl, IForestIconControl
    {
        protected ForestListBoxItemBase()
        {
            Finder                           =  GetTemplateChild();
            StateMachine                     =  new VisualDFA();
            Loaded                           += OnLoadedIntern;
            Unloaded                         += OnUnloadedIntern;
            IsEnabledChanged                 += OnEnableChanged;
            StateMachine.StateChangedHandler =  HandleStateChanged;
            BuildState();
        }
        
        
        protected abstract void StopAnimation();
        protected abstract void SetForeground(Brush brush);
        protected abstract void OnInvalidateState();
        protected abstract void GoToNormalState(HighlightColorPalette palette, ForestThemeSystem theme);
        protected abstract void GoToHighlight1State(Duration duration, HighlightColorPalette palette, ForestThemeSystem theme);
        protected abstract void GoToHighlight2State(Duration duration, HighlightColorPalette palette, ForestThemeSystem theme);
        protected abstract void GoToDisableState(HighlightColorPalette palette, ForestThemeSystem theme);

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

        private void BuildState()
        {
            StateMachine.AddState(VisualState.Normal, VisualState.Highlight1, VisualStateTrigger.Next);
            StateMachine.AddState(VisualState.Highlight1, VisualState.Highlight2, VisualStateTrigger.Next);
            StateMachine.AddState(VisualState.Highlight2, VisualState.Normal, VisualStateTrigger.Next, false);
            StateMachine.AddState(VisualState.Highlight1, VisualState.Highlight1, VisualStateTrigger.Disabled);
            StateMachine.AddState(VisualState.Highlight2, VisualState.Highlight1, VisualStateTrigger.Disabled);
            StateMachine.AddState(VisualState.Normal, VisualState.Inactive, VisualStateTrigger.Disabled);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected ITemplatePartFinder GetTemplateChild() => new Finder(GetTemplateChild);

        


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
        /// 视觉状态机。
        /// </summary>
        protected VisualDFA StateMachine { get; }

        /// <summary>
        /// 模板查找器
        /// </summary>
        protected ITemplatePartFinder Finder { get; }
    }
    
    public abstract partial class ForestListViewItemBase : ListViewItem, IForestControl, IForestIconControl
    {
        protected ForestListViewItemBase()
        {
            Finder                           =  GetTemplateChild();
            StateMachine                     =  new VisualDFA();
            Loaded                           += OnLoadedIntern;
            Unloaded                         += OnUnloadedIntern;
            IsEnabledChanged                 += OnEnableChanged;
            StateMachine.StateChangedHandler =  HandleStateChanged;
            BuildState();
        }
        
        
        protected abstract void StopAnimation();
        protected abstract void SetForeground(Brush brush);
        protected abstract void OnInvalidateState();
        protected abstract void GoToNormalState(HighlightColorPalette palette, ForestThemeSystem theme);
        protected abstract void GoToHighlight1State(Duration duration, HighlightColorPalette palette, ForestThemeSystem theme);
        protected abstract void GoToHighlight2State(Duration duration, HighlightColorPalette palette, ForestThemeSystem theme);
        protected abstract void GoToDisableState(HighlightColorPalette palette, ForestThemeSystem theme);

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

        private void BuildState()
        {
            StateMachine.AddState(VisualState.Normal, VisualState.Highlight1, VisualStateTrigger.Next);
            StateMachine.AddState(VisualState.Highlight1, VisualState.Highlight2, VisualStateTrigger.Next);
            StateMachine.AddState(VisualState.Highlight2, VisualState.Normal, VisualStateTrigger.Next, false);
            StateMachine.AddState(VisualState.Highlight1, VisualState.Highlight1, VisualStateTrigger.Disabled);
            StateMachine.AddState(VisualState.Highlight2, VisualState.Highlight1, VisualStateTrigger.Disabled);
            StateMachine.AddState(VisualState.Normal, VisualState.Inactive, VisualStateTrigger.Disabled);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected ITemplatePartFinder GetTemplateChild() => new Finder(GetTemplateChild);

        


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
        /// 视觉状态机。
        /// </summary>
        protected VisualDFA StateMachine { get; }

        /// <summary>
        /// 模板查找器
        /// </summary>
        protected ITemplatePartFinder Finder { get; }
    }
}