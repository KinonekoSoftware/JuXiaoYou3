namespace Acorisoft.FutureGL.Forest.Controls.Selectors
{
    partial class ForestListBoxItemBase
    {
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
        
        
        protected abstract void GetTemplateChildOverride(ITemplatePartFinder finder);

        public sealed override void OnApplyTemplate()
        {
            GetTemplateChildOverride(Finder);
            Finder.Find();
            StateMachine.NextState();
            OnApplyTemplateOverride();
            base.OnApplyTemplate();
        }
        private void OnUnloadedIntern(object sender, RoutedEventArgs e)
        {
            Loaded           -= OnLoadedIntern;
            Unloaded         -= OnUnloadedIntern;
            IsEnabledChanged -= OnEnableChanged;
            OnUnloaded(sender, e);
        }

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

        protected override void OnSelected(RoutedEventArgs e)
        {
            if (StateMachine.HasInitialized)
            {
                StateMachine.NextState(VisualState.Highlight2);
            }

            base.OnSelected(e);
        }

        protected override void OnUnselected(RoutedEventArgs e)
        {
            if (StateMachine.HasInitialized)
            {
                StateMachine.NextState(VisualState.Normal);
            }

            base.OnUnselected(e);
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            if (StateMachine.CurrentState != VisualState.Highlight2 && !IsSelected)
            {
                StateMachine.NextState(VisualState.Highlight1);
            }

            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            if (StateMachine.CurrentState == VisualState.Highlight1 && !IsSelected)
            {
                StateMachine.NextState(VisualState.Normal);
            }

            base.OnMouseLeave(e);
        }

        protected virtual void OnApplyTemplateOverride()
        {
            
        }
    }
    
    
    partial class ForestListViewItemBase
    {
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
        
        
        protected abstract void GetTemplateChildOverride(ITemplatePartFinder finder);

        public sealed override void OnApplyTemplate()
        {
            GetTemplateChildOverride(Finder);
            Finder.Find();
            StateMachine.NextState();
            OnApplyTemplateOverride();
            base.OnApplyTemplate();
        }
        private void OnUnloadedIntern(object sender, RoutedEventArgs e)
        {
            Loaded           -= OnLoadedIntern;
            Unloaded         -= OnUnloadedIntern;
            IsEnabledChanged -= OnEnableChanged;
            OnUnloaded(sender, e);
        }

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

        protected override void OnSelected(RoutedEventArgs e)
        {
            if (StateMachine.HasInitialized)
            {
                StateMachine.NextState(VisualState.Highlight2);
            }

            base.OnSelected(e);
        }

        protected override void OnUnselected(RoutedEventArgs e)
        {
            if (StateMachine.HasInitialized)
            {
                StateMachine.NextState(VisualState.Normal);
            }

            base.OnUnselected(e);
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            if (StateMachine.CurrentState != VisualState.Highlight2 && !IsSelected)
            {
                StateMachine.NextState(VisualState.Highlight1);
            }

            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            if (StateMachine.CurrentState == VisualState.Highlight1 && !IsSelected)
            {
                StateMachine.NextState(VisualState.Normal);
            }

            base.OnMouseLeave(e);
        }

        protected virtual void OnApplyTemplateOverride()
        {
            
        }
    }
    
    
    partial class ForestComboBoxItemBase
    {
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
        
        
        protected abstract void GetTemplateChildOverride(ITemplatePartFinder finder);

        public sealed override void OnApplyTemplate()
        {
            GetTemplateChildOverride(Finder);
            Finder.Find();
            StateMachine.NextState();
            OnApplyTemplateOverride();
            base.OnApplyTemplate();
        }
        private void OnUnloadedIntern(object sender, RoutedEventArgs e)
        {
            Loaded           -= OnLoadedIntern;
            Unloaded         -= OnUnloadedIntern;
            IsEnabledChanged -= OnEnableChanged;
            OnUnloaded(sender, e);
        }

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

        protected override void OnSelected(RoutedEventArgs e)
        {
            if (StateMachine.HasInitialized)
            {
                StateMachine.NextState(VisualState.Highlight2);
            }

            base.OnSelected(e);
        }

        protected override void OnUnselected(RoutedEventArgs e)
        {
            if (StateMachine.HasInitialized)
            {
                StateMachine.NextState(VisualState.Normal);
            }

            base.OnUnselected(e);
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            if (StateMachine.CurrentState != VisualState.Highlight2 && !IsSelected)
            {
                StateMachine.NextState(VisualState.Highlight1);
            }

            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            if (StateMachine.CurrentState == VisualState.Highlight1 && !IsSelected)
            {
                StateMachine.NextState(VisualState.Normal);
            }

            base.OnMouseLeave(e);
        }

        protected virtual void OnApplyTemplateOverride()
        {
            
        }
    }
    
    
    partial class ForestTabItemBase
    {
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
        
        
        protected abstract void GetTemplateChildOverride(ITemplatePartFinder finder);

        public sealed override void OnApplyTemplate()
        {
            GetTemplateChildOverride(Finder);
            Finder.Find();
            StateMachine.NextState();
            OnApplyTemplateOverride();
            base.OnApplyTemplate();
        }
        private void OnUnloadedIntern(object sender, RoutedEventArgs e)
        {
            Loaded           -= OnLoadedIntern;
            Unloaded         -= OnUnloadedIntern;
            IsEnabledChanged -= OnEnableChanged;
            OnUnloaded(sender, e);
        }

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

        protected override void OnSelected(RoutedEventArgs e)
        {
            if (StateMachine.HasInitialized)
            {
                StateMachine.NextState(VisualState.Highlight2);
            }

            base.OnSelected(e);
        }

        protected override void OnUnselected(RoutedEventArgs e)
        {
            if (StateMachine.HasInitialized)
            {
                StateMachine.NextState(VisualState.Normal);
            }

            base.OnUnselected(e);
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            if (StateMachine.CurrentState != VisualState.Highlight2 && !IsSelected)
            {
                StateMachine.NextState(VisualState.Highlight1);
            }

            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            if (StateMachine.CurrentState == VisualState.Highlight1 && !IsSelected)
            {
                StateMachine.NextState(VisualState.Normal);
            }

            base.OnMouseLeave(e);
        }

        protected virtual void OnApplyTemplateOverride()
        {
            
        }
    }
    
    
    partial class ForestTreeViewItemBase
    {
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
        
        
        protected abstract void GetTemplateChildOverride(ITemplatePartFinder finder);

        public sealed override void OnApplyTemplate()
        {
            GetTemplateChildOverride(Finder);
            Finder.Find();
            StateMachine.NextState();
            OnApplyTemplateOverride();
            base.OnApplyTemplate();
        }
        private void OnUnloadedIntern(object sender, RoutedEventArgs e)
        {
            Loaded           -= OnLoadedIntern;
            Unloaded         -= OnUnloadedIntern;
            IsEnabledChanged -= OnEnableChanged;
            OnUnloaded(sender, e);
        }

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

        protected override void OnSelected(RoutedEventArgs e)
        {
            if (StateMachine.HasInitialized)
            {
                StateMachine.NextState(VisualState.Highlight2);
            }

            base.OnSelected(e);
        }

        protected override void OnUnselected(RoutedEventArgs e)
        {
            if (StateMachine.HasInitialized)
            {
                StateMachine.NextState(VisualState.Normal);
            }

            base.OnUnselected(e);
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            if (StateMachine.CurrentState != VisualState.Highlight2 && !IsSelected)
            {
                StateMachine.NextState(VisualState.Highlight1);
            }

            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            if (StateMachine.CurrentState == VisualState.Highlight1 && !IsSelected)
            {
                StateMachine.NextState(VisualState.Normal);
            }

            base.OnMouseLeave(e);
        }

        protected virtual void OnApplyTemplateOverride()
        {
            
        }
    }
    
    
    partial class ForestMenuItemBase
    {
            
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
            Loaded           -= OnLoadedIntern;
            Unloaded         -= OnUnloadedIntern;
            IsEnabledChanged -= OnEnableChanged;
            OnUnloaded(sender, e);
        }

        #endregion
        
        #region Button State Defnitions

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


        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (StateMachine.CurrentState != VisualState.Highlight2)
            {
                StateMachine.NextState(VisualStateTrigger.Next);
            }

            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            if (StateMachine.CurrentState is VisualState.Highlight2 && IsPressed)
            {
                StateMachine.NextState(IsMouseOver ? VisualStateTrigger.Back : VisualStateTrigger.Next);
            }

            base.OnMouseUp(e);
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            if (StateMachine.CurrentState != VisualState.Highlight1)
            {
                StateMachine.NextState(VisualStateTrigger.Next);
            }

            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            ReleaseMouseCapture();
            StateMachine.ResetState();
            base.OnMouseLeave(e);
        }

        public override void OnApplyTemplate()
        {
            GetTemplateChildOverride(Finder);
            Finder.Find();
            StateMachine.NextState();
            OnApplyTemplateOverride();
            base.OnApplyTemplate();
        }

        #endregion
        
        protected virtual void OnApplyTemplateOverride()
        {
            
        }
    }
}