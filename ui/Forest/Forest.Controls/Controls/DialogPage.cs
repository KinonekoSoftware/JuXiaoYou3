using System.Windows.Input;

namespace Acorisoft.FutureGL.Forest.Controls
{
    public enum DialogPurpose
    {
        Notification,
        
        Query,
        
        /// <summary>
        /// 表单
        /// </summary>
        Form,
    }
    
    /// <summary>
    /// 
    /// </summary>
    public class DialogPage : ContentControl
    {
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            nameof(Title),
            typeof(string),
            typeof(DialogPage),
            new PropertyMetadata(default(string)));

        public static readonly DependencyProperty AccentProperty = DependencyProperty.Register(
            nameof(Accent),
            typeof(Brush),
            typeof(DialogPage),
            new PropertyMetadata(default(Brush)));


        public static readonly DependencyProperty PurposeProperty = DependencyProperty.Register(
            nameof(Purpose),
            typeof(DialogPurpose),
            typeof(DialogPage),
            new PropertyMetadata(default(DialogPurpose)));


        public static readonly DependencyProperty CompletedCommandProperty = DependencyProperty.Register(
            nameof(CompletedCommand),
            typeof(ICommand),
            typeof(DialogPage),
            new PropertyMetadata(default(ICommand)));


        public static readonly DependencyProperty CancelButtonTextProperty = DependencyProperty.Register(
            nameof(CancelButtonText),
            typeof(string),
            typeof(DialogPage),
            new PropertyMetadata(Services.Language.CancelText));


        public static readonly DependencyProperty CompletedButtonTextProperty = DependencyProperty.Register(
            nameof(CompletedButtonText),
            typeof(string),
            typeof(DialogPage),
            new PropertyMetadata(Services.Language.CompleteText));

        public string CompletedButtonText
        {
            get => (string)GetValue(CompletedButtonTextProperty);
            set => SetValue(CompletedButtonTextProperty, value);
        }

        public string CancelButtonText
        {
            get => (string)GetValue(CancelButtonTextProperty);
            set => SetValue(CancelButtonTextProperty, value);
        }

        public static readonly DependencyProperty CancelCommandProperty = DependencyProperty.Register(
            nameof(CancelCommand),
            typeof(ICommand),
            typeof(DialogPage),
            new PropertyMetadata(default(ICommand)));

        public ICommand CancelCommand
        {
            get => (ICommand)GetValue(CancelCommandProperty);
            set => SetValue(CancelCommandProperty, value);
        }

        public ICommand CompletedCommand
        {
            get => (ICommand)GetValue(CompletedCommandProperty);
            set => SetValue(CompletedCommandProperty, value);
        }

        static DialogPage()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DialogPage), new FrameworkPropertyMetadata(typeof(DialogPage)));
        }

        public DialogPurpose Purpose
        {
            get => (DialogPurpose)GetValue(PurposeProperty);
            set => SetValue(PurposeProperty, value);
        }

        public Brush Accent
        {
            get => (Brush)GetValue(AccentProperty);
            set => SetValue(AccentProperty, value);
        }
        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }
    }
}