namespace Acorisoft.FutureGL.MigaStudio.Controls
{
    public class RelationshipCard : Control
    {
        static RelationshipCard()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(RelationshipCard), 
                new FrameworkPropertyMetadata(typeof(RelationshipCard)));
        }


        public static readonly DependencyProperty TopNameProperty = DependencyProperty.Register(
            nameof(TopName),
            typeof(string),
            typeof(RelationshipCard),
            new PropertyMetadata(default(string)));

        public static readonly DependencyProperty BottomNameProperty = DependencyProperty.Register(
            nameof(BottomName),
            typeof(string),
            typeof(RelationshipCard),
            new PropertyMetadata(default(string)));

        public static readonly DependencyProperty SourceImageProperty = DependencyProperty.Register(
            nameof(SourceImage),
            typeof(ImageSource),
            typeof(RelationshipCard),
            new PropertyMetadata(default(ImageSource)));


        public static readonly DependencyProperty TargetImageProperty = DependencyProperty.Register(
            nameof(TargetImage),
            typeof(ImageSource),
            typeof(RelationshipCard),
            new PropertyMetadata(default(ImageSource)));

        public static readonly DependencyProperty SourceNameProperty = DependencyProperty.Register(
            nameof(SourceName),
            typeof(string),
            typeof(RelationshipCard),
            new PropertyMetadata(default(string)));

        public static readonly DependencyProperty TargetNameProperty = DependencyProperty.Register(
            nameof(TargetName),
            typeof(string),
            typeof(RelationshipCard),
            new PropertyMetadata(default(string)));

        public string TargetName
        {
            get => (string)GetValue(TargetNameProperty);
            set => SetValue(TargetNameProperty, value);
        }
        public string SourceName
        {
            get => (string)GetValue(SourceNameProperty);
            set => SetValue(SourceNameProperty, value);
        }
        public ImageSource TargetImage
        {
            get => (ImageSource)GetValue(TargetImageProperty);
            set => SetValue(TargetImageProperty, value);
        }

        public ImageSource SourceImage
        {
            get => (ImageSource)GetValue(SourceImageProperty);
            set => SetValue(SourceImageProperty, value);
        }

        public string BottomName
        {
            get => (string)GetValue(BottomNameProperty);
            set => SetValue(BottomNameProperty, value);
        }
        
        public string TopName
        {
            get => (string)GetValue(TopNameProperty);
            set => SetValue(TopNameProperty, value);
        }
    }
}