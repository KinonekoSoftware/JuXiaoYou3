using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows.Media.Imaging;
using Acorisoft.FutureGL.Forest.Utils;
using Acorisoft.FutureGL.Forest.Models;
using DryIoc;

// ReSharper disable ForCanBeConvertedToForeach

namespace Acorisoft.FutureGL.Forest
{
    /// <summary>
    /// <see cref="Xaml"/> 类型表示一个Xaml帮助类。
    /// </summary>
    public static partial class Xaml
    {
        public static readonly SolidColorBrush                 Transparent  = new SolidColorBrush(Colors.Transparent);
        public static readonly GuidelineSet                    GuidelineSet = new GuidelineSet(new[] { 0.5d, 0.5d }, new[] { 0.5d, 0.5d });
        static readonly        Dictionary<Type, BindingInfo>   ViewInfoMapper;
        static readonly        Dictionary<Type, BindingInfo>   ViewModelInfoMapper;
        static readonly        Dictionary<Type, object>        InstanceScope;
        static readonly        Dictionary<string, Geometry>    FastGeometryMapper;
        static readonly        Dictionary<string, ImageSource> FastImageMapper;
        static readonly        Dictionary<string, ImageBrush>  FastImageBrushMapper;        
        public static readonly GeometryGroup                   Checked;
        public static readonly Geometry                        InfoGeometry;
        public static readonly Geometry                        ErrorGeometry;
        

        static Xaml()
        {
            Container = new Container(x => x.With(FactoryMethod.ConstructorWithResolvableArguments)
                                            .WithTrackingDisposableTransients());
            ViewInfoMapper       = new Dictionary<Type, BindingInfo>();
            ViewModelInfoMapper  = new Dictionary<Type, BindingInfo>();
            InstanceScope        = new Dictionary<Type, object>();
            FastGeometryMapper   = new Dictionary<string, Geometry>();
            FastImageMapper      = new Dictionary<string, ImageSource>();
            FastImageBrushMapper = new Dictionary<string, ImageBrush>(); InfoGeometry = Geometry.Parse("F1 M24,24z M0,0z M21,15A2,2,0,0,1,19,17L7,17 3,21 3,5A2,2,0,0,1,5,3L19,3A2,2,0,0,1,21,5z");
            ErrorGeometry = new GeometryGroup
            {
                Children = new GeometryCollection
                {
                    new LineGeometry { StartPoint = new Point(18, 6), EndPoint = new Point(6, 18) },
                    new LineGeometry { StartPoint = new Point(6, 6), EndPoint  = new Point(18, 18) },
                }
            };
            Checked = new GeometryGroup
            {
                Children = new GeometryCollection
                {
                    new LineGeometry
                    {
                        StartPoint = new Point(12, 5),
                        EndPoint   = new Point(12, 19)
                    },
                    Geometry.Parse("F1 M24,24z M0,0z M19,12L19,12 12,19 5,12")
                }
            };
        }

        #region Commons

        public static bool AlwaysTrue<T>(T _) => true;
        public static bool AlwaysFalse<T>(T _) => false;

        #endregion

        #region Color

        /// <summary>
        /// Convert a html colour code to an OpenTK colour object.
        /// </summary>
        /// <param name="hexCode">The html style hex colour code.</param>
        /// <returns>A colour from the rainbow.</returns>
        public static Color FromHex(string hexCode)
        {
            if (string.IsNullOrEmpty(hexCode))
            {
                return Colors.White;
            }

            // Remove the # if it exists.
            var hex = hexCode.TrimStart('#');

            // Create the colour that we will work on.
            var colour = new Color();

            // If we are working with the shorter hex colour codes, duplicate each character as per the
            // spec https://www.w3.org/TR/2001/WD-css3-color-20010305#colorunits
            // (From E3F to EE33FF)
            if (hex.Length is 3 or 4)
            {
                var longHex = "";

                // For each character in the short hex code add two to the long hex code.
                for (var i = 0; i < hex.Length; i++)
                {
                    longHex += hex[i];
                    longHex += hex[i];
                }

                // the short hex is now the long hex.
                hex = longHex;
            }

            try
            {
                const NumberStyles hexStyle = NumberStyles.HexNumber;
                // We should be working with hex codes that are 6 or 8 characters long.
                if (hex.Length is 6)
                {
                    // Create a constant of the style we want (I don't want to type NumberStyles.HexNumber 4
                    // more times.)
                    colour.R = byte.Parse(hex[..2], hexStyle);
                    colour.G = byte.Parse(hex.Substring(2, 2), hexStyle);
                    colour.B = byte.Parse(hex.Substring(4, 2), hexStyle);

                    // We are done, return the parsed colour.
                    colour.A = 255;
                    return colour;
                }

                if (hex.Length is 8)
                {
                    // Create a constant of the style we want (I don't want to type NumberStyles.HexNumber 4
                    // more times.)

                    // Parse Red, Green and Blue from each pair of characters.
                    colour.A = byte.Parse(hex[..2], hexStyle);
                    colour.R = byte.Parse(hex.Substring(2, 2), hexStyle);
                    colour.G = byte.Parse(hex.Substring(4, 2), hexStyle);
                    colour.B = byte.Parse(hex.Substring(6, 2), hexStyle);

                    // We are done, return the parsed colour.
                    return colour;
                }
            }
            catch
            {
                return Colors.White;
            }

            return Colors.White;
        }

