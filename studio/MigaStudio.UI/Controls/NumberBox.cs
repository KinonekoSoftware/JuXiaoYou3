using System.Globalization;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Animation;
using Acorisoft.FutureGL.Forest.Controls;
using Acorisoft.FutureGL.Forest.UI.Tools;
using PasswordBox = System.Windows.Controls.PasswordBox;
using VisualState = Acorisoft.FutureGL.Forest.Styles.VisualState;

// ReSharper disable CompareOfFloatsByEqualityOperator

namespace Acorisoft.FutureGL.MigaStudio.Controls
{
    public class NumberBox : ForestTextBoxBase
    {
        private const string PART_WatermarkName  = "PART_Watermark";
        private const string PART_ButtonUpName   = "PART_Up";
        private const string PART_ButtonDownName = "PART_Down";

        private TextBlock  _watermark;
        private Storyboard _storyboard;
        private ToolButton _up;
        private ToolButton _down;

        private SolidColorBrush _background;
        private SolidColorBrush _foreground;
        private SolidColorBrush _backgroundHighlight1;
        private SolidColorBrush _backgroundHighlight2;
        private SolidColorBrush _foregroundHighlight;
        private SolidColorBrush _backgroundDisabled;
        private SolidColorBrush _foregroundDisabled;

        public NumberBox()
        {
            _formatter                       = new ValidateNumberFormatter();
            StateMachine.StateChangedHandler = HandleStateChanged;
            DataObject.AddPastingHandler(this, OnClipboardPaste);
        }
        
        
        private void HandleStateChanged(bool init, VisualState last, VisualState now, VisualStateTrigger value)
        {
            var theme = ThemeSystem.Instance.Theme;
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
                    break;
                case VisualState.Inactive:
                    GoToDisableState(theme);
                    break;
            }
        }

