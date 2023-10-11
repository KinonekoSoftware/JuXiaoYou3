using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Threading;
using System.Windows.Media.Imaging;
using Color = SixLabors.ImageSharp.Color;

namespace Acorisoft.FutureGL.MigaStudio.Utilities
{
    public class CloudMap
    {
    }

    /// <summary>
    /// Class to draw word clouds.
    /// </summary>
    public class WordCloud : IDisposable
    {
        public event Action<double> OnProgress;
        public event Action<Image> OnStepDrawResultImg;

        public event Action<Image> OnStepDrawIntegralImg;

        private AutoResetEvent DrawWaitHandle;

        public bool StepDrawMode { get; set; }


        /// <summary>
        /// Gets font colour or random if font wasn't set
        /// </summary>
        private Color FontColor
        {
            get { return this._mFontColor ?? GetRandomColor(); }
            set { this._mFontColor = value; }
        }


        private Color? _mFontColor;


        /// <summary>
        /// Used to select random colors.
        /// </summary>
        private Random Random { get; set; }


        /// <summary>
        /// Working image.
        /// </summary>
        private FastImage WorkImage { get; set; }


        /// <summary>
        /// Keeps track of word positions using integral image.
        /// </summary>
        private OccupancyMap Map { get; set; }


        /// <summary>
        /// Gets or sets the maximum size of the font.
        /// </summary>
        private float MaxFontSize { get; set; }


        /// <summary>
        /// User input order instead of frequency
        /// </summary>
        private bool UseRank { get; set; }

        /// <summary>
        /// Amount to decrement font size each time a word won't fit.
        /// </summary>
        private int FontStep { get; set; }

        /// <summary>
        /// If allow vertical drawing 
        /// </summary>
        private bool AllowVertical { get; set; }

        private string Fontname
        {
            get { return this._fontname ?? "Microsoft Sans Serif"; }
            set
            {
                this._fontname = value;
                if (value == null) return;
                using (var f = new Font(value, 12, FontStyle.Regular))
                {
                    this._fontname = f.FontFamily.Name;
                }
            }
        }

        private string _fontname;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="WordCloud"/> class.
        /// </summary>
        /// <param name="width">The width of word cloud.</param>
        /// <param name="height">The height of word cloud.</param>
        /// <param name="useRank">if set to <c>true</c> will ignore frequencies for best fit.</param>
        /// <param name="fontColor">Color of the font.</param>
        /// <param name="maxFontSize">Maximum size of the font.</param>
        /// <param name="fontStep">The font step to use.</param>
        /// <param name="mask">mask image</param>
        /// <param name="allowVerical">allow vertical text</param>
        public WordCloud(int width, int height, bool useRank = false, Color? fontColor = null, float maxFontSize = -1, int fontStep = 1, Image mask = null, bool allowVerical = false, string fontname = null)
        {
            if (mask == null)
            {
                this.Map       = new OccupancyMap(width, height);
                this.WorkImage = new FastImage(width, height, PixelFormat.Format32bppArgb);
            }
            else
            {
                mask = Util.ResizeImage(mask, width, height);
                if (!Util.CheckMaskValid(mask))
                    throw new Exception("Mask is not a valid black-white image");
                this.Map       = new OccupancyMap(mask);
                this.WorkImage = new FastImage(mask);
            }

            this.MaxFontSize   = maxFontSize < 0 ? (float)height : maxFontSize;
            this.FontStep      = fontStep;
            this._mFontColor   = fontColor;
            this.UseRank       = useRank;
            this.Random        = new Random(Environment.TickCount);
            this.AllowVertical = allowVerical;
            this.Fontname      = fontname;
#if DEBUG
            this.DrawWaitHandle = new AutoResetEvent(false);
            this.StepDrawMode   = false;
#endif
        }

        /// <summary>
        /// Gets a random color.
        /// </summary>
        /// <returns>Color</returns>
        private Color GetRandomColor()
        {
            return Color.FromArgb(this.Random.Next(0, 255), this.Random.Next(0, 255), this.Random.Next(0, 255));
        }

