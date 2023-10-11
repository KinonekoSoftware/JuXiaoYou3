namespace Acorisoft.FutureGL.Forest.Controls
{
    public abstract class ForestContentControlBase : ContentControl
    {
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            nameof(CornerRadius),
            typeof(CornerRadius),
            typeof(ForestContentControlBase),
            new PropertyMetadata(default(CornerRadius)));
        
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
    }
}