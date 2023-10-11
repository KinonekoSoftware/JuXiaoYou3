namespace Acorisoft.FutureGL.MigaStudio.Controls.Socials
{
    public class MemberJoinMessageControl : ChatMessageBase
    {
        static MemberJoinMessageControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MemberJoinMessageControl), new FrameworkPropertyMetadata(typeof(MemberJoinMessageControl)));
        }


        public static readonly DependencyProperty CharacterNameProperty = DependencyProperty.Register(
            nameof(CharacterName),
            typeof(string),
            typeof(MemberJoinMessageControl),
            new PropertyMetadata(default(string)));


        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(
            nameof(Content),
            typeof(string),
            typeof(MemberJoinMessageControl),
            new PropertyMetadata(default(string)));


        public static readonly DependencyProperty WelcomeContentProperty = DependencyProperty.Register(
            nameof(WelcomeContent),
            typeof(string),
            typeof(MemberJoinMessageControl),
            new PropertyMetadata(default(string)));

        public string WelcomeContent
        {
            get => (string)GetValue(WelcomeContentProperty);
            set => SetValue(WelcomeContentProperty, value);
        }
        
        public string Content
        {
            get => (string)GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        public string CharacterName
        {
            get => (string)GetValue(CharacterNameProperty);
            set => SetValue(CharacterNameProperty, value);
        }
    }
}