        #endregion

        /// <summary>
        /// 寻找指定元素的父级。
        /// </summary>
        /// <param name="source"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T FindAncestor<T>(DependencyObject source) where T : DependencyObject
        {
            while (source != null && source.GetType() != typeof(T))
            {
                source = VisualTreeHelper.GetParent(source);
            }

            return source as T;
        }

        public static T FindChild<T>(DependencyObject source) where T : DependencyObject
        {
            var count = VisualTreeHelper.GetChildrenCount(source);

            for (var i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(source, i);
                if (child is T instance)
                {
                    return instance;
                }
            }

            return default(T);
        }

        public static T GetViewModel<T>(Type key)
        {
            //
            // instance是一个ViewModel
            // 返回 View
            if (ViewModelInfoMapper.TryGetValue(key, out var info))
            {
                return (T)Activator.CreateInstance(info.ViewModel);
            }

            return default(T);
        }

        public static T GetViewModel<T>()
        {
            var key = typeof(T);

            //
            // instance是一个ViewModel
            // 返回 View
            if (ViewModelInfoMapper.TryGetValue(key, out var info))
            {
                return (T)Activator.CreateInstance(info.ViewModel);
            }

            return default(T);
        }

        public static object Connect(Type key)
        {
            if (key is null)
            {
                return null;
            }

            //
            // instance是一个ViewModel
            // 返回 View
            if (ViewModelInfoMapper.TryGetValue(key, out var info))
            {
                var view      = (FrameworkElement)Activator.CreateInstance(info.View);
                var viewModel = Activator.CreateInstance(info.ViewModel);

                //
                //
                if (view is not null)
                {
                    view.DataContext = viewModel;
                }

                return view;
            }

            //
            // instance是一个View
            // 返回 ViewModel
            // ReSharper disable once InvertIf
            if (ViewInfoMapper.TryGetValue(key, out info))
            {
                var view      = (FrameworkElement)Activator.CreateInstance(info.View);
                var viewModel = Activator.CreateInstance(info.ViewModel);

                //
                //
                if (view is not null)
                {
                    view.DataContext = viewModel;
                }

                return view;
            }

            return null;
        }


        /// <summary>
        /// 连接视图或者视图模型。
        /// </summary>
        /// <param name="instance">当前实例（仅限于视图或者视图模型）</param>
        /// <returns>返回一个值</returns>
        public static object Connect(object instance)
        {
            if (instance is null)
            {
                return null;
            }

            var key = instance.GetType();

            //
            // instance是一个ViewModel
            // 返回 View
            if (ViewModelInfoMapper.TryGetValue(key, out var info))
            {
                return (FrameworkElement)Activator.CreateInstance(info.View);
            }

            //
            // instance是一个View
            // 返回 ViewModel
            // ReSharper disable once InvertIf
            if (ViewInfoMapper.TryGetValue(key, out info) &&
                instance is FrameworkElement)
            {
                return instance;
            }

            Debug.WriteLine($"VM:{instance.GetType().Name}的视图无法找到！请注意绑定！");
            return null;
        }

        /// <summary>
        /// 关联指定的视图与视图模型
        /// </summary>
        /// <param name="bindingInfo">绑定信息。</param>
        public static void InstallView(BindingInfo bindingInfo)
        {
            if (bindingInfo is null)
            {
                return;
            }

            if (bindingInfo.IsSingleton)
            {
                // ReSharper disable once InvertIf
                if (!InstanceScope.ContainsKey(bindingInfo.ViewModel))
                {
                    InstanceScope.TryAdd(bindingInfo.ViewModel, Activator.CreateInstance(bindingInfo.ViewModel));
                    ViewInfoMapper.TryAdd(bindingInfo.View, bindingInfo);
                    ViewModelInfoMapper.TryAdd(bindingInfo.ViewModel, bindingInfo);
                }

                return;
            }

            ViewInfoMapper.TryAdd(bindingInfo.View, bindingInfo);
            ViewModelInfoMapper.TryAdd(bindingInfo.ViewModel, bindingInfo);
        }

        public static void InstallView<TView, TViewModel>() where TView : UserControl where TViewModel : IViewModel =>
            InstallView(new BindingInfo
            {
                ViewModel = typeof(TViewModel),
                View      = typeof(TView)
            });

        /// <summary>
        /// 
        /// </summary>
        public static void InstallViewFromSourceGenerator()
        {
            var implType = Classes.FindInterfaceImplementation<IViewModelRegister>(Assembly.GetEntryAssembly());

            if (implType is null)
            {
                return;
            }

            var impl = (IViewModelRegister)Activator.CreateInstance(implType);
            impl?.Register(new Installer());
        }

