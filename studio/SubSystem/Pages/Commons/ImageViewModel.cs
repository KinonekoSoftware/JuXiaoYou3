using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons
{
    public class ImageViewModel : DialogViewModel
    {
        private MemoryStream _ms;
        
        protected override void OnStart(RoutingEventArgs parameter)
        {
            var fileName = parameter.Parameter
                                    .Args[0]
                                    ?.ToString();
            var buffer = File.ReadAllBytes(fileName);
            _ms    = new MemoryStream(buffer);
            var bi = new BitmapImage();
            bi.BeginInit();
            bi.CacheOption  = BitmapCacheOption.OnLoad;
            bi.StreamSource = _ms;
            bi.EndInit();
            Source = bi;
            base.OnStart(parameter);
        }

        protected override void ReleaseManagedResourcesOverride()
        {
            _ms.Close();
            _ms.Dispose();
            _ms = null;
            base.ReleaseManagedResourcesOverride();
        }

        private ImageSource _source;

        /// <summary>
        /// 获取或设置 <see cref="Source"/> 属性。
        /// </summary>
        public ImageSource Source
        {
            get => _source;
            set => SetValue(ref _source, value);
        }
    }
}