using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons
{
    public class ImageEditViewModel : ImplicitDialogVM
    {
        private double _scale;
        private double _imageWidth;
        private double _imageHeight;
        private double _thumbSize;
        private double _posX;
        private double _posY;

        protected override void OnStart(RoutingEventArgs parameter)
        {
            var args = parameter.Parameter.Args;
            if (args[0] is Image<Rgba32> img &&
                args[1] is MemoryStream ms)
            {
                ms.Seek(0, SeekOrigin.Begin);

                BackendImage    = img;
                BackgroundImage = ms;
                ImageWidth      = img.Width;
                ImageHeight     = img.Height;
            }
        }

        protected override void ReleaseManagedResourcesOverride()
        {
            BackendImage?.Dispose();
            BackgroundImage?.Dispose();
            BackendImage    = null;
            BackgroundImage = null;
        }

        protected override bool IsCompleted() => true;

        protected override void Finish()
        {
            var ms = new MemoryStream();
            BackendImage.Mutate(x => { 
                x.Crop(new Rectangle((int)PosX, (int)PosY, (int)ThumbSize, (int)ThumbSize));
                x.Resize(256, 256);
            });
            BackendImage.SaveAsPng(ms);
            ms.Seek(0, SeekOrigin.Begin);
            Result = ms;
        }

        protected override string Failed() => SubSystemString.Unknown;

        /// <summary>
        /// 目标图片
        /// </summary>
        public Image<Rgba32> BackendImage { get; private set; }

        public MemoryStream BackgroundImage { get; private set; }

        /// <summary>
        /// 获取或设置 <see cref="ThumbSize"/> 属性。
        /// </summary>
        public double PosX
        {
            get => _posX;
            set => SetValue(ref _posX, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="ThumbSize"/> 属性。
        /// </summary>
        public double PosY
        {
            get => _posY;
            set => SetValue(ref _posY, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="ThumbSize"/> 属性。
        /// </summary>
        public double ThumbSize
        {
            get => _thumbSize;
            set => SetValue(ref _thumbSize, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="ImageHeight"/> 属性。
        /// </summary>
        public double ImageHeight
        {
            get => _imageHeight;
            set => SetValue(ref _imageHeight, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="ImageWidth"/> 属性。
        /// </summary>
        public double ImageWidth
        {
            get => _imageWidth;
            set => SetValue(ref _imageWidth, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="Scale"/> 属性。
        /// </summary>
        public double Scale
        {
            get => _scale;
            set => SetValue(ref _scale, value);
        }
    }
}