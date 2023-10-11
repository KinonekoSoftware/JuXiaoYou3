namespace Acorisoft.FutureGL.MigaStudio.Resources.Converters
{
    public static class ConverterPool
    {
        public static AvatarConverter Avatar { get; } = new AvatarConverter();
        public static ImageFromMusicConverter Music { get; } = new ImageFromMusicConverter();
        public static ImageFromAlbumConverter Image { get; } = new ImageFromAlbumConverter();
        public static ScopedImageConverter ScopedImage { get; } = new ScopedImageConverter();
        public static ImageFromScaledStringConverter ScaledImage { get; } = new ImageFromScaledStringConverter();
        public static NullSwitchStringConverter NullSwitchString { get; } = new NullSwitchStringConverter();
    }
}