        /// <summary>
        /// Draws the specified word cloud given list of words and frequecies
        /// </summary>
        /// <param name="words">List of words ordered by occurance.</param>
        /// <param name="freqs">List of frequecies.</param>
        /// <param name="bgcolor">Backgroud color of the output image</param>
        /// <param name="img">Backgroud image of the output image</param>
        /// <returns>Image of word cloud.</returns>
        /// <exception cref="System.ArgumentException">
        /// Arguments null.
        /// or
        /// Must have the same number of words as frequencies.
        /// </exception>
        private Image Draw(IList<string> words, IList<int> freqs, Color bgcolor, Image img)
        {
#if DEBUG
            ShowIntegralImgStepDraw(this.Map.IntegralImageToBitmap());
            this.DrawWaitHandle.Reset();
#endif
            var fontSize = this.MaxFontSize;
            if (words == null || freqs == null)
            {
                throw new ArgumentException("Arguments null.");
            }

            if (words.Count != freqs.Count)
            {
                throw new ArgumentException("Must have the same number of words as frequencies.");
            }

            Bitmap result;
            if (img == null)
                result = new Bitmap(this.WorkImage.Width, this.WorkImage.Height);
            else
            {
                if (img.Size != this.WorkImage.Bitmap.Size)
                    throw new Exception("The backgroud img should be with the same size with the mask");
                result = new Bitmap(img);
            }

            using (var gworking = Graphics.FromImage(this.WorkImage.Bitmap))
                using (var gresult = Graphics.FromImage(result))
                {
                    if (img == null)
                        gresult.Clear(bgcolor);
                    gresult.TextRenderingHint  = TextRenderingHint.AntiAlias;
                    gworking.TextRenderingHint = TextRenderingHint.AntiAlias;
                    var lastProgress = 0.0d;
                    for (var i = 0; i < words.Count; ++i)
                    {
                        var progress = (double)i / words.Count;
                        if (progress - lastProgress > 0.01)
                        {
                            ShowProgress(progress);
                            lastProgress = progress;
                        }

                        if (!this.UseRank)
                        {
                            fontSize = (float)Math.Min(fontSize, 100 * Math.Log10(freqs[i] + 100));
                        }

                        var format = new StringFormat();
                        if (this.AllowVertical)
                        {
                            if (this.Random.Next(0, 2) == 1)
                                format.FormatFlags = StringFormatFlags.DirectionVertical;
                        }

                        Point p;
                        var   foundPosition = false;
                        Font  font;
                        var   size = new SizeF();
                        Debug.WriteLine("Word: " + words[i]);
                        do
                        {
                            font = new Font(this.Fontname, fontSize, GraphicsUnit.Pixel);
                            size = gworking.MeasureString(words[i], font, new PointF(0, 0), format);
                            Debug.WriteLine("Search with font size: " + fontSize);
                            foundPosition = this.Map.GetRandomUnoccupiedPosition((int)size.Width, (int)size.Height, out p);
                            if (!foundPosition) fontSize -= this.FontStep;
                        } while (fontSize > 0 && !foundPosition);

                        Debug.WriteLine("Found pos: " + p);
                        if (fontSize <= 0) break;
                        gworking.DrawString(words[i], font, new SolidBrush(this.FontColor), p.X, p.Y, format);
                        gresult.DrawString(words[i], font, new SolidBrush(this.FontColor), p.X, p.Y, format);
                        this.Map.Update(this.WorkImage, p.X, p.Y);
#if DEBUG
                        if (this.StepDrawMode)
                        {
                            ShowResultStepDraw(new Bitmap(this.WorkImage.Bitmap));
                            ShowIntegralImgStepDraw(this.Map.IntegralImageToBitmap());
                            this.DrawWaitHandle.WaitOne();
                        }
#endif
                    }
                }

            this.WorkImage.Dispose();
            return result;
        }

        /// <summary>
        /// Draws the specified word cloud with background color spicified given list of words and frequecies
        /// </summary>
        /// <param name="words">List of words ordered by occurance.</param>
        /// <param name="freqs">List of frequecies.</param>
        /// <param name="bgcolor">Specified background color</param>
        /// <returns>Image of word cloud.</returns>
        public Image Draw(IList<string> words, IList<int> freqs, Color bgcolor)
        {
            return Draw(words, freqs, bgcolor, null);
        }

        /// <summary>
        /// Draws the specified word cloud with background spicified given list of words and frequecies
        /// </summary>
        /// <param name="words">List of words ordered by occurance.</param>
        /// <param name="freqs">List of frequecies.</param>
        /// <param name="img">Specified background image</param>
        /// <returns>Image of word cloud.</returns>
        public Image Draw(IList<string> words, IList<int> freqs, Image img)
        {
            return Draw(words, freqs, Color.White, img);
        }