        protected override void GetTemplateChildOverride(ITemplatePartFinder finder)
        {
            finder.Find<Border>(PART_BdName, x => PART_Bd                 = x)
                  .Find<TextBlock>(PART_WatermarkName, x => _watermark    = x)
                  .Find<ToolButton>(PART_ButtonUpName, x => _up           = x)
                  .Find<ToolButton>(PART_ButtonDownName, x => _down       = x)
                  .Find<ScrollViewer>(PART_ContentName, x => PART_Content = x);
        }
        
        
        protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            if (StateMachine.CurrentState == VisualState.Normal)
            {
                StateMachine.NextState(VisualState.Highlight2);
            }
        }

        protected override void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            
            if (StateMachine.CurrentState == VisualState.Highlight2)
            {
                if (_up.IsFocused || _down.IsFocused)
                {
                    return;
                }

                StateMachine.NextState(IsMouseOver ? VisualStateTrigger.Back : VisualStateTrigger.Next);
            }

        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            if (!IsKeyboardFocused && 
                !_up.IsFocused &&
                !_down.IsFocused)
            {
                StateMachine.NextState(VisualStateTrigger.Next);
            }

        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            ReleaseMouseCapture();
            if (StateMachine.CurrentState == VisualState.Highlight1)
            {
                StateMachine.ResetState();
            }

        }

        protected override void OnApplyTemplateOverride()
        {
            _up.Click   += OnUp;
            _down.Click += OnDown;
        }

        protected override void StopAnimation()
        {
            _storyboard?.Stop(PART_Bd);
        }

        protected override void SetForeground(Brush brush)
        {
            _watermark.Foreground = brush;
            PART_Content.SetValue(TextElement.ForegroundProperty, brush);
        }

        protected override void OnInvalidateState()
        {
            _background           = null;
            _foreground           = null;
            _backgroundHighlight1 = null;
            _foregroundHighlight  = null;
            _backgroundHighlight2 = null;
            _backgroundDisabled   = null;
            _foregroundDisabled   = null;
            InvalidateVisual();
        }

        protected override void GoToNormalState(ForestThemeSystem theme)
        {
            _background  ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.AdaptiveLevel2]);
            _foreground  ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.ForegroundInAdaptive]);

            PART_Bd.Background    = _background;
            SetForeground(_foreground);
        }

        protected override void GoToHighlight1State(Duration duration, ForestThemeSystem theme)
        {
            _backgroundHighlight1 ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.AdaptiveLevel2]);
            _foregroundHighlight  ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.ForegroundInAdaptive]);

            // 白底变特殊色
            // 高亮色变白色
            var backgroundAnimation = new ColorAnimation
            {
                Duration = duration,
                From     = _background.Color,
                To       = _backgroundHighlight1.Color,
            };

            // var borderAnimation = new ColorAnimation
            // {
            //     Duration = duration,
            //     From     = _borderBrush.Color,
            //     To       = _highlight.Color,
            // };

            Storyboard.SetTarget(backgroundAnimation, PART_Bd);
            Storyboard.SetTargetProperty(backgroundAnimation, new PropertyPath("(Border.Background).(SolidColorBrush.Color)"));
            // Storyboard.SetTarget(borderAnimation, PART_Bd);
            // Storyboard.SetTargetProperty(borderAnimation, new PropertyPath("(Border.BorderBrush).(SolidColorBrush.Color)"));

            _storyboard = new Storyboard
            {
                Children = new TimelineCollection { backgroundAnimation, /*borderAnimation*/ }
            };

            //
            // 开始动画
            PART_Bd.BeginStoryboard(_storyboard, HandoffBehavior.SnapshotAndReplace, true);

            // 设置文本颜色
            SetForeground(_foregroundHighlight);
        }

        protected override void GoToHighlight2State(Duration duration, ForestThemeSystem theme)
        {
            _backgroundHighlight1 ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.AdaptiveLevel2]);
            _backgroundHighlight2 ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.AdaptiveLevel3]);
            _foregroundHighlight  ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.ForegroundInAdaptive]);

            // 白底变特殊色
            // 高亮色变白色
            var backgroundAnimation = new ColorAnimation
            {
                Duration = duration,
                From     = _backgroundHighlight1.Color,
                To       = _backgroundHighlight2.Color,
            };

            Storyboard.SetTarget(backgroundAnimation, PART_Bd);
            // Storyboard.SetTarget(borderAnimation, PART_Bd);
            // Storyboard.SetTargetProperty(borderAnimation, new PropertyPath("(Border.BorderBrush).(SolidColorBrush.Color)"));
            Storyboard.SetTargetProperty(backgroundAnimation, new PropertyPath("(Border.Background).(SolidColorBrush.Color)"));

            _storyboard = new Storyboard
            {
                Children = new TimelineCollection { backgroundAnimation, /* borderAnimation */ }
            };

            //
            // 开始动画
            PART_Bd.BeginStoryboard(_storyboard, HandoffBehavior.SnapshotAndReplace, true);

            // 设置文本颜色
            PART_Bd.BorderThickness = BorderThickness;
            SelectionBrush          = new SolidColorBrush(theme.Colors[(int)ForestTheme.HighlightA2]);
            SetForeground(_foregroundHighlight);
        }

        protected override void GoToDisableState(ForestThemeSystem theme)
        {
            _backgroundDisabled ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.BackgroundLevel3]);
            _foregroundDisabled ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.ForegroundLevel3]);

            PART_Bd.Background      = _background;
            SetForeground(_foregroundDisabled);
        }

        #region NumberBox

        private readonly ValidateNumberFormatter _formatter;

        public class ValidateNumberFormatter
        {
            public string FormatDouble(double? value)
            {
                return value?.ToString(GetFormatSpecifier(), GetCurrentCultureConverter()) ?? string.Empty;
            }


            public string FormatInt(int? value)
            {
                return value?.ToString(GetFormatSpecifier(), GetCurrentCultureConverter()) ?? string.Empty;
            }


            public string FormatUInt(uint? value)
            {
                return value?.ToString(GetFormatSpecifier(), GetCurrentCultureConverter()) ?? string.Empty;
            }


            public double ParseDouble(string value)
            {
                return double.TryParse(value, out var d) ?d : 0d;
            }


            public int ParseInt(string value)
            { return int.TryParse(value, out var i) ? i : 0;
            }


            public uint ParseUInt(string value)
            {
                return uint.TryParse(value, out var ui) ? ui : 0;
            }

            private static string GetFormatSpecifier()
            {
                return "G";
            }

            private static CultureInfo GetCurrentCultureConverter()
            {
                return CultureInfo.CurrentCulture;
            }
        }

        private bool _valueUpdating;

        /// <summary>
        /// Property for <see cref="Value"/>.
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value),
            typeof(double?), typeof(NumberBox), new PropertyMetadata(null, OnValuePropertyChanged));

        /// <summary>
        /// Property for <see cref="MaxDecimalPlaces"/>.
        /// </summary>
        public static readonly DependencyProperty MaxDecimalPlacesProperty = DependencyProperty.Register(nameof(MaxDecimalPlaces),
            typeof(int), typeof(NumberBox), new PropertyMetadata(6));

        /// <summary>
        /// Property for <see cref="SmallChange"/>.
        /// </summary>
        public static readonly DependencyProperty SmallChangeProperty = DependencyProperty.Register(nameof(SmallChange),
            typeof(double), typeof(NumberBox), new PropertyMetadata(1.0d));

        /// <summary>
        /// Property for <see cref="LargeChange"/>.
        /// </summary>
        public static readonly DependencyProperty LargeChangeProperty = DependencyProperty.Register(nameof(LargeChange),
            typeof(double), typeof(NumberBox), new PropertyMetadata(10.0d));

        /// <summary>
        /// Property for <see cref="Maximum"/>.
        /// </summary>
        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(nameof(Maximum),
            typeof(double), typeof(NumberBox), new PropertyMetadata(double.MaxValue));

        /// <summary>
        /// Property for <see cref="Minimum"/>.
        /// </summary>
        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(nameof(Minimum),
            typeof(double), typeof(NumberBox), new PropertyMetadata(double.MinValue));

        /// <summary>
        /// Property for <see cref="AcceptsExpression"/>.
        /// </summary>
        public static readonly DependencyProperty AcceptsExpressionProperty = DependencyProperty.Register(nameof(AcceptsExpression),
            typeof(bool), typeof(NumberBox), new PropertyMetadata(true));

        /// <summary>
        /// Routed event for <see cref="ValueChanged"/>.
        /// </summary>
        public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent(
            nameof(ValueChanged), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(NumberBox));

        /// <summary>
        /// Gets or sets the numeric value of a <see cref="NumberBox"/>.
        /// </summary>
        public double? Value
        {
            get => (double?)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        /// <summary>
        /// Gets or sets the number of decimal places to be rounded when converting from Text to Value.
        /// </summary>
        public int MaxDecimalPlaces
        {
            get => (int)GetValue(MaxDecimalPlacesProperty);
            set => SetValue(MaxDecimalPlacesProperty, value);
        }

        /// <summary>
        /// Gets or sets the value that is added to or subtracted from <see cref="Value"/> when a small change is made, such as with an arrow key or scrolling.
        /// </summary>
        public double SmallChange
        {
            get => (double)GetValue(SmallChangeProperty);
            set => SetValue(SmallChangeProperty, value);
        }

        /// <summary>
        /// Gets or sets the value that is added to or subtracted from <see cref="Value"/> when a large change is made, such as with the PageUP and PageDown keys.
        /// </summary>
        public double LargeChange
        {
            get => (double)GetValue(LargeChangeProperty);
            set => SetValue(LargeChangeProperty, value);
        }

        /// <summary>
        /// Gets or sets the numerical maximum for <see cref="Value"/>.
        /// </summary>
        public double Maximum
        {
            get => (double)GetValue(MaximumProperty);
            set => SetValue(MaximumProperty, value);
        }

        /// <summary>
        /// Gets or sets the numerical minimum for <see cref="Value"/>.
        /// </summary>
        public double Minimum
        {
            get => (double)GetValue(MinimumProperty);
            set => SetValue(MinimumProperty, value);
        }

        /// <summary>
        /// Toggles whether the control will accept and evaluate a basic formulaic expression entered as input.
        /// </summary>
        public bool AcceptsExpression
        {
            get => (bool)GetValue(AcceptsExpressionProperty);
            set => SetValue(AcceptsExpressionProperty, value);
        }

        /// <summary>
        /// Occurs after the user triggers evaluation of new input by pressing the Enter key, clicking a spin button, or by changing focus.
        /// </summary>
        public event RoutedEventHandler ValueChanged
        {
            add => AddHandler(ValueChangedEvent, value);
            remove => RemoveHandler(ValueChangedEvent, value);
        }

        static NumberBox()
        {
            AcceptsReturnProperty.OverrideMetadata(typeof(PasswordBox), new FrameworkPropertyMetadata(false));
            MaxLinesProperty.OverrideMetadata(typeof(PasswordBox), new FrameworkPropertyMetadata(1));
            MinLinesProperty.OverrideMetadata(typeof(PasswordBox), new FrameworkPropertyMetadata(1));
        }


        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            switch (e.Key)
            {
                case Key.PageUp:
                    StepValue(LargeChange);
                    break;
                case Key.PageDown:
                    StepValue(-LargeChange);
                    break;
                case Key.Up:
                    StepValue(SmallChange);
                    break;
                case Key.Down:
                    StepValue(-SmallChange);
                    break;
                case Key.Enter:
                    if (TextWrapping != TextWrapping.Wrap)
                    {
                        ValidateInput();
                        MoveCaretToTextEnd();
                    }

                    break;
            }
        }


        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);

            ValidateInput();
        }


        private void OnUp(object sender, RoutedEventArgs e)
        {
            StepValue(SmallChange);
        }
        
        private void OnDown(object sender, RoutedEventArgs e)
        {
            StepValue(-SmallChange);
        }

        //protected override void OnTextChanged(System.Windows.Controls.TextChangedEventArgs e)
        //{
        //    base.OnTextChanged(e);

        //    //if (new string[] { ",", ".", " " }.Any(s => Text.EndsWith(s)))
        //    //    return;

        //    //if (!_textUpdating)
        //    //    UpdateValueToText();
        //}


        protected override void OnTemplateChanged(ControlTemplate oldTemplate, ControlTemplate newTemplate)
        {
            base.OnTemplateChanged(oldTemplate, newTemplate);

            // If Text has been set, but Value hasn't, update Value based on Text.
            if (string.IsNullOrEmpty(Text) && Value != null)
                UpdateValueToText();
            else
                UpdateTextToValue();
        }

        /// <summary>
        /// Is called when <see cref="Value"/> in this <see cref="NumberBox"/> changes.
        /// </summary>
        protected virtual void OnValueChanged(DependencyObject d, double? oldValue)
        {
            if (_valueUpdating)
                return;

            _valueUpdating = true;

            var newValue = Value;

            if (newValue > Maximum)
                Value = Maximum;

            if (newValue < Minimum)
                Value = Minimum;

            if (newValue != oldValue)
                RaiseEvent(new RoutedEventArgs(ValueChangedEvent));

            UpdateTextToValue();

            _valueUpdating = false;
        }

        /// <summary>
        /// Is called when something is pasted in this <see cref="NumberBox"/>.
        /// </summary>
        protected void OnClipboardPaste(object sender, DataObjectPastingEventArgs e)
        {
            // TODO: Fix clipboard
            if (sender is not NumberBox)
                return;

            ValidateInput();
        }

        private void StepValue(double? change)
        {

            // Before adjusting the value, validate the contents of the textbox so we don't override it.
            ValidateInput();

            var newValue = Value ?? 0;

            if (change != null)
                newValue += (double)change;

            Value = newValue;

            MoveCaretToTextEnd();
        }

        private void UpdateTextToValue()
        {

            var newText = string.Empty;
            // text = value

            if (Value != null)
                newText = _formatter.FormatDouble(
                    Math.Round((double)Value, MaxDecimalPlaces));

            Text = newText;

        }

        private void UpdateValueToText()
        {
            ValidateInput();
        }

        private void ValidateInput()
        {
            var text = Text.Trim();

            if (string.IsNullOrEmpty(text))
            {
                Value = null;

                return;
            }

            var value        = _formatter.ParseDouble(text);

            if (Value == value)
            {
                UpdateTextToValue();

                return;
            }

            if (value > Maximum)
                value = Maximum;

            if (value < Minimum)
                value = Minimum;

            Value = value;
        }

        private void MoveCaretToTextEnd()
        {
            CaretIndex = Text.Length;
        }


        private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not NumberBox numberBox)
                return;

            numberBox.OnValueChanged(d, (double?)e.OldValue);
        }

        #endregion
    }
}