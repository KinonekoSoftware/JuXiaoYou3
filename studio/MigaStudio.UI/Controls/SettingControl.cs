namespace Acorisoft.FutureGL.MigaStudio.Controls
{
    public class SettingControl : ContentControl
    {

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            nameof(Title),
            typeof(string),
            typeof(SettingControl),
            new PropertyMetadata(default(string)));

        public static readonly DependencyProperty IntroProperty = DependencyProperty.Register(
            nameof(Intro),
            typeof(string),
            typeof(SettingControl),
            new PropertyMetadata(default(string)));

        public static readonly DependencyProperty SubTitleFontFamilyProperty = DependencyProperty.Register(
            nameof(SubTitleFontFamily),
            typeof(FontFamily),
            typeof(SettingControl),
            new PropertyMetadata(default(FontFamily)));

        public FontFamily SubTitleFontFamily
        {
            get => (FontFamily)GetValue(SubTitleFontFamilyProperty);
            set => SetValue(SubTitleFontFamilyProperty, value);
        }
        public string Intro
        {
            get => (string)GetValue(IntroProperty);
            set => SetValue(IntroProperty, value);
        }
        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }
    }
}