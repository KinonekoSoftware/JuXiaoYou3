using System.Windows.Shapes;
namespace Acorisoft.FutureGL.MigaStudio.Controls
{
    public abstract class GeometryGeneratedControl : GeneratedControl
    {
        protected const string Thumb = "F1 M24,24z M0,0z M20.84,4.61A5.5,5.5,0,0,0,13.06,4.61L12,5.67 10.94,4.61A5.5,5.5,0,0,0,3.16,12.39L4.22,13.45 12,21.23 19.78,13.45 20.84,12.39A5.5,5.5,0,0,0,20.84,4.61z";
        protected const string Heart = "F1 M24,24z M0,0z M20.84,4.61A5.5,5.5,0,0,0,13.06,4.61L12,5.67 10.94,4.61A5.5,5.5,0,0,0,3.16,12.39L4.22,13.45 12,21.23 19.78,13.45 20.84,12.39A5.5,5.5,0,0,0,20.84,4.61z";
        protected const string Rate  = "F1 M24,24z M0,0z M12,2L12,2 15.09,8.26 22,9.27 17,14.14 18.18,21.02 12,17.77 5.82,21.02 7,14.14 2,9.27 8.91,8.26 12,2z";

        private readonly Geometry _geometry;
        private readonly Brush    _brush;

        protected GeometryGeneratedControl(Color accent, string path)
        {
            _brush    = new SolidColorBrush(accent);
            _geometry = Geometry.Parse(path);
        }

        protected sealed override FrameworkElement GenerateElement()
        {
            return new Path
            {
                Data                = _geometry,
                Height              = 24,
                Fill                = Transparent,
                Stroke              = _brush,
                StrokeThickness     = 1,
                UseLayoutRounding   = true,
                SnapsToDevicePixels = true,
                Width               = 24

            };
        }

        protected sealed override void HighlightElement(FrameworkElement element)
        {
            if (element is Path b)
            {
                b.Fill = _brush;
            }
        }

        protected sealed override void UnhighlightElement(FrameworkElement element)
        {
            if (element is Path b)
            {
                b.Fill = Transparent;
            }
        }
    }

    public class RatingControl : GeometryGeneratedControl
    {
        public RatingControl() : base(Color.FromRgb(255, 193, 7), Rate)
        {

        }
    }

    public class FavoriteControl : GeometryGeneratedControl
    {

        public FavoriteControl() : base(Color.FromRgb(0xff, 0x66, 0x99), Heart)
        {

        }
    }
}