        /// <summary>
        /// Draws the specified word cloud with given list of words and frequecies
        /// </summary>
        /// <param name="words">List of words ordered by occurance.</param>
        /// <param name="freqs">List of frequecies.</param>
        /// <returns>Image of word cloud.</returns>
        public Image Draw(IList<string> words, IList<int> freqs)
        {
            return Draw(words, freqs, Color.White, null);
        }

        private void ShowProgress(double progress)
        {
            OnProgress?.Invoke(progress);
        }
#if DEBUG
        private void ShowResultStepDraw(Image bmp)
        {
            OnStepDrawResultImg?.Invoke(bmp);
        }

        private void ShowIntegralImgStepDraw(Image bmp)
        {
            OnStepDrawIntegralImg?.Invoke(bmp);
        }

        public void ContinueDrawing()
        {
            this.DrawWaitHandle.Set();
        }
#endif
        public void Dispose()
        {
#if DEBUG
            DrawWaitHandle?.Dispose();
#endif
            WorkImage?.Dispose();
        }
    }

    internal class Util
    {
        internal static FastImage CropImage(FastImage img)
        {
            var cropRect = new Rectangle(1, 1, img.Width - 1, img.Height - 1);
            var src      = img.Bitmap;
            var target   = new Bitmap(cropRect.Width, cropRect.Height);

            using (var g = Graphics.FromImage(target))
            {
                g.DrawImage(src, new Rectangle(0, 0, target.Width, target.Height),
                    cropRect,
                    GraphicsUnit.Pixel);
            }

            return new FastImage(target);
        }

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        internal static Image ResizeImage(Image image, int width, int height)
        {
            if (image.Width == width && image.Height == height)
                return image;
            var destRect  = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height, PixelFormat.Format32bppArgb);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode    = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode  = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode      = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode    = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            var bmpdata = destImage.LockBits(new Rectangle(0, 0, destImage.Width, destImage.Height), ImageLockMode.ReadOnly, destImage.PixelFormat);
            var len     = bmpdata.Height * bmpdata.Stride;
            var buf     = new byte[len];
            Marshal.Copy(bmpdata.Scan0, buf, 0, len);
            for (var i = 0; i < width * height * 4; i++)
            {
                if (buf[i] == 255 || buf[i] == 0)
                    continue;
                if (i % 4 == 3)
                    continue;
                if (buf[i] > 0)
                    buf[i] = 255;
                else
                    buf[i] = 0;
            }

            Marshal.Copy(buf, 0, bmpdata.Scan0, len);
            destImage.UnlockBits(bmpdata);

            return destImage;
        }

        internal static bool CheckMaskValid(Image mask)
        {
            bool valid;
            using (var bmp = new Bitmap(mask))
            {
                var bmpdata = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, bmp.PixelFormat);
                var len     = bmpdata.Height * bmpdata.Stride;
                var buf     = new byte[len];
                Marshal.Copy(bmpdata.Scan0, buf, 0, len);
                valid = buf.Count(b => b != 0 && b != 255) == 0;
                bmp.UnlockBits(bmpdata);
            }