        public static void InstallViewManually(IBindingInfoProvider provider)
        {
            if (provider is null)
            {
                return;
            }

            foreach (var info in provider.GetBindingInfo())
            {
                InstallView(info);
            }
        }

        /// <summary>
        /// 手动安装视图。
        /// </summary>
        /// <param name="providers"></param>
        public static void InstallViewManually(IEnumerable<IBindingInfoProvider> providers)
        {
            if (providers is null)
            {
                return;
            }

            foreach (var provider in providers)
            {
                if (provider is null)
                {
                    continue;
                }

                foreach (var info in provider.GetBindingInfo())
                {
                    InstallView(info);
                }
            }
        }

        #region GetResource

        public static Geometry GetGeometry(string key)
        {
            if (!FastGeometryMapper.TryGetValue(key, out var geometry))
            {
                geometry = Application.Current.Resources[key] as Geometry;

                if (geometry is not null)
                {
                    FastGeometryMapper.TryAdd(key, geometry);
                }
            }

            return geometry;
        }

        public static ImageSource GetImageSource(string key)
        {
            if (!FastImageMapper.TryGetValue(key, out var imageSource))
            {
                imageSource = Application.Current.Resources[key] as ImageSource;

                if (imageSource is not null)
                {
                    FastImageMapper.TryAdd(key, imageSource);
                }
            }

            return imageSource;
        }


        public static ImageBrush GetImageBrush(string key)
        {
            if (!FastImageBrushMapper.TryGetValue(key, out var imageBrush))
            {
                imageBrush = Application.Current.Resources[key] as ImageBrush;

                if (imageBrush is not null)
                {
                    FastImageBrushMapper.TryAdd(key, imageBrush);
                }
            }

            return imageBrush;
        }

        #endregion

        /// <summary>
        /// 转化为纯色画刷
        /// </summary>
        /// <param name="color">要转换的颜色</param>
        /// <returns>返回纯色画刷</returns>
        public static SolidColorBrush ToSolidColorBrush(this Color color) => new SolidColorBrush(color);

        public static ImageSource FromStream(byte[] buffer, int w, int h)
        {
            var ms = new MemoryStream(buffer);
            return FromStream(ms, w, h);
        }

        public static ImageSource FromStream(MemoryStream ms, int w, int h)
        {
            ms.Seek(0, SeekOrigin.Begin);
            var bi = new BitmapImage();
            bi.BeginInit();
            bi.CacheOption       = BitmapCacheOption.OnLoad;
            bi.DecodePixelWidth  = w;
            bi.DecodePixelHeight = h;
            bi.StreamSource      = ms;
            bi.EndInit();
            bi.Freeze();
            return bi;
        }

        /// <summary>
        /// 渲染
        /// </summary>
        /// <param name="element"></param>
        /// <param name="dip"></param>
        /// <returns></returns>
        public static RenderTargetBitmap Capture(FrameworkElement element, int dip = 96)
        {
            if (element is null)
            {
                return null;
            }

            var bitmap = new RenderTargetBitmap(
                (int)element.ActualWidth,
                (int)element.ActualHeight,
                dip,
                dip,
                PixelFormats.Pbgra32);
            bitmap.Render(element);

            return bitmap;
        }

        /// <summary>
        /// 渲染
        /// </summary>
        /// <param name="fileStream"></param>
        /// <param name="element"></param>
        /// <param name="dip"></param>
        /// <returns></returns>
        public static RenderTargetBitmap Capture(Stream fileStream, FrameworkElement element, int dip = 96)
        {
            if (element is null)
            {
                return null;
            }

            var bitmap = new RenderTargetBitmap(
                (int)element.ActualWidth,
                (int)element.ActualHeight,
                dip,
                dip,
                PixelFormats.Pbgra32);
            bitmap.Render(element);
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));
            encoder.Save(fileStream);
            fileStream.Dispose();

            return bitmap;
        }

        public static MemoryStream CaptureToStream(FrameworkElement element, int dip = 96)
        {
            if (element is null)
            {
                return null;
            }

            var bitmap = new RenderTargetBitmap(
                (int)element.ActualWidth,
                (int)element.ActualHeight,
                dip,
                dip,
                PixelFormats.Pbgra32);
            bitmap.Render(element);
            var ms      = new MemoryStream();
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));
            encoder.Save(ms);

            return ms;
        }

        public static byte[] CaptureToBuffer(FrameworkElement element, int dip = 96)
        {
            if (element is null)
            {
                return null;
            }

            var bitmap = new RenderTargetBitmap(
                (int)element.ActualWidth,
                (int)element.ActualHeight,
                dip,
                dip,
                PixelFormats.Pbgra32);
            bitmap.Render(element);
            var ms      = new MemoryStream();
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));
            encoder.Save(ms);

            return ms.ToArray();
        }
    }
}