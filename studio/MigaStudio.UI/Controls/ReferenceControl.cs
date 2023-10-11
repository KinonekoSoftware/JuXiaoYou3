using System.Windows.Input;

namespace Acorisoft.FutureGL.MigaStudio.Controls
{
    
    public class ActionButton : Button
    {
        public static readonly DependencyProperty    IconProperty;
        public static readonly DependencyProperty    IsFilledProperty;
        public static readonly DependencyProperty    IconSizeProperty;
        public static readonly DependencyPropertyKey HasIconPropertyKey;
        public static readonly DependencyProperty    HasIconProperty; 
        public static readonly DependencyProperty    CornerRadiusProperty;

        public static readonly DependencyProperty ShowArrowProperty;

        static ActionButton()
        {
            IconProperty = DependencyProperty.Register(
                nameof(Icon),
                typeof(Geometry),
                typeof(ActionButton),
                new PropertyMetadata(default(Geometry)));

            IsFilledProperty = DependencyProperty.Register(
                nameof(IsFilled),
                typeof(bool),
                typeof(ActionButton),
                new PropertyMetadata(Boxing.False));

            IconSizeProperty = DependencyProperty.Register(
                nameof(IconSize),
                typeof(double),
                typeof(ActionButton),
                new PropertyMetadata(17d));

            HasIconPropertyKey = DependencyProperty.RegisterReadOnly(
                nameof(HasIcon),
                typeof(bool),
                typeof(ActionButton),
                new PropertyMetadata(Boxing.False));

            
            CornerRadiusProperty = DependencyProperty.Register(
                nameof(CornerRadius),
                typeof(CornerRadius),
                typeof(ActionButton),
                new PropertyMetadata(default(CornerRadius)));
            ShowArrowProperty = DependencyProperty.Register(
                nameof(ShowArrow),
                typeof(bool),
                typeof(ActionButton),
                new PropertyMetadata(Boxing.False));
            HasIconProperty = HasIconPropertyKey.DependencyProperty;
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ActionButton), new FrameworkPropertyMetadata(typeof(ActionButton)));
        }


        public bool ShowArrow
        {
            get => (bool)GetValue(ShowArrowProperty);
            set => SetValue(ShowArrowProperty, Boxing.Box(value));
        }

        
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public bool HasIcon
        {
            get => (bool)GetValue(HasIconProperty);
            private set => SetValue(HasIconPropertyKey, value);
        }

        public double IconSize
        {
            get => (double)GetValue(IconSizeProperty);
            set => SetValue(IconSizeProperty, value);
        }

        public bool IsFilled
        {
            get => (bool)GetValue(IsFilledProperty);
            set => SetValue(IsFilledProperty, Boxing.Box(value));
        }

        public Geometry Icon
        {
            get => (Geometry)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }
    }
    
    public class ReferenceControl : Control
    {

        public static readonly DependencyProperty    SourceProperty;
        public static readonly DependencyProperty    IconProperty;
        public static readonly DependencyProperty    IsFilledProperty;
        public static readonly DependencyProperty    IconSizeProperty;
        public static readonly DependencyPropertyKey HasIconPropertyKey;
        public static readonly DependencyProperty    HasIconProperty; 
        public static readonly DependencyProperty    DisplayNameProperty;
        
        static ReferenceControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ReferenceControl), new FrameworkPropertyMetadata(typeof(ReferenceControl)));
            IconProperty = DependencyProperty.Register(
                nameof(Icon),
                typeof(Geometry),
                typeof(ReferenceControl),
                new PropertyMetadata(default(Geometry)));

            IsFilledProperty = DependencyProperty.Register(
                nameof(IsFilled),
                typeof(bool),
                typeof(ReferenceControl),
                new PropertyMetadata(Boxing.False));

            IconSizeProperty = DependencyProperty.Register(
                nameof(IconSize),
                typeof(double),
                typeof(ReferenceControl),
                new PropertyMetadata(17d));

            HasIconPropertyKey = DependencyProperty.RegisterReadOnly(
                nameof(HasIcon),
                typeof(bool),
                typeof(ReferenceControl),
                new PropertyMetadata(Boxing.False));

            
            HasIconProperty = HasIconPropertyKey.DependencyProperty;
            DisplayNameProperty = DependencyProperty.Register(
                nameof(DisplayName),
                typeof(string),
                typeof(ReferenceControl),
                new PropertyMetadata(default(string)));
            SourceProperty = DependencyProperty.Register(
                nameof(Source),
                typeof(string),
                typeof(ReferenceControl),
                new PropertyMetadata(default(string)));
        }


        public static readonly DependencyProperty OpenCommandProperty = DependencyProperty.Register(
            nameof(OpenCommand),
            typeof(ICommand),
            typeof(ReferenceControl),
            new PropertyMetadata(default(ICommand)));


        public static readonly DependencyProperty SelectCommandProperty = DependencyProperty.Register(
            nameof(SelectCommand),
            typeof(ICommand),
            typeof(ReferenceControl),
            new PropertyMetadata(default(ICommand)));

        public ICommand SelectCommand
        {
            get => (ICommand)GetValue(SelectCommandProperty);
            set => SetValue(SelectCommandProperty, value);
        }
        
        public ICommand OpenCommand
        {
            get => (ICommand)GetValue(OpenCommandProperty);
            set => SetValue(OpenCommandProperty, value);
        }

        public bool HasIcon
        {
            get => (bool)GetValue(HasIconProperty);
            private set => SetValue(HasIconPropertyKey, value);
        }

        public double IconSize
        {
            get => (double)GetValue(IconSizeProperty);
            set => SetValue(IconSizeProperty, value);
        }

        public bool IsFilled
        {
            get => (bool)GetValue(IsFilledProperty);
            set => SetValue(IsFilledProperty, Boxing.Box(value));
        }

        public Geometry Icon
        {
            get => (Geometry)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }
        public string Source
        {
            get => (string)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }
        public string DisplayName
        {
            get => (string)GetValue(DisplayNameProperty);
            set => SetValue(DisplayNameProperty, value);
        }
    }
}