            return valid;
        }
    }

    internal class OccupancyMap : IntegralImage
    {
        private Random Rand { get; set; }

        public OccupancyMap(int outputImgWidth, int outputImgHeight) : base(outputImgWidth, outputImgHeight)
        {
            this.Rand = new Random(Environment.TickCount);
        }

        public OccupancyMap(Image mask) : base(new FastImage(mask))
        {
            this.Rand = new Random(Environment.TickCount);
        }

        /// <summary>
        /// get the first availible postion from occupancy map
        /// </summary>
        /// <param name="strSizeX"></param>
        /// <param name="strSizeY"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        /// <remarks>Obsoleted because this methods not brief enough</remarks>
        [Obsolete("TryFindUnoccupiedPosition is deprecated, please use GetRandomUnoccupiedPosition instead.")]
        public bool TryFindUnoccupiedPosition(int strSizeX, int strSizeY, out Point p)
        {
            var startPosX = this.Rand.Next(1, this.OutputImgWidth);
            var startPosY = this.Rand.Next(1, this.OutputImgHeight);

            int x, y, dx, dy;
            x  = y = dx = 0;
            dy = -1;
            var width  = this.OutputImgWidth - strSizeX;
            var height = this.OutputImgHeight - strSizeY;

            var t    = Math.Max(width, height);
            var maxI = t * t;

            for (var i = 0; i < maxI; i++)
            {
                if ((-width / 2 <= x) && (x <= width / 2) && (-height / 2 <= y) && (y <= height / 2))
                {
                    var posX = x + startPosX;
                    var posY = y + startPosY;
                    if (posY > 0 && posY < this.OutputImgHeight - strSizeY && posX > 0 && posX < this.OutputImgWidth - strSizeX)
                    {
                        if (GetArea(posX, posY, strSizeX, strSizeY) == 0)
                        {
                            p = new Point(posX, posY);
                            return true;
                        }
                    }
                }

                if ((x == y) || ((x < 0) && (x == -y)) || ((x > 0) && (x == 1 - y)))
                {
                    t  = dx;
                    dx = -dy;
                    dy = t;
                }

                x += dx;
                y += dy;
            }

            p = new Point();
            return false;
        }

        /// <summary>
        /// get the a random postion from all availible positions from occupancy map
        /// </summary>
        /// <param name="strSizeX"></param>
        /// <param name="strSizeY"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        /// <remarks>Obsoleted because this methods not brief enough</remarks>
        [Obsolete("TryFindRandomUnoccupiedPosition is deprecated, please use GetRandomUnoccupiedPosition instead.")]
        public bool TryFindRandomUnoccupiedPosition(int strSizeX, int strSizeY, out Point p)
        {
            var startPosX = this.Rand.Next(1, this.OutputImgWidth);
            var startPosY = this.Rand.Next(1, this.OutputImgHeight);

            int x, y, dx, dy;
            x  = y = dx = 0;
            dy = -1;
            var width  = this.OutputImgWidth - strSizeX;
            var height = this.OutputImgHeight - strSizeY;

            var t    = Math.Max(width, height);
            var maxI = t * t;

            var posList = new List<Point>();

            for (var i = 0; i < maxI; i++)
            {
                if ((-width / 2 <= x) && (x <= width / 2) && (-height / 2 <= y) && (y <= height / 2))
                {
                    var posX = x + startPosX;
                    var posY = y + startPosY;
                    if (posY > 0 && posY < this.OutputImgHeight - strSizeY && posX > 0 && posX < this.OutputImgWidth - strSizeX)
                    {
                        if (GetArea(posX, posY, strSizeX, strSizeY) == 0)
                        {
                            posList.Add(new Point(posX, posY));
                        }
                    }
                }

                if ((x == y) || ((x < 0) && (x == -y)) || ((x > 0) && (x == 1 - y)))
                {
                    t  = dx;
                    dx = -dy;
                    dy = t;
                }

                x += dx;
                y += dy;
            }

            if (posList.Count == 0)
            {
                p = new Point();
                return false;
            }
            else
            {
                p = posList[this.Rand.Next(0, posList.Count - 1)];
                return true;
            }
        }

        public bool GetRandomUnoccupiedPosition(int strSizeX, int strSizeY, out Point p)
        {
            var posList = new List<Point>();
            for (var i = 1; i < this.OutputImgWidth - strSizeX; i++)
            {
                for (var j = 1; j < this.OutputImgHeight - strSizeY; j++)
                {
                    if (GetArea(i, j, strSizeX, strSizeY) == 0)
                    {
                        posList.Add(new Point(i, j));
                    }
                }
            }

            if (posList.Count == 0)
            {
                p = new Point();
                return false;
            }
            else
            {
                var randomIndex = this.Rand.Next(0, posList.Count - 1);
                p = posList[randomIndex];
                return true;
            }
        }
    }

    internal class IntegralImage
    {
        #region attributes & constructors

        public int OutputImgWidth { get; set; }

        public int OutputImgHeight { get; set; }

        protected uint[,] Integral { get; set; }

        public IntegralImage(int outputImgWidth, int outputImgHeight)
        {
            this.Integral        = new uint[outputImgWidth, outputImgHeight];
            this.OutputImgWidth  = outputImgWidth;
            this.OutputImgHeight = outputImgHeight;
        }

        public IntegralImage(FastImage image)
        {
            this.Integral        = new uint[image.Width, image.Height];
            this.OutputImgWidth  = image.Width;
            this.OutputImgHeight = image.Height;
            InitMask(image);
        }

        #endregion

        private void InitMask(FastImage image)
        {
            Update(Util.CropImage(image), 1, 1);
        }

        public void Update(FastImage image, int posX, int posY)
        {
            if (posX < 1) posX = 1;
            if (posY < 1) posY = 1;
            var pixelSize      = Math.Min(3, image.PixelFormatSize);

            for (var i = posY; i < image.Height; ++i)
            {
                for (var j = posX; j < image.Width; ++j)
                {
                    byte pixel = 0;
                    for (var p = 0; p < pixelSize; ++p)
                    {
                        pixel |= image.Data[i * image.Stride + j * image.PixelFormatSize + p];
                    }

                    this.Integral[j, i] = pixel + this.Integral[j - 1, i] + this.Integral[j, i - 1] - this.Integral[j - 1, i - 1];
                }
            }
        }

        public ulong GetArea(int xPos, int yPos, int sizeX, int sizeY)
        {
            ulong area = this.Integral[xPos, yPos] + this.Integral[xPos + sizeX, yPos + sizeY];
            area -= this.Integral[xPos + sizeX, yPos] + this.Integral[xPos, yPos + sizeY];
            return area;
        }

#if DEBUG

        public Bitmap IntegralImageToBitmap()
        {
            var bmp = new Bitmap(this.OutputImgWidth, this.OutputImgHeight, PixelFormat.Format8bppIndexed);
            var bmpdata = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
                ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);
            var len      = bmpdata.Stride * bmp.Height;
            var buffer   = new byte[len];
            var bufIndex = 0;
            for (var i = 0; i < this.OutputImgHeight; ++i)
            {
                for (var j = 0; j < this.OutputImgWidth; ++j)
                {
                    if (i == 0 || j == 0)
                    {
                        buffer[bufIndex++] = 255;
                        continue;
                    }

                    if (this.Integral[j, i] - this.Integral[j - 1, i] - this.Integral[j, i - 1] + this.Integral[j - 1, i - 1] == 0)
                        buffer[bufIndex++] = 255;
                    else
                        buffer[bufIndex++] = 0;
                }
            }

            Marshal.Copy(buffer, 0, bmpdata.Scan0, buffer.Length);
            bmp.UnlockBits(bmpdata);
            return bmp;
        }

        public void SaveIntegralImageToBitmap(string filename = "")
        {
            if (filename == "")
                IntegralImageToBitmap().Save(Path.GetRandomFileName() + ".bmp");
            else
                IntegralImageToBitmap().Save(filename + DateTime.Now.ToString("_hhmmss_fff") + ".bmp");
        }


        public static string GetSha(byte[] array)
        {
            return Convert.ToBase64String(Pool.SHA1.ComputeHash(array));
        }

