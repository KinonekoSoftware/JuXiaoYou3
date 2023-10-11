namespace Acorisoft.FutureGL.Forest.Controls.Selectors
{
    #region ForestListBoxItem

    public abstract partial class ForestListBoxItemBase
    {
        public static readonly DependencyProperty    IconProperty;
        public static readonly DependencyProperty    IsFilledProperty;
        public static readonly DependencyProperty    IconSizeProperty;
        public static readonly DependencyPropertyKey HasIconPropertyKey;
        public static readonly DependencyProperty    HasIconProperty;
        public static readonly DependencyProperty    PlacementProperty;
        public static readonly DependencyProperty    PaletteProperty;
        public static readonly DependencyProperty    CornerRadiusProperty;

        static ForestListBoxItemBase()
        {
            IconProperty = DependencyProperty.Register(
                nameof(Icon),
                typeof(Geometry),
                typeof(ForestListBoxItemBase),
                new PropertyMetadata(default(Geometry), OnIconChanged));

            IsFilledProperty = DependencyProperty.Register(
                nameof(IsFilled),
                typeof(bool),
                typeof(ForestListBoxItemBase),
                new PropertyMetadata(Boxing.False, OnIsFilledChanged));

            IconSizeProperty = DependencyProperty.Register(
                nameof(IconSize),
                typeof(double),
                typeof(ForestListBoxItemBase),
                new PropertyMetadata(17d));

            HasIconPropertyKey = DependencyProperty.RegisterReadOnly(
                nameof(HasIcon),
                typeof(bool),
                typeof(ForestListBoxItemBase),
                new PropertyMetadata(Boxing.False));

            PaletteProperty = DependencyProperty.Register(
                nameof(Palette),
                typeof(HighlightColorPalette),
                typeof(ForestListBoxItemBase),
                new PropertyMetadata(default(HighlightColorPalette), OnPaletteChanged));

            CornerRadiusProperty = DependencyProperty.Register(
                nameof(CornerRadius),
                typeof(CornerRadius),
                typeof(ForestListBoxItemBase),
                new PropertyMetadata(default(CornerRadius)));

            PlacementProperty = DependencyProperty.Register(
                nameof(Placement),
                typeof(Dock),
                typeof(ForestListBoxItemBase),
                new PropertyMetadata(Dock.Left));

            HasIconProperty = HasIconPropertyKey.DependencyProperty;
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ForestListBoxItemBase), new FrameworkPropertyMetadata(typeof(ForestListBoxItemBase)));
        }


        private static void OnPaletteChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ForestListBoxItemBase)d).InvalidateState();
        }

        private static void OnIsFilledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = ((ForestListBoxItemBase)d);
            ctrl.HasIcon = e.NewValue is Geometry;
            ctrl.InvalidateState();
        }

        private static void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = ((ForestListBoxItemBase)d);
            ctrl.HasIcon = e.NewValue is Geometry;
            ctrl.InvalidateState();
        }

        public Dock Placement
        {
            get => (Dock)GetValue(PlacementProperty);
            set => SetValue(PlacementProperty, value);
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

    #region ForestComboBoxItem

    public abstract partial class ForestComboBoxItemBase
    {
        public static readonly DependencyProperty    IconProperty;
        public static readonly DependencyProperty    IsFilledProperty;
        public static readonly DependencyProperty    IconSizeProperty;
        public static readonly DependencyPropertyKey HasIconPropertyKey;
        public static readonly DependencyProperty    HasIconProperty;
        public static readonly DependencyProperty    PlacementProperty;
        public static readonly DependencyProperty    PaletteProperty;
        public static readonly DependencyProperty    CornerRadiusProperty;

        static ForestComboBoxItemBase()
        {
            IconProperty = DependencyProperty.Register(
                nameof(Icon),
                typeof(Geometry),
                typeof(ForestComboBoxItemBase),
                new PropertyMetadata(default(Geometry), OnIconChanged));

            IsFilledProperty = DependencyProperty.Register(
                nameof(IsFilled),
                typeof(bool),
                typeof(ForestComboBoxItemBase),
                new PropertyMetadata(Boxing.False, OnIsFilledChanged));

            IconSizeProperty = DependencyProperty.Register(
                nameof(IconSize),
                typeof(double),
                typeof(ForestComboBoxItemBase),
                new PropertyMetadata(17d));

            HasIconPropertyKey = DependencyProperty.RegisterReadOnly(
                nameof(HasIcon),
                typeof(bool),
                typeof(ForestComboBoxItemBase),
                new PropertyMetadata(Boxing.False));

            PaletteProperty = DependencyProperty.Register(
                nameof(Palette),
                typeof(HighlightColorPalette),
                typeof(ForestComboBoxItemBase),
                new PropertyMetadata(default(HighlightColorPalette), OnPaletteChanged));

            CornerRadiusProperty = DependencyProperty.Register(
                nameof(CornerRadius),
                typeof(CornerRadius),
                typeof(ForestComboBoxItemBase),
                new PropertyMetadata(default(CornerRadius)));

            PlacementProperty = DependencyProperty.Register(
                nameof(Placement),
                typeof(Dock),
                typeof(ForestComboBoxItemBase),
                new PropertyMetadata(Dock.Left));

            HasIconProperty = HasIconPropertyKey.DependencyProperty;
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ForestComboBoxItemBase), new FrameworkPropertyMetadata(typeof(ForestComboBoxItemBase)));
        }


        private static void OnPaletteChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ForestComboBoxItemBase)d).InvalidateState();
        }

        private static void OnIsFilledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = ((ForestComboBoxItemBase)d);
            ctrl.HasIcon = e.NewValue is Geometry;
            ctrl.InvalidateState();
        }

        private static void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = ((ForestComboBoxItemBase)d);
            ctrl.HasIcon = e.NewValue is Geometry;
            ctrl.InvalidateState();
        }

        public Dock Placement
        {
            get => (Dock)GetValue(PlacementProperty);
            set => SetValue(PlacementProperty, value);
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

    #region ForestListViewItem

    public abstract partial class ForestListViewItemBase
    {
        public static readonly DependencyProperty    IconProperty;
        public static readonly DependencyProperty    IsFilledProperty;
        public static readonly DependencyProperty    IconSizeProperty;
        public static readonly DependencyPropertyKey HasIconPropertyKey;
        public static readonly DependencyProperty    HasIconProperty;
        public static readonly DependencyProperty    PlacementProperty;
        public static readonly DependencyProperty    PaletteProperty;
        public static readonly DependencyProperty    CornerRadiusProperty;

        static ForestListViewItemBase()
        {
            IconProperty = DependencyProperty.Register(
                nameof(Icon),
                typeof(Geometry),
                typeof(ForestListViewItemBase),
                new PropertyMetadata(default(Geometry), OnIconChanged));

            IsFilledProperty = DependencyProperty.Register(
                nameof(IsFilled),
                typeof(bool),
                typeof(ForestListViewItemBase),
                new PropertyMetadata(Boxing.False, OnIsFilledChanged));

            IconSizeProperty = DependencyProperty.Register(
                nameof(IconSize),
                typeof(double),
                typeof(ForestListViewItemBase),
                new PropertyMetadata(17d));

            HasIconPropertyKey = DependencyProperty.RegisterReadOnly(
                nameof(HasIcon),
                typeof(bool),
                typeof(ForestListViewItemBase),
                new PropertyMetadata(Boxing.False));

            PaletteProperty = DependencyProperty.Register(
                nameof(Palette),
                typeof(HighlightColorPalette),
                typeof(ForestListViewItemBase),
                new PropertyMetadata(default(HighlightColorPalette), OnPaletteChanged));

            CornerRadiusProperty = DependencyProperty.Register(
                nameof(CornerRadius),
                typeof(CornerRadius),
                typeof(ForestListViewItemBase),
                new PropertyMetadata(default(CornerRadius)));

            PlacementProperty = DependencyProperty.Register(
                nameof(Placement),
                typeof(Dock),
                typeof(ForestListViewItemBase),
                new PropertyMetadata(Dock.Left));

            HasIconProperty = HasIconPropertyKey.DependencyProperty;
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ForestListViewItemBase), new FrameworkPropertyMetadata(typeof(ForestListViewItemBase)));
        }


        private static void OnPaletteChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ForestListViewItemBase)d).InvalidateState();
        }

        private static void OnIsFilledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = ((ForestListViewItemBase)d);
            ctrl.HasIcon = e.NewValue is Geometry;
            ctrl.InvalidateState();
        }

        private static void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = ((ForestListViewItemBase)d);
            ctrl.HasIcon = e.NewValue is Geometry;
            ctrl.InvalidateState();
        }

        public Dock Placement
        {
            get => (Dock)GetValue(PlacementProperty);
            set => SetValue(PlacementProperty, value);
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

    #region ForestTabItem

    public abstract partial class ForestTabItemBase
    {
        public static readonly DependencyProperty    IconProperty;
        public static readonly DependencyProperty    IsFilledProperty;
        public static readonly DependencyProperty    IconSizeProperty;
        public static readonly DependencyPropertyKey HasIconPropertyKey;
        public static readonly DependencyProperty    HasIconProperty;
        public static readonly DependencyProperty    PlacementProperty;
        public static readonly DependencyProperty    PaletteProperty;
        public static readonly DependencyProperty    CornerRadiusProperty;

        static ForestTabItemBase()
        {
            IconProperty = DependencyProperty.Register(
                nameof(Icon),
                typeof(Geometry),
                typeof(ForestTabItemBase),
                new PropertyMetadata(default(Geometry), OnIconChanged));

            IsFilledProperty = DependencyProperty.Register(
                nameof(IsFilled),
                typeof(bool),
                typeof(ForestTabItemBase),
                new PropertyMetadata(Boxing.False, OnIsFilledChanged));

            IconSizeProperty = DependencyProperty.Register(
                nameof(IconSize),
                typeof(double),
                typeof(ForestTabItemBase),
                new PropertyMetadata(17d));

            HasIconPropertyKey = DependencyProperty.RegisterReadOnly(
                nameof(HasIcon),
                typeof(bool),
                typeof(ForestTabItemBase),
                new PropertyMetadata(Boxing.False));

            PaletteProperty = DependencyProperty.Register(
                nameof(Palette),
                typeof(HighlightColorPalette),
                typeof(ForestTabItemBase),
                new PropertyMetadata(default(HighlightColorPalette), OnPaletteChanged));

            CornerRadiusProperty = DependencyProperty.Register(
                nameof(CornerRadius),
                typeof(CornerRadius),
                typeof(ForestTabItemBase),
                new PropertyMetadata(default(CornerRadius)));

            PlacementProperty = DependencyProperty.Register(
                nameof(Placement),
                typeof(Dock),
                typeof(ForestTabItemBase),
                new PropertyMetadata(Dock.Left));

            HasIconProperty = HasIconPropertyKey.DependencyProperty;
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ForestTabItemBase), new FrameworkPropertyMetadata(typeof(ForestTabItemBase)));
        }


        private static void OnPaletteChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ForestTabItemBase)d).InvalidateState();
        }

        private static void OnIsFilledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = ((ForestTabItemBase)d);
            ctrl.HasIcon = e.NewValue is Geometry;
            ctrl.InvalidateState();
        }

        private static void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = ((ForestTabItemBase)d);
            ctrl.HasIcon = e.NewValue is Geometry;
            ctrl.InvalidateState();
        }

        public Dock Placement
        {
            get => (Dock)GetValue(PlacementProperty);
            set => SetValue(PlacementProperty, value);
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

    #region ForestTreeViewItem

    public abstract partial class ForestTreeViewItemBase
    {
        public static readonly DependencyProperty    IconProperty;
        public static readonly DependencyProperty    IsFilledProperty;
        public static readonly DependencyProperty    IconSizeProperty;
        public static readonly DependencyPropertyKey HasIconPropertyKey;
        public static readonly DependencyProperty    HasIconProperty;
        public static readonly DependencyProperty    PlacementProperty;
        public static readonly DependencyProperty    PaletteProperty;
        public static readonly DependencyProperty    CornerRadiusProperty;

        static ForestTreeViewItemBase()
        {
            IconProperty = DependencyProperty.Register(
                nameof(Icon),
                typeof(Geometry),
                typeof(ForestTreeViewItemBase),
                new PropertyMetadata(default(Geometry), OnIconChanged));

            IsFilledProperty = DependencyProperty.Register(
                nameof(IsFilled),
                typeof(bool),
                typeof(ForestTreeViewItemBase),
                new PropertyMetadata(Boxing.False, OnIsFilledChanged));

            IconSizeProperty = DependencyProperty.Register(
                nameof(IconSize),
                typeof(double),
                typeof(ForestTreeViewItemBase),
                new PropertyMetadata(17d));

            HasIconPropertyKey = DependencyProperty.RegisterReadOnly(
                nameof(HasIcon),
                typeof(bool),
                typeof(ForestTreeViewItemBase),
                new PropertyMetadata(Boxing.False));

            PaletteProperty = DependencyProperty.Register(
                nameof(Palette),
                typeof(HighlightColorPalette),
                typeof(ForestTreeViewItemBase),
                new PropertyMetadata(default(HighlightColorPalette), OnPaletteChanged));

            CornerRadiusProperty = DependencyProperty.Register(
                nameof(CornerRadius),
                typeof(CornerRadius),
                typeof(ForestTreeViewItemBase),
                new PropertyMetadata(default(CornerRadius)));

            PlacementProperty = DependencyProperty.Register(
                nameof(Placement),
                typeof(Dock),
                typeof(ForestTreeViewItemBase),
                new PropertyMetadata(Dock.Left));

            HasIconProperty = HasIconPropertyKey.DependencyProperty;
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ForestTreeViewItemBase), new FrameworkPropertyMetadata(typeof(ForestTreeViewItemBase)));
        }


        private static void OnPaletteChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ForestTreeViewItemBase)d).InvalidateState();
        }

        private static void OnIsFilledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = ((ForestTreeViewItemBase)d);
            ctrl.HasIcon = e.NewValue is Geometry;
            ctrl.InvalidateState();
        }

        private static void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = ((ForestTreeViewItemBase)d);
            ctrl.HasIcon = e.NewValue is Geometry;
            ctrl.InvalidateState();
        }

        public Dock Placement
        {
            get => (Dock)GetValue(PlacementProperty);
            set => SetValue(PlacementProperty, value);
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

    #region ForestMenuItem

    public abstract partial class ForestMenuItemBase
    {
        public static readonly DependencyProperty    IsFilledProperty;
        public static readonly DependencyProperty    IconSizeProperty;
        public static readonly DependencyPropertyKey HasIconPropertyKey;
        public static readonly DependencyProperty    HasIconProperty;
        public static readonly DependencyProperty    PlacementProperty;
        public static readonly DependencyProperty    PaletteProperty;
        public static readonly DependencyProperty    CornerRadiusProperty;

        static ForestMenuItemBase()
        {
            IconProperty.AddOwner(typeof(ForestMenuItemBase))
                        .OverrideMetadata(
                            typeof(ForestMenuItemBase),
                            new FrameworkPropertyMetadata(default(Geometry), OnIconChanged));

            IsFilledProperty = DependencyProperty.Register(
                nameof(IsFilled),
                typeof(bool),
                typeof(ForestMenuItemBase),
                new PropertyMetadata(Boxing.False, OnIsFilledChanged));

            IconSizeProperty = DependencyProperty.Register(
                nameof(IconSize),
                typeof(double),
                typeof(ForestMenuItemBase),
                new PropertyMetadata(17d));

            HasIconPropertyKey = DependencyProperty.RegisterReadOnly(
                nameof(HasIcon),
                typeof(bool),
                typeof(ForestMenuItemBase),
                new PropertyMetadata(Boxing.False));

            PaletteProperty = DependencyProperty.Register(
                nameof(Palette),
                typeof(HighlightColorPalette),
                typeof(ForestMenuItemBase),
                new PropertyMetadata(default(HighlightColorPalette), OnPaletteChanged));

            CornerRadiusProperty = DependencyProperty.Register(
                nameof(CornerRadius),
                typeof(CornerRadius),
                typeof(ForestMenuItemBase),
                new PropertyMetadata(default(CornerRadius)));

            PlacementProperty = DependencyProperty.Register(
                nameof(Placement),
                typeof(Dock),
                typeof(ForestMenuItemBase),
                new PropertyMetadata(Dock.Left));

            HasIconProperty = HasIconPropertyKey.DependencyProperty;
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ForestMenuItemBase), new FrameworkPropertyMetadata(typeof(ForestMenuItemBase)));
        }

        private static void OnMenuItemRoleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var menuItem = (ForestMenuItemBase)d;
            menuItem.InvalidateState();
            menuItem.OnMenuItemRoleChanged(
                e.OldValue is MenuItemRole r ? r : default(MenuItemRole),
                e.NewValue is MenuItemRole r1 ? r1 : default(MenuItemRole));
        }


        private static void OnPaletteChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ForestMenuItemBase)d).InvalidateState();
        }

        private static void OnIsFilledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = ((ForestMenuItemBase)d);
            ctrl.HasIcon = e.NewValue is Geometry;
            ctrl.InvalidateState();
        }

        private static void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = ((ForestMenuItemBase)d);
            ctrl.HasIcon = e.NewValue is Geometry;
            ctrl.InvalidateState();
        }

        public Dock Placement
        {
            get => (Dock)GetValue(PlacementProperty);
            set => SetValue(PlacementProperty, value);
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

        public new Geometry Icon
        {
            get => (Geometry)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
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