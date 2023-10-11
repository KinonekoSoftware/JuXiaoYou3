using System.Collections.Concurrent;


namespace Acorisoft.FutureGL.Forest.Styles
{
    public class ForestLightTheme : ForestThemeSystem
    {
        protected override void InitializeOverlayBrush(ConcurrentDictionary<int, Color> colors)
        {
            colors.TryAdd((int)ForestTheme.Overlay100, Color.FromArgb(0x20, 0x00, 0x00, 0x00));
            colors.TryAdd((int)ForestTheme.Overlay200, Color.FromArgb(0x50, 0x00, 0x00, 0x00));
        }

        protected override void InitializeBackgroundBrush(ConcurrentDictionary<int, Color> colors)
        {
            colors.TryAdd((int)ForestTheme.BorderBrush, Color.FromRgb( 0xc9, 0xc9, 0xc9));
            colors.TryAdd((int)ForestTheme.BackgroundLevel1, Color.FromRgb( 0xff, 0xff, 0xff));
            colors.TryAdd((int)ForestTheme.BackgroundLevel2, Color.FromRgb( 0xf1, 0xf1, 0xf1));
            colors.TryAdd((int)ForestTheme.BackgroundLevel3, Color.FromRgb( 0xE8, 0xE8, 0xE8));
            colors.TryAdd((int)ForestTheme.BackgroundLevel4, Color.FromRgb( 0xD9, 0xD9, 0xD9));
            colors.TryAdd((int)ForestTheme.BackgroundLevel5, Color.FromRgb( 0xCC, 0xCC, 0xCC));
            colors.TryAdd((int)ForestTheme.BackgroundLevel6, Color.FromRgb( 0xBF, 0xBF, 0xBF));
            colors.TryAdd((int)ForestTheme.BackgroundDisabled, Color.FromRgb( 0xf6, 0xf6, 0xf6));
        }

        protected override void InitializeForegroundBrush(ConcurrentDictionary<int, Color> colors)
        {
            colors.TryAdd((int)ForestTheme.ForegroundLevel1, Color.FromRgb( 0x00, 0x0d, 0x16));
            colors.TryAdd((int)ForestTheme.ForegroundLevel2, Color.FromRgb( 0x75, 0x81, 0x8a));
            colors.TryAdd((int)ForestTheme.ForegroundLevel3, Color.FromRgb( 0x9f, 0xa7, 0xad));
            colors.TryAdd((int)ForestTheme.ForegroundOther, Color.FromRgb( 0x84, 0x92, 0xa6));
            colors.TryAdd((int)ForestTheme.ForegroundDisabled, Color.FromRgb( 0xba, 0xc0, 0xc4));
            colors.TryAdd((int)ForestTheme.ForegroundInHighlight, Color.FromRgb( 0xff, 0xff, 0xff));
            colors.TryAdd((int)ForestTheme.ForegroundInAdaptive, Color.FromRgb( 0x00, 0x0d, 0x16));
            colors.TryAdd((int)ForestTheme.ForegroundInAdaptive2, Color.FromRgb(0x75, 0x81, 0x8a));
        }

        protected override void InitializeAdaptiveBrush(ConcurrentDictionary<int, Color> colors)
        {
            colors.TryAdd((int)ForestTheme.AdaptiveLevel1, Color.FromRgb( 0xA6, 0xA6, 0xA6));
            colors.TryAdd((int)ForestTheme.AdaptiveLevel2, Color.FromRgb( 0x8C, 0x8C, 0x8C));
            colors.TryAdd((int)ForestTheme.AdaptiveLevel3, Color.FromRgb( 0x73, 0x73, 0x73));
        }
    }
}