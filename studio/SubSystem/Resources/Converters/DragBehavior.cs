using System.Windows;
using System.Windows.Input;

namespace Acorisoft.FutureGL.MigaStudio.Resources.Converters
{
public static class DragBehavior
    {
        #region IsDragEnabled

        /// <summary>
        /// Dragging enable state attached dependency property.
        /// </summary>
        public static readonly DependencyProperty IsDragEnabledProperty = DependencyProperty.RegisterAttached(
            "IsDragEnabled", typeof(bool), typeof(DragBehavior), new UIPropertyMetadata(false, OnIsDragEnabledPropertyChanged));

        /// <summary>
        /// Gets the dragging enabled attached property value.
        /// </summary>
        public static bool GetIsDragEnabled(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsDragEnabledProperty);
        }

        /// <summary>
        /// Sets the dragging enabled attached property value.
        /// </summary>
        public static void SetIsDragEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(IsDragEnabledProperty, value);
        }

        private static void OnIsDragEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            var element = d as FrameworkElement;
            FrameworkContentElement contentElement = null;
            if (element is null)
            {
                contentElement = d as FrameworkContentElement;
                if (contentElement is null)
                    return;
            }

            if (!(args.NewValue is bool newValue))
                return;

            if (newValue)
            {
                // Register the event handlers
                if (element != null)
                {
                    // Registering on the FrameworkElement
                    element.MouseLeftButtonDown += OnDragStarted;
                    element.MouseLeftButtonUp   += OnDragFinished;
                }
                else
                {
                    // Registering on the FrameworkContentElement
                    contentElement.MouseLeftButtonDown += OnDragStarted;
                    contentElement.MouseLeftButtonUp   += OnDragFinished;
                }
            }
            else
            {
                // Un-register the event handlers
                if (element != null)
                {
                    // Un-registering on the FrameworkElement
                    element.MouseLeftButtonDown -= OnDragStarted;
                    element.MouseLeftButtonUp   -= OnDragFinished;
                }
                else
                {
                    // Un-registering on the FrameworkContentElement
                    contentElement.MouseLeftButtonDown -= OnDragStarted;
                    contentElement.MouseLeftButtonUp   -= OnDragFinished;
                }
            }
        }

        #endregion

        #region IsDragging

        /// <summary>
        /// Dragging state attached dependency property.
        /// </summary>
        public static readonly DependencyProperty IsDraggingProperty = DependencyProperty.RegisterAttached(
            "IsDragging", typeof(bool), typeof(DragBehavior), new UIPropertyMetadata(false));

        /// <summary>
        /// Gets the dragging attached property value.
        /// </summary>
        public static bool GetIsDragging(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsDraggingProperty);
        }

        /// <summary>
        /// Sets the dragging attached property value.
        /// </summary>
        public static void SetIsDragging(DependencyObject obj, bool value)
        {
            obj.SetValue(IsDraggingProperty, value);
        }

        #endregion

        #region X

        /// <summary>
        /// X position attached dependency property.
        /// </summary>
        public static readonly DependencyProperty XProperty = DependencyProperty.RegisterAttached(
            "X", typeof(double), typeof(DragBehavior), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// Gets the X attached property value.
        /// </summary>
        public static double GetX(DependencyObject obj)
        {
            return (double)obj.GetValue(XProperty);
        }

        /// <summary>
        /// Sets the X attached property value.
        /// </summary>
        public static void SetX(DependencyObject obj, double value)
        {
            obj.SetValue(XProperty, value);
        }

        #endregion

        #region Y

        /// <summary>
        /// Y position attached dependency property.
        /// </summary>
        public static readonly DependencyProperty YProperty = DependencyProperty.RegisterAttached(
            "Y", typeof(double), typeof(DragBehavior), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// Gets the Y attached property value.
        /// </summary>
        public static double GetY(DependencyObject obj)
        {
            return (double)obj.GetValue(YProperty);
        }

        /// <summary>
        /// Sets the Y attached property value.
        /// </summary>
        public static void SetY(DependencyObject obj, double value)
        {
            obj.SetValue(YProperty, value);
        }

        #endregion
        
        #region PositionX

        private static readonly DependencyPropertyKey PositionXPropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "PositionX", typeof(double), typeof(DragBehavior), new UIPropertyMetadata(0.0));

        private static double GetPositionX(DependencyObject obj)
        {
            return (double)obj.GetValue(PositionXPropertyKey.DependencyProperty);
        }

        private static void SetPositionX(DependencyObject obj, double value)
        {
            obj.SetValue(PositionXPropertyKey, value);
        }

        #endregion

        #region PositionY

        private static readonly DependencyPropertyKey PositionYPropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "PositionY", typeof(double), typeof(DragBehavior), new UIPropertyMetadata(0.0));

        private static double GetPositionY(DependencyObject obj)
        {
            return (double)obj.GetValue(PositionYPropertyKey.DependencyProperty);
        }

        private static void SetPositionY(DependencyObject obj, double value)
        {
            obj.SetValue(PositionYPropertyKey, value);
        }

        #endregion

        #region OriginalX

        private static readonly DependencyPropertyKey OriginalXPropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "OriginalX", typeof(double), typeof(DragBehavior), new UIPropertyMetadata(0.0));

        private static double GetOriginalX(DependencyObject obj)
        {
            return (double)obj.GetValue(OriginalXPropertyKey.DependencyProperty);
        }

        private static void SetOriginalX(DependencyObject obj, double value)
        {
            obj.SetValue(OriginalXPropertyKey, value);
        }

        #endregion

        #region OriginalY

        private static readonly DependencyPropertyKey OriginalYPropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "OriginalY", typeof(double), typeof(DragBehavior), new UIPropertyMetadata(0.0));

        private static double GetOriginalY(DependencyObject obj)
        {
            return (double)obj.GetValue(OriginalYPropertyKey.DependencyProperty);
        }

        private static void SetOriginalY(DependencyObject obj, double value)
        {
            obj.SetValue(OriginalYPropertyKey, value);
        }

        #endregion

        #region Handlers

        private static void OnDragStarted(object sender, MouseButtonEventArgs args)
        {
            var obj = sender as DependencyObject;
            // We are starting the drag
            SetIsDragging(obj, true);

            var position = args.GetPosition(obj as IInputElement);

            // Save the position of the mouse to the start position
            SetOriginalX(obj, position.X);
            SetOriginalY(obj, position.Y);
            SetPositionX(obj, GetX(obj));
            SetPositionY(obj, GetY(obj));

            // Capture the mouse
            if (obj is FrameworkElement element)
            {
                element.CaptureMouse();
                element.MouseMove += OnDragging;
            }
            else
            {
                var contentElement = obj as FrameworkContentElement;
                if (contentElement is null)
                    throw new ArgumentException($"The control must be a descendent of the {nameof(FrameworkElement)} or {nameof(FrameworkContentElement)}.");
                contentElement.CaptureMouse();
                contentElement.MouseMove += OnDragging;
            }

            args.Handled = true;
        }

        private static void OnDragging(object sender, MouseEventArgs args)
        {
            var obj = sender as DependencyObject;
            if (!GetIsDragging(obj))
                return;

            var position = args.GetPosition(obj as IInputElement);
            var horizontalChange = position.X - GetOriginalX(obj);
            var verticalChange = position.Y - GetOriginalY(obj);


            if (double.IsNaN(GetX(obj)))
            {
                SetX(obj, 0);
            }

            if (double.IsNaN(GetY(obj)))
            {
                SetY(obj, 0);
            }

            // Move the object
            SetX(obj, GetX(obj) + horizontalChange);
            SetY(obj, GetY(obj) + verticalChange);

            args.Handled = true;
        }

        private static void OnDragFinished(object sender, MouseButtonEventArgs args)
        {
            var obj = (DependencyObject)sender;
            SetIsDragging(obj, false);



            var originalX = GetOriginalX(obj);
            var originalY = GetOriginalY(obj);
            var x = GetX(obj);
            var start_x = GetPositionX(obj);
            var y = GetY(obj);
            var start_y = GetPositionY(obj);

            if(Math.Abs(start_x - x ) > 500 || Math.Abs(start_y - y) > 500)
            {

                SetX(obj, originalX + Math.CopySign(Math.Clamp(x - originalX, -500, 500), x));
                SetY(obj, originalY + Math.CopySign(Math.Clamp(y - originalY, -500, 500), y));
            }


            obj.ClearValue(OriginalXPropertyKey);
            obj.ClearValue(OriginalYPropertyKey);

            // We finished the drag, release the mouse
            if (sender is FrameworkElement element)
            {
                element.MouseMove -= OnDragging;
                element.ReleaseMouseCapture();
            }
            else
            {
                var contentElement = sender as FrameworkContentElement;
                if (contentElement is null)
                    throw new ArgumentException($"The control must be a descendent of the {nameof(FrameworkElement)} or {nameof(FrameworkContentElement)}.");
                contentElement.MouseMove -= OnDragging;
                contentElement.ReleaseMouseCapture();
            }

            args.Handled = true;
        }

        #endregion
    }
}