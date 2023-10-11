using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Acorisoft.FutureGL.Forest.Adorners;
using Point = System.Windows.Point;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons
{
    public partial class ImageEditView
    {
        private const int ScaleFactor = 20;

        public ImageEditView()
        {
            InitializeComponent();
            InstallControls();
        }

        protected override void OnLoaded(object sender, RoutedEventArgs e)
        {
            _grayImage        = new BitmapImage();
            _grayImage.BeginInit();
            _grayImage.StreamSource = ViewModel.BackgroundImage;
            _grayImage.EndInit();
            SetDefaultPositionAndData();
            ScaleDown.IsEnabled = false;
            ScaleRate.Minimum   = 100;
            ScaleRate.Maximum   = _minImageSize / 100 * 100;
        }

        #region StartDragging

        private bool   _startDragging;
        private double _x;
        private double _y;

        private void InstallControls()
        {
            //
            // Thumb
            Thumb.Height               =  100;
            Thumb.Width                =  100;
            Thumb.MouseLeftButtonDown  += DraggingStart;
            Canvas.MouseLeftButtonDown += DraggingStartFromOriginal;
            Canvas.MouseLeave          += DraggingFinished;
            Canvas.MouseLeftButtonUp   += DraggingFinished;
        }

        private void Dragging(object sender, MouseEventArgs e)
        {
            if (!_startDragging) return;
            var pos = e.GetPosition(sender as IInputElement);
            _x             = pos.X;
            _y             = pos.Y;
            SetPositionAndImageCropped(pos);
            EnsureViewPort();
            e.Handled = true;
        }

        private void DraggingStartFromOriginal(object sender, MouseButtonEventArgs e)
        {
            var pos = e.GetPosition(sender as IInputElement);
            _x             = pos.X;
            _y             = pos.Y;
            _startDragging = true;
            SetPositionAndImageCropped(pos);
            EnsureViewPort();

            Canvas.CaptureMouse();
            Canvas.MouseMove += Dragging;
            e.Handled        =  true;
        }

        private void DraggingStart(object sender, MouseButtonEventArgs e)
        {
            var pos = e.GetPosition(sender as IInputElement);
            pos            = Thumb.PointToScreen(pos);
            pos            = Canvas.PointFromScreen(pos);
            _x             = pos.X;
            _y             = pos.Y;
            _startDragging = true;
            SetPositionAndImageCropped(pos);
            EnsureViewPort();

            Canvas.CaptureMouse();
            Canvas.MouseMove += Dragging;
            e.Handled        =  true;
        }

        private void DraggingFinished(object sender, MouseButtonEventArgs e)
        {
            _startDragging   =  false;
            e.Handled        =  true;
            Canvas.MouseMove -= Dragging;
            Canvas.ReleaseMouseCapture();
        }

        private void DraggingFinished(object sender, MouseEventArgs e)
        {
            _startDragging   =  false;
            e.Handled        =  true;
            Canvas.MouseMove -= Dragging;
            Canvas.ReleaseMouseCapture();
        }

        private void EnsureViewPort()
        {
            // var verticalOffset = _y1 - ScrollViewer.ViewportHeight + ThumbSize;
            // var horizontalOffset = _x1 - ScrollViewer.ViewportWidth + ThumbSize;

            // if (verticalOffset > 0)
            // {
            //     ScrollViewer.ScrollToVerticalOffset(verticalOffset - ThumbSize);
            // }
            //
            // if (horizontalOffset > 0)
            // {
            //     ScrollViewer.ScrollToHorizontalOffset(verticalOffset - ThumbSize);
            // }
        }

        #endregion

        #region ImageEditing

        private double        _minImageSize;
        private BitmapImage   _grayImage;
        private double        _thumbOriginalSize;
        private CroppedBitmap _cropped;
        private ImageBrush    _thumbImage;
        private ThumbAdorner  _adorner;
        private AdornerLayer  _layer;
        private int           _x1, _y1;

        private void ReleaseResource()
        {
            _layer?.Remove(_adorner);
            ScaleDown.IsEnabled = true;
            ScaleUp.IsEnabled   = true;
            Scale               = 100;
            _x                  = 0;
            _y                  = 0;
            ImageHeight         = 0;
            ImageWidth          = 0;
            ThumbSize           = 0;
            _thumbImage         = null;
            //
            // release all resource
            _grayImage   = null;
            _cropped     = null;
            ViewModel.BackendImage?.Dispose();
        }

        public static BitmapImage FromStream(Stream stream)
        {
            var bi = new BitmapImage();
            bi.BeginInit();
            bi.CacheOption   = BitmapCacheOption.Default;
            bi.CreateOptions = BitmapCreateOptions.None;
            bi.StreamSource  = stream;
            bi.EndInit();
            bi.Freeze();
            return bi;
        }

        #endregion

        private void SetDefaultPositionAndData()
        {
            _adorner = new ThumbAdorner(Thumb);
            _layer   = AdornerLayer.GetAdornerLayer(Thumb);
            _layer?.Add(_adorner);


            Scale       = 100;
            ImageHeight = ViewModel.BackendImage.Height;
            ImageWidth  = ViewModel.BackendImage.Width;

            if (ImageWidth == 0 || ImageHeight == 0)
            {
                ReleaseResource();
                MessageBox.Show("图片大小异常，请使用其它的图片处理软件编辑！");
                return;
            }

            if (ImageWidth < 32 || ImageHeight < 32)
            {
                ReleaseResource();
                MessageBox.Show("图片大小太小，请使用其它的图片处理软件编辑！");
                return;
            }

            if (ImageWidth > 4096 || ImageHeight > 2560)
            {
                ReleaseResource();
                MessageBox.Show("图片大小过大，请使用其它的图片处理软件编辑！");
                return;
            }


            _minImageSize = Math.Min(ImageHeight, ImageWidth);

            _thumbOriginalSize =
                ThumbSize = Math.Clamp(_minImageSize, Math.Min(_minImageSize, 64), 100);

            //
            // Set Behavior
            // ImageActualSize.Text = $"Actual Size : {ImageWidth},{ImageHeight}";
            // ImageCropSize.Text   = $"Crop Size : {ThumbSize},{ThumbSize}";
            Thumb.Width          = ThumbSize;
            Thumb.Height         = ThumbSize;
            Canvas.SetLeft(Thumb, 0);
            Canvas.SetTop(Thumb, 0);

            //
            //
            _cropped           = new CroppedBitmap(_grayImage, new Int32Rect(0, 0, (int)ThumbSize, (int)ThumbSize));
            _thumbImage        = new ImageBrush { ImageSource = _cropped };
            ActualImage.Source = _grayImage;
            ActualImage.Width  = ImageWidth;
            ActualImage.Height = ImageHeight;
            Canvas.Width       = ImageWidth;
            Canvas.Height      = ImageHeight;

            //
            // Thumb And Presentation
            Thumb.Background   = _thumbImage;
            Presentation.Background = _thumbImage;
        }

        private void SetPositionAndImageCropped(Point pos)
        {

            var thumbSize = ThumbSize / 2d;
            _x1 = (int)Math.Clamp(pos.X - thumbSize, 0, ImageWidth);
            _y1 = (int)Math.Clamp(pos.Y - thumbSize, 0, ImageHeight);

            //
            // 检查边界
            if (_x < 0 || _x + thumbSize > ImageWidth)
            {
                _x1 = _x1 <= 0 ? 0 : (int)(ImageWidth - ThumbSize);
                Canvas.SetLeft(Thumb, _x1);
            }

            if (_y < 0 || _y + thumbSize > ImageHeight)
            {
                _y1 = _y1 <= 0 ? 0 : (int)(ImageHeight - ThumbSize);
            }

            Canvas.SetLeft(Thumb, _x1);
            Canvas.SetTop(Thumb, _y1);
            ViewModel.PosX = _x1;
            ViewModel.PosY = _y1;

            _cropped                = new CroppedBitmap(_grayImage, new Int32Rect(_x1, _y1, (int)ThumbSize, (int)ThumbSize));
            _thumbImage.ImageSource = _cropped;
            Thumb.Background        = _thumbImage;
            Presentation.Background = _thumbImage;
        }

        private void Button_ScaleUp(object sender, RoutedEventArgs e)
        {
            var newThumbSize = _thumbOriginalSize * ((Scale + ScaleFactor) / 100d);
            if (newThumbSize > _minImageSize)
            {
                return;
            }

            ScaleDown.IsEnabled =  true; 
            Scale               += ScaleFactor;
            ScaleRate.Value     =  Scale;
            ThumbSize           =  _thumbOriginalSize * Scale / 100d;
            Thumb.Width         =  ThumbSize;
            Thumb.Height        =  ThumbSize;

            SetPositionAndImageCropped(new Point(_x, _y));
            //
            // enabled
            ScaleUp.IsEnabled = !(_thumbOriginalSize * ((Scale + ScaleFactor) / 100d) > _minImageSize);
        }

        private void Button_ScaleDown(object sender, RoutedEventArgs e)
        {
            var newThumbSize = _thumbOriginalSize * ((Scale - ScaleFactor) / 100d);
            if (newThumbSize < 100)
            {
                return;
            }

            ScaleUp.IsEnabled =  true;
            Scale             -= ScaleFactor;
            ScaleRate.Value   =  Scale;
            ThumbSize         =  _thumbOriginalSize * Scale / 100d;
            Thumb.Width       =  ThumbSize;
            Thumb.Height      =  ThumbSize;

            SetPositionAndImageCropped(new Point(_x, _y));

            //
            // enabled
            ScaleDown.IsEnabled = !(_thumbOriginalSize * ((Scale - ScaleFactor) / 100d) < 100);
        }

        private void ScaleRate_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var minSize      = Math.Min(100, _minImageSize);

            Scale               = ScaleRate.Value;
            ScaleUp.IsEnabled = _thumbOriginalSize * ((Scale + ScaleFactor) / 100d) <= _minImageSize;
            ScaleDown.IsEnabled   = minSize < _thumbOriginalSize * ((Scale - ScaleFactor) / 100d) ;
            ThumbSize           = _thumbOriginalSize * Scale / 100d;
            Thumb.Width         = ThumbSize;
            Thumb.Height        = ThumbSize;

            SetPositionAndImageCropped(new Point(_x, _y));
        }

        public double Scale
        {
            get => ViewModel.Scale;
            set => ViewModel.Scale = value;
        }

        public double ImageWidth
        {
            get => ViewModel.ImageWidth;
            set => ViewModel.ImageWidth = value;
        }

        public double ImageHeight
        {
            get => ViewModel.ImageHeight;
            set => ViewModel.ImageHeight = value;
        }

        public double ThumbSize
        {
            get => ViewModel.ThumbSize;
            set => ViewModel.ThumbSize = value;
        }
        
        public ImageEditViewModel ViewModel => DataContext as ImageEditViewModel;
    }
}