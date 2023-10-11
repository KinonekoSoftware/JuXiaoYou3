using System.Collections.Generic;
using Acorisoft.FutureGL.MigaDB.Data.Socials;

namespace Acorisoft.FutureGL.MigaStudio.Controls.Socials
{
    public class SocialSoftwareLayout : ContentControl
    {
        static SocialSoftwareLayout()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SocialSoftwareLayout), new FrameworkPropertyMetadata(typeof(SocialSoftwareLayout)));
        }
        
        public static readonly DependencyProperty ChannelNameProperty = DependencyProperty.Register(
            nameof(ChannelName),
            typeof(string),
            typeof(SocialSoftwareLayout),
            new PropertyMetadata(default(string)));


        public static readonly DependencyProperty ChannelSubtitleProperty = DependencyProperty.Register(
            nameof(ChannelSubtitle),
            typeof(string),
            typeof(SocialSoftwareLayout),
            new PropertyMetadata(default(string)));


        public static readonly DependencyProperty PendingContentProperty = DependencyProperty.Register(
            nameof(PendingContent),
            typeof(string),
            typeof(SocialSoftwareLayout),
            new PropertyMetadata(default(string)));


        public static readonly DependencyProperty LayoutProperty = DependencyProperty.Register(
            nameof(Layout),
            typeof(PreferSocialLayout),
            typeof(SocialSoftwareLayout),
            new PropertyMetadata(default(PreferSocialLayout)));

        public PreferSocialLayout Layout
        {
            get => (PreferSocialLayout)GetValue(LayoutProperty);
            set => SetValue(LayoutProperty, value);
        }

        public string PendingContent
        {
            get => (string)GetValue(PendingContentProperty);
            set => SetValue(PendingContentProperty, value);
        }

        public string ChannelSubtitle
        {
            get => (string)GetValue(ChannelSubtitleProperty);
            set => SetValue(ChannelSubtitleProperty, value);
        }
        
        public string ChannelName
        {
            get => (string)GetValue(ChannelNameProperty);
            set => SetValue(ChannelNameProperty, value);
        }
    }
}