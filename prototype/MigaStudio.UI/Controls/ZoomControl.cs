using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Animation;
// ReSharper disable PossibleNullReferenceException

namespace Acorisoft.FutureGL.MigaStudio.Controls
{
    /// <summary>
    /// Converter from <see cref="double"/> value to <see cref="Math.Log10"/> <see cref="double"/> value and vice versa.
    /// </summary>
    internal sealed class DoubleToLog10Converter : IValueConverter
    {
        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (double) value;
            return Math.Log10(val);
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (double) value;
            return Math.Pow(10, val);
        }
    }
    
    /// <summary>
    /// Converter that checks equality between value and parameter.
    /// </summary>
    internal sealed class EqualityToBooleanConverter : IValueConverter
    {
        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Equals(value, parameter);
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is true)
                return parameter;
            throw new NotSupportedException($"{nameof(EqualityToBooleanConverter)}: Can't bind back.");
        }
    }
    
    /// <summary>
    /// Handler for a content size changed event.
    /// </summary>
    /// <param name="sender">Event sender.</param>
    /// <param name="newSize">New content size.</param>
    internal delegate void ContentSizeChangedHandler([NotNull] object sender, Size newSize);

    /// <summary>
    /// Zoom modes.
    /// </summary>
    public enum ZoomControlModes
    {
        /// <summary>
        /// The content should fill the given space.
        /// </summary>
        Fill,

        /// <summary>
        /// The content will be represented in its original size.
        /// </summary>
        Original,

        /// <summary>
        /// The content will be zoomed with a custom percent.
        /// </summary>
        Custom
    }

    /// <summary>
    /// Zoom view modes.
    /// </summary>
    public enum ZoomViewModifierMode
    {
        /// <summary>
        /// It does nothing at all.
        /// </summary>
        None,

        /// <summary>
        /// You can pan the view with the mouse in this mode.
        /// </summary>
        Pan,

        /// <summary>
        /// You can zoom in with the mouse in this mode.
        /// </summary>
        ZoomIn,

        /// <summary>
        /// You can zoom out with the mouse in this mode.
        /// </summary>
        ZoomOut,

        /// <summary>
        /// Zooming after the user has been selected the zooming box.
        /// </summary>
        ZoomBox
    }


    /// <summary>
    /// Zoom content presenter control.
    /// </summary>
    internal sealed class ZoomContentPresenter : ContentPresenter
    {
        public event ContentSizeChangedHandler ContentSizeChanged;

        private Size _contentSize;

        public Size ContentSize
        {
            get => _contentSize;
            private set
            {
                if (_contentSize == value)
                    return;

                _contentSize = value;
                ContentSizeChanged?.Invoke(this, _contentSize);
            }
        }

        private const int InfiniteSize = 1000000000;

        /// <inheritdoc />
        protected override Size MeasureOverride(Size constraint)
        {
            base.MeasureOverride(new Size(double.PositiveInfinity, double.PositiveInfinity));

            return new Size(
                double.IsInfinity(constraint.Width) ? InfiniteSize : constraint.Width,
                double.IsInfinity(constraint.Height) ? InfiniteSize : constraint.Height);
        }

        /// <inheritdoc />
        protected override Size ArrangeOverride(Size arrangeSize)
        {
            var uiElement = VisualChildrenCount > 0
                ? VisualTreeHelper.GetChild(this, 0) as UIElement
                : null;
            if (uiElement is null)
                return arrangeSize;

            ContentSize = uiElement.DesiredSize;
            uiElement.Arrange(new Rect(uiElement.DesiredSize));

            return arrangeSize;
        }
    }

    /// <summary>
    /// Zoom control.
    /// </summary>
    [TemplatePart(Name = PartPresenter, Type = typeof(ZoomContentPresenter))]
    public sealed class ZoomControl : ContentControl
    {
        [NotNull]
        private const string PartPresenter = "PART_Presenter";

        private Point _mouseDownPos;
        private ZoomContentPresenter _presenter;
        private ScaleTransform _scaleTransform;
        private Vector _startTranslate;
        private TransformGroup _transformGroup;
        private TranslateTransform _translateTransform;
        private int _zoomAnimCount;
        private bool _isZooming;

        /// <summary/>
        static ZoomControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(ZoomControl),
                new FrameworkPropertyMetadata(typeof(ZoomControl)));
        }

        /// <summary/>
        public ZoomControl()
        {
            PreviewMouseWheel += OnZoomControlMouseWheel;
            PreviewMouseDown += OnZoomControlPreviewMouseDown;
            MouseDown += OnZoomControlMouseDown;
            MouseUp += OnZoomControlMouseUp;
        }

        public Brush ZoomBoxBackground
        {
            get => (Brush)GetValue(ZoomBoxBackgroundProperty);
            set => SetValue(ZoomBoxBackgroundProperty, value);
        }

        [NotNull]
        public static readonly DependencyProperty ZoomBoxBackgroundProperty = DependencyProperty.Register(
            nameof(ZoomBoxBackground), typeof(Brush), typeof(ZoomControl), new UIPropertyMetadata(null));

        public Brush ZoomBoxBorderBrush
        {
            get => (Brush)GetValue(ZoomBoxBorderBrushProperty);
            set => SetValue(ZoomBoxBorderBrushProperty, value);
        }

        [NotNull]
        public static readonly DependencyProperty ZoomBoxBorderBrushProperty = DependencyProperty.Register(
            nameof(ZoomBoxBorderBrush), typeof(Brush), typeof(ZoomControl), new UIPropertyMetadata(null));

        public Thickness ZoomBoxBorderThickness
        {
            get => (Thickness)GetValue(ZoomBoxBorderThicknessProperty);
            set => SetValue(ZoomBoxBorderThicknessProperty, value);
        }

        [NotNull]
        public static readonly DependencyProperty ZoomBoxBorderThicknessProperty = DependencyProperty.Register(
            nameof(ZoomBoxBorderThickness), typeof(Thickness), typeof(ZoomControl), new UIPropertyMetadata(null));

        public double ZoomBoxOpacity
        {
            get => (double)GetValue(ZoomBoxOpacityProperty);
            set => SetValue(ZoomBoxOpacityProperty, value);
        }

        [NotNull]
        public static readonly DependencyProperty ZoomBoxOpacityProperty = DependencyProperty.Register(
            nameof(ZoomBoxOpacity), typeof(double), typeof(ZoomControl), new UIPropertyMetadata(0.5));

        public Rect ZoomBox
        {
            get => (Rect)GetValue(ZoomBoxProperty);
            set => SetValue(ZoomBoxProperty, value);
        }

        [NotNull]
        public static readonly DependencyProperty ZoomBoxProperty = DependencyProperty.Register(
            nameof(ZoomBox), typeof(Rect), typeof(ZoomControl), new UIPropertyMetadata(new Rect()));

        public Point OrigoPosition => new Point(ActualWidth / 2.0, ActualHeight / 2.0);

        public double TranslateX
        {
            get => (double)GetValue(TranslateXProperty);
            set
            {
                BeginAnimation(TranslateXProperty, null);
                SetValue(TranslateXProperty, value);
            }
        }

        [NotNull]
        public static readonly DependencyProperty TranslateXProperty = DependencyProperty.Register(
            nameof(TranslateX), typeof(double), typeof(ZoomControl), new UIPropertyMetadata(0.0, OnTranslateXPropertyChanged, OnTranslateXCoerce));

        public double TranslateY
        {
            get => (double)GetValue(TranslateYProperty);
            set
            {
                BeginAnimation(TranslateYProperty, null);
                SetValue(TranslateYProperty, value);
            }
        }

        [NotNull]
        public static readonly DependencyProperty TranslateYProperty = DependencyProperty.Register(
            nameof(TranslateY), typeof(double), typeof(ZoomControl), new UIPropertyMetadata(0.0, OnTranslateYPropertyChanged, OnTranslateYCoerce));

        public TimeSpan AnimationLength
        {
            get => (TimeSpan)GetValue(AnimationLengthProperty);
            set => SetValue(AnimationLengthProperty, value);
        }

        [NotNull]
        public static readonly DependencyProperty AnimationLengthProperty = DependencyProperty.Register(
            nameof(AnimationLength), typeof(TimeSpan), typeof(ZoomControl), new UIPropertyMetadata(TimeSpan.FromMilliseconds(500.0)));

        public double MinZoom
        {
            get => (double)GetValue(MinZoomProperty);
            set => SetValue(MinZoomProperty, value);
        }

        [NotNull]
        public static readonly DependencyProperty MinZoomProperty = DependencyProperty.Register(
            nameof(MinZoom), typeof(double), typeof(ZoomControl), new UIPropertyMetadata(0.01));

        public double MaxZoom
        {
            get => (double)GetValue(MaxZoomProperty);
            set => SetValue(MaxZoomProperty, value);
        }

        [NotNull]
        public static readonly DependencyProperty MaxZoomProperty = DependencyProperty.Register(
            nameof(MaxZoom), typeof(double), typeof(ZoomControl), new UIPropertyMetadata(100.0));

        public double MaxZoomDelta
        {
            get => (double)GetValue(MaxZoomDeltaProperty);
            set => SetValue(MaxZoomDeltaProperty, value);
        }

        [NotNull]
        public static readonly DependencyProperty MaxZoomDeltaProperty = DependencyProperty.Register(
            nameof(MaxZoomDelta), typeof(double), typeof(ZoomControl), new UIPropertyMetadata(5.0));

        public double ZoomDeltaMultiplier
        {
            get => (double)GetValue(ZoomDeltaMultiplierProperty);
            set => SetValue(ZoomDeltaMultiplierProperty, value);
        }

        [NotNull]
        public static readonly DependencyProperty ZoomDeltaMultiplierProperty = DependencyProperty.Register(
            nameof(ZoomDeltaMultiplier), typeof(double), typeof(ZoomControl), new UIPropertyMetadata(100.0));

        public double Zoom
        {
            get => (double)GetValue(ZoomProperty);
            set
            {
                if (Math.Abs((double)GetValue(ZoomProperty) - value) < double.Epsilon)
                    return;

                BeginAnimation(ZoomProperty, null);
                SetValue(ZoomProperty, value);
            }
        }

        [NotNull]
        public static readonly DependencyProperty ZoomProperty = DependencyProperty.Register(
            nameof(Zoom), typeof(double), typeof(ZoomControl), new UIPropertyMetadata(1.0, OnZoomPropertyChanged));

        private ZoomContentPresenter Presenter
        {
            get => _presenter;
            set
            {
                _presenter = value;
                if (_presenter is null)
                    return;

                _transformGroup = new TransformGroup();
                _scaleTransform = new ScaleTransform();
                _translateTransform = new TranslateTransform();
                _transformGroup.Children.Add(_scaleTransform);
                _transformGroup.Children.Add(_translateTransform);
                _presenter.RenderTransform = _transformGroup;
                _presenter.RenderTransformOrigin = new Point(0.5, 0.5);
            }
        }

        public ZoomViewModifierMode ModifierMode
        {
            get => (ZoomViewModifierMode)GetValue(ModifierModeProperty);
            set => SetValue(ModifierModeProperty, value);
        }

        [NotNull]
        public static readonly DependencyProperty ModifierModeProperty = DependencyProperty.Register(
            nameof(ModifierMode), typeof(ZoomViewModifierMode), typeof(ZoomControl), new UIPropertyMetadata(ZoomViewModifierMode.None));

        public ZoomControlModes Mode
        {
            get => (ZoomControlModes)GetValue(ModeProperty);
            set => SetValue(ModeProperty, value);
        }

        [NotNull]
        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(
            nameof(Mode), typeof(ZoomControlModes), typeof(ZoomControl), new UIPropertyMetadata(ZoomControlModes.Custom, OnModePropertyChanged));

        private static void OnModePropertyChanged(
            [NotNull] DependencyObject d,
            DependencyPropertyChangedEventArgs args)
        {
            var zoomControl = (ZoomControl)d;
            switch ((ZoomControlModes)args.NewValue)
            {
                case ZoomControlModes.Fill:
                    zoomControl.DoZoomToFill();
                    break;

                case ZoomControlModes.Original:
                    zoomControl.DoZoomToOriginal();
                    break;

                case ZoomControlModes.Custom:
                    break;

                default:
                    throw new NotSupportedException();
            }
        }

        [Pure]
        private static object OnTranslateXCoerce(DependencyObject d, object basevalue)
        {
            var zoomControl = (ZoomControl)d;
            return zoomControl.GetCoercedTranslateX((double)basevalue);
        }

        [Pure]
        private double GetCoercedTranslateX(double baseValue)
        {
            return _presenter is null ? 0.0 : baseValue;
        }

        [Pure]
        private static object OnTranslateYCoerce([NotNull] DependencyObject d, object basevalue)
        {
            var zoomControl = (ZoomControl)d;
            return zoomControl.GetCoercedTranslateY((double)basevalue);
        }

        [Pure]
        private double GetCoercedTranslateY(double baseValue)
        {
            return _presenter is null ? 0.0 : baseValue;
        }

        private void OnZoomControlMouseUp([NotNull] object sender, [NotNull] MouseButtonEventArgs args)
        {
            switch (ModifierMode)
            {
                case ZoomViewModifierMode.None:
                    break;

                case ZoomViewModifierMode.Pan:
                case ZoomViewModifierMode.ZoomIn:
                case ZoomViewModifierMode.ZoomOut:
                    ModifierMode = ZoomViewModifierMode.None;
                    PreviewMouseMove -= OnZoomControlPreviewMouseMove;
                    ReleaseMouseCapture();
                    break;

                case ZoomViewModifierMode.ZoomBox:
                    ZoomTo(ZoomBox);
                    goto case ZoomViewModifierMode.Pan;

                default:
                    throw new NotSupportedException();
            }
        }

        public void ZoomTo(Rect rect)
        {
            DoZoom(
                Math.Min(ActualWidth / rect.Width, ActualHeight / rect.Height),
                OrigoPosition,
                new Point(rect.X + rect.Width / 2.0, rect.Y + rect.Height / 2.0),
                OrigoPosition);
            ZoomBox = new Rect();
        }

        private void OnZoomControlPreviewMouseMove([NotNull] object sender, [NotNull] MouseEventArgs args)
        {
            switch (ModifierMode)
            {
                case ZoomViewModifierMode.None:
                case ZoomViewModifierMode.ZoomIn:
                case ZoomViewModifierMode.ZoomOut:
                    break;

                case ZoomViewModifierMode.Pan:
                    var vector = _startTranslate + (args.GetPosition(this) - _mouseDownPos);
                    TranslateX = vector.X;
                    TranslateY = vector.Y;
                    break;

                case ZoomViewModifierMode.ZoomBox:
                    var position = args.GetPosition(this);
                    ZoomBox = new Rect(
                        Math.Min(_mouseDownPos.X, position.X),
                        Math.Min(_mouseDownPos.Y, position.Y),
                        Math.Abs(_mouseDownPos.X - position.X),
                        Math.Abs(_mouseDownPos.Y - position.Y));
                    break;

                default:
                    throw new NotSupportedException();
            }
        }

        private void OnZoomControlMouseDown([NotNull] object sender, [NotNull] MouseButtonEventArgs args) => OnMouseDown(args, false);

        private void OnZoomControlPreviewMouseDown([NotNull] object sender, [NotNull] MouseButtonEventArgs args) => OnMouseDown(args, true);

        private void OnMouseDown([NotNull] MouseButtonEventArgs args, bool isPreview)
        {
            if (ModifierMode != ZoomViewModifierMode.None)
                return;

            switch (Keyboard.Modifiers)
            {
                case ModifierKeys.None:
                    if (!isPreview)
                    {
                        ModifierMode = ZoomViewModifierMode.Pan;
                        goto case ModifierKeys.Control;
                    }
                    else
                        goto case ModifierKeys.Control;

                case ModifierKeys.Alt:
                    ModifierMode = ZoomViewModifierMode.ZoomBox;
                    goto case ModifierKeys.Control;

                case ModifierKeys.Control:
                case ModifierKeys.Windows:
                    if (ModifierMode == ZoomViewModifierMode.None)
                        break;

                    _mouseDownPos = args.GetPosition(this);
                    _startTranslate = new Vector(TranslateX, TranslateY);
                    Mouse.Capture(this);
                    PreviewMouseMove += OnZoomControlPreviewMouseMove;
                    break;

                case ModifierKeys.Shift:
                    ModifierMode = ZoomViewModifierMode.Pan;
                    goto case ModifierKeys.Control;
            }
        }

        private static void OnTranslateXPropertyChanged(
            [NotNull] DependencyObject d,
            DependencyPropertyChangedEventArgs args)
        {
            var zoomControl = (ZoomControl)d;
            if (zoomControl._translateTransform is null)
                return;

            zoomControl._translateTransform.X = (double)args.NewValue;
            if (zoomControl._isZooming)
                return;

            zoomControl.Mode = ZoomControlModes.Custom;
        }

        private static void OnTranslateYPropertyChanged(
            [NotNull] DependencyObject d,
            DependencyPropertyChangedEventArgs args)
        {
            var zoomControl = (ZoomControl)d;
            if (zoomControl._translateTransform is null)
                return;

            zoomControl._translateTransform.Y = (double)args.NewValue;
            if (zoomControl._isZooming)
                return;

            zoomControl.Mode = ZoomControlModes.Custom;
        }

        private static void OnZoomPropertyChanged(
            [NotNull] DependencyObject d,
            DependencyPropertyChangedEventArgs args)
        {
            var zoomControl = (ZoomControl)d;
            if (zoomControl._scaleTransform is null)
                return;

            var newValue = (double)args.NewValue;
            zoomControl._scaleTransform.ScaleX = newValue;
            zoomControl._scaleTransform.ScaleY = newValue;
            if (zoomControl._isZooming)
                return;

            var num = (double)args.NewValue / (double)args.OldValue;
            zoomControl.TranslateX *= num;
            zoomControl.TranslateY *= num;
            zoomControl.Mode = ZoomControlModes.Custom;
        }

        private void OnZoomControlMouseWheel([NotNull] object sender, [NotNull] MouseWheelEventArgs args)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Control) <= ModifierKeys.None || ModifierMode != ZoomViewModifierMode.None)
                return;

            args.Handled = true;
            var origoPosition = new Point(ActualWidth / 2.0, ActualHeight / 2.0);
            var position = args.GetPosition(this);
            DoZoom(
                Math.Max(1.0 / MaxZoomDelta, Math.Min(MaxZoomDelta, args.Delta / 10000.0 * ZoomDeltaMultiplier + 1.0)),
                origoPosition,
                position,
                position);
        }

        private void DoZoom(
            double deltaZoom,
            Point origoPosition,
            Point startHandlePosition,
            Point targetHandlePosition)
        {
            var zoom = Zoom;
            var num = Math.Max(MinZoom, Math.Min(MaxZoom, zoom * deltaZoom));
            var vector1 = new Vector(TranslateX, TranslateY);
            var vector2 = startHandlePosition - origoPosition;
            var vector3 = targetHandlePosition - origoPosition - ((vector2 - vector1) / zoom * num + vector1);
            var coercedTranslateX = GetCoercedTranslateX(TranslateX + vector3.X);
            var coercedTranslateY = GetCoercedTranslateY(TranslateY + vector3.Y);
            DoZoomAnimation(num, coercedTranslateX, coercedTranslateY);
            Mode = ZoomControlModes.Custom;
        }

        private void DoZoomAnimation(double targetZoom, double transformX, double transformY)
        {
            _isZooming = true;

            var duration = new Duration(AnimationLength);
            StartAnimation(TranslateXProperty, transformX, duration);
            StartAnimation(TranslateYProperty, transformY, duration);
            StartAnimation(ZoomProperty, targetZoom, duration);
        }

        private void StartAnimation([NotNull] DependencyProperty dp, double toValue, Duration duration)
        {
            if (double.IsNaN(toValue) || double.IsInfinity(toValue))
            {
                if (dp != ZoomProperty)
                    return;

                _isZooming = false;
            }
            else
            {
                var doubleAnimation = new DoubleAnimation(toValue, duration);
                if (dp == ZoomProperty)
                {
                    ++_zoomAnimCount;
                    doubleAnimation.Completed += (_, _) =>
                    {
                        --_zoomAnimCount;
                        if (_zoomAnimCount > 0)
                            return;

                        var zoom = Zoom;
                        BeginAnimation(ZoomProperty, null);
                        SetValue(ZoomProperty, zoom);
                        _isZooming = false;
                    };
                }

                BeginAnimation(dp, doubleAnimation, HandoffBehavior.Compose);
            }
        }

        public void ZoomToOriginal() => Mode = ZoomControlModes.Original;

        private void DoZoomToOriginal()
        {
            if (_presenter is null)
                return;

            var initialTranslate = GetInitialTranslate();
            DoZoomAnimation(1.0, initialTranslate.X, initialTranslate.Y);
        }

        [Pure]
        private Vector GetInitialTranslate()
        {
            return _presenter is null
                ? default
                : new Vector(
                    -(_presenter.ContentSize.Width - _presenter.DesiredSize.Width) / 2.0,
                    -(_presenter.ContentSize.Height - _presenter.DesiredSize.Height) / 2.0);
        }

        public void ZoomToFill() => Mode = ZoomControlModes.Fill;

        private void DoZoomToFill()
        {
            if (_presenter is null)
                return;

            var targetZoom = Math.Min(
                ActualWidth / _presenter.ContentSize.Width,
                ActualHeight / _presenter.ContentSize.Height);

            var initialTranslate = GetInitialTranslate();
            DoZoomAnimation(targetZoom, initialTranslate.X * targetZoom, initialTranslate.Y * targetZoom);
        }

        /// <inheritdoc />
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Presenter = GetTemplateChild(PartPresenter) as ZoomContentPresenter;
            if (Presenter != null)
            {
                Presenter.SizeChanged += (_, _) =>
                {
                    if (Mode != ZoomControlModes.Fill)
                        return;
                    DoZoomToFill();
                };

                Presenter.ContentSizeChanged += (_, _) =>
                {
                    if (Mode != ZoomControlModes.Fill)
                        return;
                    DoZoomToFill();
                };
            }

            ZoomToFill();
        }
    }
}
