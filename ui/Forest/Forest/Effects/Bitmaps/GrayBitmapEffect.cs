using System.Windows.Media.Effects;

namespace Acorisoft.FutureGL.Forest.Effects.Bitmaps
{
    public class GrayBitmapEffect : ShaderEffect
    {
        public static readonly DependencyProperty InputProperty = RegisterPixelShaderSamplerProperty("Input", typeof(GrayBitmapEffect), 0);

        public GrayBitmapEffect()
        {
            var pixelShader = new PixelShader
            {
                UriSource = new Uri("pack://application:,,,/Forest;component/Effects/Bitmaps/GrayBitmapEffect.ps", UriKind.RelativeOrAbsolute)
            };
            PixelShader = pixelShader;

            UpdateShaderValue(InputProperty);
        }

        public Brush Input
        {
            get => ((Brush)(GetValue(InputProperty)));
            set => SetValue(InputProperty, value);
        }
    }
}