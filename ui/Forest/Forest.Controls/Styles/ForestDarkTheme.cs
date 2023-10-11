using System.Collections.Concurrent;


namespace Acorisoft.FutureGL.Forest.Styles
{
    public class ForestDarkTheme : ForestThemeSystem
    {

        protected override void InitializeOverlayBrush(ConcurrentDictionary<int, Color> colors)
        {
            colors.TryAdd((int)ForestTheme.Overlay100, Color.FromArgb(0x28, 0xff, 0xff, 0xff));
            colors.TryAdd((int)ForestTheme.Overlay200, Color.FromArgb(0x50, 0xff, 0xff, 0xff));
        }

        protected override void InitializeBackgroundBrush(ConcurrentDictionary<int, Color> colors)
        {
            colors.TryAdd((int)ForestTheme.BorderBrush, Color.FromRgb( 0x77, 0x7c, 0x88));
            colors.TryAdd((int)ForestTheme.BackgroundLevel1, Color.FromRgb( 0x54, 0x57, 0x5F));
            colors.TryAdd((int)ForestTheme.BackgroundLevel2, Color.FromRgb( 0x43, 0x46, 0x4C));
            colors.TryAdd((int)ForestTheme.BackgroundLevel3, Color.FromRgb( 0x30, 0x31, 0x36));
            colors.TryAdd((int)ForestTheme.BackgroundLevel4, Color.FromRgb( 0x24, 0x25, 0x29));
            colors.TryAdd((int)ForestTheme.BackgroundLevel5, Color.FromRgb( 0x15, 0x16, 0x18));
            colors.TryAdd((int)ForestTheme.BackgroundLevel6, Color.FromRgb( 0x1D, 0x1E, 0x21));
            colors.TryAdd((int)ForestTheme.BackgroundDisabled, Color.FromRgb( 0x3C, 0x3E, 0x44));
        }

        protected override void InitializeForegroundBrush(ConcurrentDictionary<int, Color> colors)
        {
            colors.TryAdd((int)ForestTheme.ForegroundLevel1, Color.FromRgb( 0xEB, 0xEB, 0xEB));
            colors.TryAdd((int)ForestTheme.ForegroundLevel2, Color.FromRgb( 0x8C, 0x8C, 0x8C));
            colors.TryAdd((int)ForestTheme.ForegroundLevel3, Color.FromRgb( 0x73, 0x73, 0x73));
            colors.TryAdd((int)ForestTheme.ForegroundOther, Color.FromRgb( 0x59, 0x59, 0x59));
            colors.TryAdd((int)ForestTheme.ForegroundDisabled, Color.FromRgb( 0x73, 0x73, 0x73));
            colors.TryAdd((int)ForestTheme.ForegroundInHighlight, Color.FromRgb( 0xff, 0xff, 0xff));
            colors.TryAdd((int)ForestTheme.ForegroundInAdaptive, Color.FromRgb(0x1d, 0x1e, 0x21));
            colors.TryAdd((int)ForestTheme.ForegroundInAdaptive2, Color.FromRgb(0x30, 0x31, 0x36));
        }

        protected override void InitializeAdaptiveBrush(ConcurrentDictionary<int, Color> colors)
        {
            colors.TryAdd((int)ForestTheme.AdaptiveLevel1, Color.FromRgb( 0xae, 0xb0, 0xb7));
            colors.TryAdd((int)ForestTheme.AdaptiveLevel2, Color.FromRgb( 0x8d, 0x91, 0x9a));
            colors.TryAdd((int)ForestTheme.AdaptiveLevel3, Color.FromRgb( 0x65, 0x68, 0x72));
        }
    }
}