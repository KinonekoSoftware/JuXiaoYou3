namespace Acorisoft.FutureGL.Forest.Controls.Selectors
{
    public abstract class ForestTreeViewBase : TreeView, ITextResourceAdapter
    {
        public static readonly DependencyProperty BindableSelectedItemProperty = DependencyProperty.Register(
            nameof(BindableSelectedItem),
            typeof(object),
            typeof(ForestTreeViewBase),
            new PropertyMetadata(default(object)));
        
        protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseRightButtonDown(e);
            
            var ancestor = Xaml.FindAncestor<TreeViewItem>(e.OriginalSource as FrameworkElement);

            if (ancestor is null)
            {
                return;
            }
            
            ancestor.IsSelected = true;
        }

        protected override void OnSelectedItemChanged(RoutedPropertyChangedEventArgs<object> e)
        {
            BindableSelectedItem = SelectedItem;
            base.OnSelectedItemChanged(e);
        }

        
        #region ITextResourceAdapter Members

        void ITextResourceAdapter.SetText(string text)
        {
        }

        void ITextResourceAdapter.SetToolTips(string text)
        {
            ToolTip = text;
        }

        #endregion

        public object BindableSelectedItem
        {
            get => (object)GetValue(BindableSelectedItemProperty);
            set => SetValue(BindableSelectedItemProperty, value);
        }
    }
}