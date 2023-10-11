namespace Acorisoft.FutureGL.Forest.Controls.Buttons
{
    #region ForestButton

    
    public abstract partial class ForestIconButtonBase 
    {
        public static readonly DependencyProperty    IconProperty;
        public static readonly DependencyProperty    IsFilledProperty;
        public static readonly DependencyProperty    IconSizeProperty;
        public static readonly DependencyPropertyKey HasIconPropertyKey;
        public static readonly DependencyProperty    HasIconProperty;

        static ForestIconButtonBase()
        {
            IconProperty = DependencyProperty.Register(
                nameof(Icon),
                typeof(Geometry),
                typeof(ForestIconButtonBase),
                new PropertyMetadata(default(Geometry), OnIconChanged));

            IsFilledProperty = DependencyProperty.Register(
                nameof(IsFilled),
                typeof(bool),
                typeof(ForestIconButtonBase),
                new PropertyMetadata(Boxing.False, OnIsFilledChanged));

            IconSizeProperty = DependencyProperty.Register(
                nameof(IconSize),
                typeof(double),
                typeof(ForestIconButtonBase),
                new PropertyMetadata(17d));

            HasIconPropertyKey = DependencyProperty.RegisterReadOnly(
                nameof(HasIcon),
                typeof(bool),
                typeof(ForestIconButtonBase),
                new PropertyMetadata(Boxing.False));

            HasIconProperty = HasIconPropertyKey.DependencyProperty;
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ForestIconButtonBase), new FrameworkPropertyMetadata(typeof(ForestIconButtonBase)));
        }

        private static void OnIsFilledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = ((ForestIconButtonBase)d);
            ctrl.HasIcon = e.NewValue is Geometry;
            ctrl.InvalidateState();
        }

        private static void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = ((ForestIconButtonBase)d);
            ctrl.HasIcon = e.NewValue is Geometry;
            ctrl.InvalidateState();
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

    partial class ForestButtonBase
    {
        public static readonly DependencyProperty PaletteProperty;
        public static readonly DependencyProperty CornerRadiusProperty;

        static ForestButtonBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ForestButtonBase), new FrameworkPropertyMetadata(typeof(ForestButtonBase)));
            
            PaletteProperty = DependencyProperty.Register(
                nameof(Palette),
                typeof(HighlightColorPalette),
                typeof(ForestButtonBase),
                new PropertyMetadata(default(HighlightColorPalette), OnPaletteChanged));
            
            CornerRadiusProperty = DependencyProperty.Register(
                nameof(CornerRadius),
                typeof(CornerRadius),
                typeof(ForestButtonBase),
                new PropertyMetadata(default(CornerRadius)));
        }

        private static void OnPaletteChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ForestButtonBase)d).InvalidateState();
        }

        public HighlightColorPalette Palette
        {
            get => (HighlightColorPalette)GetValue(PaletteProperty);
            set => SetValue(PaletteProperty, value);
        }

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
    }
    
    #endregion
    
    #region ForestCheckBox

    
    public abstract partial class ForestIconCheckBoxBase
    {
        public static readonly DependencyProperty    IconProperty;
        public static readonly DependencyProperty    IsFilledProperty;
        public static readonly DependencyProperty    IconSizeProperty;
        public static readonly DependencyPropertyKey HasIconPropertyKey;
        public static readonly DependencyProperty    HasIconProperty;

        static ForestIconCheckBoxBase()
        {
            IconProperty = DependencyProperty.Register(
                nameof(Icon),
                typeof(Geometry),
                typeof(ForestIconCheckBoxBase),
                new PropertyMetadata(default(Geometry), OnIconChanged));

            IsFilledProperty = DependencyProperty.Register(
                nameof(IsFilled),
                typeof(bool),
                typeof(ForestIconCheckBoxBase),
                new PropertyMetadata(Boxing.False, OnIsFilledChanged));

            IconSizeProperty = DependencyProperty.Register(
                nameof(IconSize),
                typeof(double),
                typeof(ForestIconCheckBoxBase),
                new PropertyMetadata(17d));

            HasIconPropertyKey = DependencyProperty.RegisterReadOnly(
                nameof(HasIcon),
                typeof(bool),
                typeof(ForestIconCheckBoxBase),
                new PropertyMetadata(Boxing.False));

            HasIconProperty = HasIconPropertyKey.DependencyProperty;
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ForestIconCheckBoxBase), new FrameworkPropertyMetadata(typeof(ForestIconCheckBoxBase)));
        }
        
        private static void OnIsFilledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = ((ForestIconCheckBoxBase)d);
            ctrl.HasIcon = e.NewValue is Geometry;
            ctrl.InvalidateState();
        }

        private static void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = ((ForestIconCheckBoxBase)d);
            ctrl.HasIcon = e.NewValue is Geometry;
            ctrl.InvalidateState();
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

    partial class ForestCheckBoxBase
    {
        public static readonly DependencyProperty PaletteProperty;
        public static readonly DependencyProperty CornerRadiusProperty;

        static ForestCheckBoxBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ForestCheckBoxBase), new FrameworkPropertyMetadata(typeof(ForestCheckBoxBase)));
            
            PaletteProperty = DependencyProperty.Register(
                nameof(Palette),
                typeof(HighlightColorPalette),
                typeof(ForestCheckBoxBase),
                new PropertyMetadata(default(HighlightColorPalette), OnPaletteChanged));
            
            CornerRadiusProperty = DependencyProperty.Register(
                nameof(CornerRadius),
                typeof(CornerRadius),
                typeof(ForestCheckBoxBase),
                new PropertyMetadata(default(CornerRadius)));
        }

        private static void OnPaletteChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ForestCheckBoxBase)d).InvalidateState();
        }

        public HighlightColorPalette Palette
        {
            get => (HighlightColorPalette)GetValue(PaletteProperty);
            set => SetValue(PaletteProperty, value);
        }

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
    }
    
    #endregion

    #region ForestRadioButton

    
    public abstract partial class ForestIconRadioButtonBase
    {
        public static readonly DependencyProperty    IconProperty;
        public static readonly DependencyProperty    IsFilledProperty;
        public static readonly DependencyProperty    IconSizeProperty;
        public static readonly DependencyPropertyKey HasIconPropertyKey;
        public static readonly DependencyProperty    HasIconProperty;

        static ForestIconRadioButtonBase()
        {
            IconProperty = DependencyProperty.Register(
                nameof(Icon),
                typeof(Geometry),
                typeof(ForestIconRadioButtonBase),
                new PropertyMetadata(default(Geometry), OnIconChanged));

            IsFilledProperty = DependencyProperty.Register(
                nameof(IsFilled),
                typeof(bool),
                typeof(ForestIconRadioButtonBase),
                new PropertyMetadata(Boxing.False, OnIsFilledChanged));

            IconSizeProperty = DependencyProperty.Register(
                nameof(IconSize),
                typeof(double),
                typeof(ForestIconRadioButtonBase),
                new PropertyMetadata(17d));

            HasIconPropertyKey = DependencyProperty.RegisterReadOnly(
                nameof(HasIcon),
                typeof(bool),
                typeof(ForestIconRadioButtonBase),
                new PropertyMetadata(Boxing.False));

            HasIconProperty = HasIconPropertyKey.DependencyProperty;
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ForestIconRadioButtonBase), new FrameworkPropertyMetadata(typeof(ForestIconRadioButtonBase)));
        }
        
        private static void OnIsFilledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = ((ForestIconRadioButtonBase)d);
            ctrl.HasIcon = e.NewValue is Geometry;
            ctrl.InvalidateState();
        }

        private static void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = ((ForestIconRadioButtonBase)d);
            ctrl.HasIcon = e.NewValue is Geometry;
            ctrl.InvalidateState();
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

    partial class ForestRadioButtonBase
    {
        public static readonly DependencyProperty PaletteProperty;
        public static readonly DependencyProperty CornerRadiusProperty;

        static ForestRadioButtonBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ForestRadioButtonBase), new FrameworkPropertyMetadata(typeof(ForestRadioButtonBase)));
            
            PaletteProperty = DependencyProperty.Register(
                nameof(Palette),
                typeof(HighlightColorPalette),
                typeof(ForestRadioButtonBase),
                new PropertyMetadata(default(HighlightColorPalette), OnPaletteChanged));
            
            CornerRadiusProperty = DependencyProperty.Register(
                nameof(CornerRadius),
                typeof(CornerRadius),
                typeof(ForestRadioButtonBase),
                new PropertyMetadata(default(CornerRadius)));
        }

        private static void OnPaletteChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ForestRadioButtonBase)d).InvalidateState();
        }

        public HighlightColorPalette Palette
        {
            get => (HighlightColorPalette)GetValue(PaletteProperty);
            set => SetValue(PaletteProperty, value);
        }

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
    }
    
    #endregion

    #region ForestRepeatButton

    
    public abstract partial class ForestIconRepeatButtonBase
    {
        public static readonly DependencyProperty    IconProperty;
        public static readonly DependencyProperty    IsFilledProperty;
        public static readonly DependencyProperty    IconSizeProperty;
        public static readonly DependencyPropertyKey HasIconPropertyKey;
        public static readonly DependencyProperty    HasIconProperty;

        static ForestIconRepeatButtonBase()
        {
            IconProperty = DependencyProperty.Register(
                nameof(Icon),
                typeof(Geometry),
                typeof(ForestIconRepeatButtonBase),
                new PropertyMetadata(default(Geometry), OnIconChanged));

            IsFilledProperty = DependencyProperty.Register(
                nameof(IsFilled),
                typeof(bool),
                typeof(ForestIconRepeatButtonBase),
                new PropertyMetadata(Boxing.False));

            IconSizeProperty = DependencyProperty.Register(
                nameof(IconSize),
                typeof(double),
                typeof(ForestIconRepeatButtonBase),
                new PropertyMetadata(17d));

            HasIconPropertyKey = DependencyProperty.RegisterReadOnly(
                nameof(HasIcon),
                typeof(bool),
                typeof(ForestIconRepeatButtonBase),
                new PropertyMetadata(Boxing.False));

            HasIconProperty = HasIconPropertyKey.DependencyProperty;
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ForestIconRepeatButtonBase), new FrameworkPropertyMetadata(typeof(ForestIconRepeatButtonBase)));
        }

        private static void OnIsFilledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = ((ForestIconRepeatButtonBase)d);
            ctrl.HasIcon = e.NewValue is Geometry;
            ctrl.InvalidateState();
        }

        private static void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = ((ForestIconRepeatButtonBase)d);
            ctrl.HasIcon = e.NewValue is Geometry;
            ctrl.InvalidateState();
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

    partial class ForestRepeatButtonBase
    {
        public static readonly DependencyProperty PaletteProperty;
        public static readonly DependencyProperty CornerRadiusProperty;

        static ForestRepeatButtonBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ForestRepeatButtonBase), new FrameworkPropertyMetadata(typeof(ForestRepeatButtonBase)));
            
            PaletteProperty = DependencyProperty.Register(
                nameof(Palette),
                typeof(HighlightColorPalette),
                typeof(ForestRepeatButtonBase),
                new PropertyMetadata(default(HighlightColorPalette), OnPaletteChanged));
            
            CornerRadiusProperty = DependencyProperty.Register(
                nameof(CornerRadius),
                typeof(CornerRadius),
                typeof(ForestRepeatButtonBase),
                new PropertyMetadata(default(CornerRadius)));
        }

        private static void OnPaletteChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ForestRepeatButtonBase)d).InvalidateState();
        }

        public HighlightColorPalette Palette
        {
            get => (HighlightColorPalette)GetValue(PaletteProperty);
            set => SetValue(PaletteProperty, value);
        }

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
    }
    
    #endregion

    #region ForestToggleButton

    
    public abstract partial class ForestIconToggleButtonBase
    {
        public static readonly DependencyProperty    IconProperty;
        public static readonly DependencyProperty    IsFilledProperty;
        public static readonly DependencyProperty    IconSizeProperty;
        public static readonly DependencyPropertyKey HasIconPropertyKey;
        public static readonly DependencyProperty    HasIconProperty;

        static ForestIconToggleButtonBase()
        {
            IconProperty = DependencyProperty.Register(
                nameof(Icon),
                typeof(Geometry),
                typeof(ForestIconToggleButtonBase),
                new PropertyMetadata(default(Geometry), OnIconChanged));

            IsFilledProperty = DependencyProperty.Register(
                nameof(IsFilled),
                typeof(bool),
                typeof(ForestIconToggleButtonBase),
                new PropertyMetadata(Boxing.False, OnIsFilledChanged));

            IconSizeProperty = DependencyProperty.Register(
                nameof(IconSize),
                typeof(double),
                typeof(ForestIconToggleButtonBase),
                new PropertyMetadata(17d));

            HasIconPropertyKey = DependencyProperty.RegisterReadOnly(
                nameof(HasIcon),
                typeof(bool),
                typeof(ForestIconToggleButtonBase),
                new PropertyMetadata(Boxing.False));

            HasIconProperty = HasIconPropertyKey.DependencyProperty;
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ForestIconToggleButtonBase), new FrameworkPropertyMetadata(typeof(ForestIconToggleButtonBase)));
        }

        
        private static void OnIsFilledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = ((ForestIconToggleButtonBase)d);
            ctrl.HasIcon = e.NewValue is Geometry;
            ctrl.InvalidateState();
        }

        private static void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = ((ForestIconToggleButtonBase)d);
            ctrl.HasIcon = e.NewValue is Geometry;
            ctrl.InvalidateState();
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

    partial class ForestToggleButtonBase
    {
        public static readonly DependencyProperty PaletteProperty;
        public static readonly DependencyProperty CornerRadiusProperty;

        static ForestToggleButtonBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ForestToggleButtonBase), new FrameworkPropertyMetadata(typeof(ForestToggleButtonBase)));
            
            PaletteProperty = DependencyProperty.Register(
                nameof(Palette),
                typeof(HighlightColorPalette),
                typeof(ForestToggleButtonBase),
                new PropertyMetadata(default(HighlightColorPalette), OnPaletteChanged));
            
            CornerRadiusProperty = DependencyProperty.Register(
                nameof(CornerRadius),
                typeof(CornerRadius),
                typeof(ForestToggleButtonBase),
                new PropertyMetadata(default(CornerRadius)));
        }

        private static void OnPaletteChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ForestToggleButtonBase)d).InvalidateState();
        }

        public HighlightColorPalette Palette
        {
            get => (HighlightColorPalette)GetValue(PaletteProperty);
            set => SetValue(PaletteProperty, value);
        }

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
    }
    
    #endregion
}