#endif
    }

    /// <summary>
    /// Image class for fast manipulation of bitmap data.
    /// </summary>
    /// 
    public class FastImage : IDisposable
    {
        private readonly WriteableBitmap _Bitmap;

        public FastImage(int width, int height, PixelFormat format)
        {
            _Bitmap              = new WriteableBitmap();
            this.PixelFormatSize = Image.GetPixelFormatSize(format) / 8;
            this.Stride          = width * this.PixelFormatSize;

            this.Data   = new byte[this.Stride * height];
            this.Handle = GCHandle.Alloc(this.Data, GCHandleType.Pinned);
            var pData = Marshal.UnsafeAddrOfPinnedArrayElement(this.Data, 0);
            this.Bitmap = new Bitmap(width, height, this.Stride, format, pData);
        }

        public FastImage(Image img) : this(img.Width, img.Height, img.PixelFormat)
        {
            var bmp       = new Bitmap(img);
            var bmpdatain = bmp.LockBits(new Rectangle(0, 0, this.Bitmap.Width, this.Bitmap.Height), ImageLockMode.ReadOnly, this.Bitmap.PixelFormat);
            Marshal.Copy(bmpdatain.Scan0, this.Data, 0, this.Data.Length);
            bmp.UnlockBits(bmpdatain);
        }

        public int Width => this.Bitmap.Width;

        public int Height => this.Bitmap.Height;

        public int PixelFormatSize { get; set; }

        public GCHandle Handle { get; set; }

        public int Stride { get; set; }

        public byte[] Data { get; set; }

        public Bitmap Bitmap { get; set; }

        public void Dispose()
        {
            this.Handle.Free();
            this.Bitmap.Dispose();
        }
    }
}