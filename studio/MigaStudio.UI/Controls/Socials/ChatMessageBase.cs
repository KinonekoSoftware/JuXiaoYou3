using Acorisoft.FutureGL.MigaDB.Enums;

namespace Acorisoft.FutureGL.MigaStudio.Controls.Socials
{
    
    public abstract class ChatMessageBase : Control
    {

    }

    public abstract class UserMessageBase : ChatMessageBase
    {
        
        public static readonly DependencyProperty IsSelfProperty = DependencyProperty.Register(
            nameof(IsSelf),
            typeof(bool),
            typeof(UserMessageBase),
            new PropertyMetadata(Boxing.False));
        
        public static readonly DependencyProperty CharacterAvatarProperty = DependencyProperty.Register(
            nameof(CharacterAvatar),
            typeof(ImageSource),
            typeof(UserMessageBase),
            new PropertyMetadata(default(ImageBrush)));


        public static readonly DependencyProperty CharacterNameProperty = DependencyProperty.Register(
            nameof(CharacterName),
            typeof(string),
            typeof(UserMessageBase),
            new PropertyMetadata(default(string)));


        public static readonly DependencyProperty CharacterTitleProperty = DependencyProperty.Register(
            nameof(CharacterTitle),
            typeof(string),
            typeof(UserMessageBase),
            new PropertyMetadata(default(string)));


        public static readonly DependencyProperty CharacterTitleBrushProperty = DependencyProperty.Register(
            nameof(CharacterTitleBrush),
            typeof(Brush),
            typeof(UserMessageBase),
            new PropertyMetadata(default(ImageSource)));
        
        public static readonly DependencyProperty LayoutProperty = DependencyProperty.Register(
            nameof(Layout),
            typeof(PreferSocialMessageLayout),
            typeof(UserMessageBase),
            new PropertyMetadata(default(PreferSocialMessageLayout)));


        public static readonly DependencyProperty ShowTitleProperty = DependencyProperty.Register(
            nameof(ShowTitle),
            typeof(bool),
            typeof(UserMessageBase),
            new PropertyMetadata(Boxing.False));

        public bool ShowTitle
        {
            get => (bool)GetValue(ShowTitleProperty);
            set => SetValue(ShowTitleProperty, Boxing.Box(value));
        }

        public Brush CharacterTitleBrush
        {
            get => (Brush)GetValue(CharacterTitleBrushProperty);
            set => SetValue(CharacterTitleBrushProperty, value);
        }
        
        public PreferSocialMessageLayout Layout
        {
            get => (PreferSocialMessageLayout)GetValue(LayoutProperty);
            set => SetValue(LayoutProperty, value);
        }

        public string CharacterTitle
        {
            get => (string)GetValue(CharacterTitleProperty);
            set => SetValue(CharacterTitleProperty, value);
        }

        public string CharacterName
        {
            get => (string)GetValue(CharacterNameProperty);
            set => SetValue(CharacterNameProperty, value);
        }
        
        public ImageSource CharacterAvatar
        {
            get => (ImageSource)GetValue(CharacterAvatarProperty);
            set => SetValue(CharacterAvatarProperty, value);
        }

        public bool IsSelf
        {
            get => (bool)GetValue(IsSelfProperty);
            set => SetValue(IsSelfProperty, Boxing.Box(value));
        }
    }
}