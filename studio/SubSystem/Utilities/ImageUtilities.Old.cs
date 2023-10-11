using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using IDialogService = Acorisoft.FutureGL.Forest.Interfaces.IDialogService;


namespace Acorisoft.FutureGL.MigaStudio.Utilities
{
    [SuppressMessage("ReSharper", "MethodHasAsyncOverload")]
    static partial class ImageUtilities
    {
        public const char   Separator                = '+';
        public const string AvatarPattern            = "avatar+{0}.png";
        public const string ThumbnailPattern         = "thumb+{0}.png";
        public const string ThumbnailWithSizePattern = "{0}:{1}:{2}";
        public const string ThumbnailPrefixPattern   = "Images\\thumb+";
        public const string RawPattern               = "{0}.png";


        public static string GetAvatarName() => string.Format(AvatarPattern, ID.Get());


        private static bool ConvertAlbumFromUri(string uri, out Album album)
        {
            var sources = uri.Split("_");

            if (sources is null ||
                sources.Length < 3)
            {
                album = null;
                return false;
            }

            album = new Album
            {
                Id     = uri,
                Source = string.Format(ThumbnailPattern, sources[0]),
                Width  = int.Parse(sources[1]),
                Height = int.Parse(sources[2])
            };
            return true;
        }

        public static async Task CropAllAvatar()
        {
            Configuration.Default
                         .MemoryAllocator
                         .ReleaseRetainedResources();
            var dir = Studio.DatabaseManager()
                            .GetEngine<ImageEngine>()
                            .FullDirectory;

            var images = Directory.GetFiles(dir)
                                  .Where(x => x.Contains("avatar"))
                                  .ToArray();


            Image<Rgba32> img;

            await Task.Run(async () =>
            {
                var session = Xaml.Get<IBusyService>().CreateSession();
                session.Update(SubSystemString.Processing);

                foreach (var image in images)
                {
                    img = Image.Load<Rgba32>(image);
                    var w = img.Width;
                    var h = img.Height;

                    if (w > 256 && w == h)
                    {
                        var scale = 256d / w;
                        img.Mutate(x => { x.Resize(new Size((int)(w * scale), (int)(h * scale))); });
                        await img.SaveAsPngAsync(image);
                    }
                }

                session.Dispose();
            });
        }

        public static async Task<Op<Album>> Thumbnail(ImageEngine engine, string fileName)
        {
            Configuration.Default
                         .MemoryAllocator
                         .ReleaseRetainedResources();
            var    buffer = await File.ReadAllBytesAsync(fileName);
            byte[] thumbnailBuffer;
            var    raw   = Pool.MD5.ComputeHash(buffer);
            var    md5   = Convert.ToBase64String(raw);
            var    image = Image.Load<Rgba32>(buffer);
            var    id    = ID.Get();

            if (engine.HasFile(md5))
            {
                var fr = engine.Records
                               .FindById(md5);
                var target = Path.Combine(engine.SourceDirectory, string.Format(ThumbnailPattern, id));
                if (!File.Exists(target))
                {
                    File.Copy(fileName, target, true);
                }

                image.Dispose();
                return Op<Album>.Success(new Album
                {
                    Id     = fr.Uri,
                    Width  = Math.Clamp(fr.Width, 0, 1920),
                    Height = Math.Clamp(fr.Height, 0, 1920),
                    Source = fr.Uri
                });
            }

            if (image.Width < 32 || image.Height < 32)
            {
                return Op<Album>.Failed("图片过小");
            }


            var w          = image.Width;
            var h          = image.Height;
            var h1         = h;
            var w1         = w;
            var horizontal = w > 1920;

            if (horizontal || h > 1080)
            {
                var ms = ResizeTo1080P(
                    w, 
                    h,
                    out w1,
                    out h1,
                    horizontal, 
                    image, 
                    buffer.Length);
                thumbnailBuffer = ms.GetBuffer();
            }
            else
            {
                thumbnailBuffer = new byte[buffer.Length];
                Array.Copy(buffer, thumbnailBuffer, buffer.Length);
            }

            var thumbnail = string.Format(ThumbnailPattern, id);
            engine.AddFile(new FileRecord
            {
                Id   = md5,
                Uri  = string.Format(ThumbnailPattern, id),
                Width = w1,
                Height = h1,
                Type = ResourceType.Image
            });

            //
            // 复制源文件到
            var dst = Path.Combine(engine.SourceDirectory, thumbnail);
            if (!File.Exists(dst))
            {
                File.Copy(fileName, dst, true);
            }

            //
            //
            engine.Write(thumbnail, thumbnailBuffer);
            image.Dispose();
            return Op<Album>.Success(new Album
            {
                Id     = thumbnail,
                Source = thumbnail,
                Width  = w1,
                Height = h1,
            });
        }

        public enum ImageScale
        {
            Square,
            Horizontal,
            Vertical
        }

        private static double GetScaleRate(ImageScale scale, int w, int h)
        {
            if (scale == ImageScale.Square)
            {
                return w > 640 ? 640d / w : 1d;
            }

            if (scale == ImageScale.Horizontal)
            {
                return w > 1920 ? 1920d / w : 1d;
            }
            
            return h > 1080 ? 1080d / h : 1d;
        }

        public static async Task<Op<string>> Raw(ImageEngine engine, string fileName, ImageScale scale, Action<Album> callback)
        {
            Configuration.Default
                         .MemoryAllocator
                         .ReleaseRetainedResources();
            var    buffer = await File.ReadAllBytesAsync(fileName);
            var    raw    = Pool.MD5.ComputeHash(buffer);
            var    md5    = Convert.ToBase64String(raw);
            var    image  = Image.Load<Rgba32>(buffer);


            var w = image.Width;
            var h = image.Height;

            if ((scale == ImageScale.Horizontal && w <= h) ||
                (scale == ImageScale.Vertical && w >= h) ||
                (scale == ImageScale.Square && w != h))
            {
                return Op<string>.Failed("比例不对");
            }

            var scale1 = GetScaleRate(scale, w, h);

            var h1     = (int)(h * scale1);
            var w1     = (int)(w * scale1);

            if (engine.HasFile(md5))
            {
                var fr = engine.Records
                               .FindById(md5);

                callback?.Invoke(new Album
                {
                    Id     = fr.Id,
                    Source = fr.Uri,
                    Width  = Math.Clamp(fr.Width, 0, 1920),
                    Height = Math.Clamp(fr.Height, 0, 1080),
                });
                image.Dispose();
                return Op<string>.Success(string.Format(ThumbnailWithSizePattern, fr.Uri, fr.Width, fr.Height));
            }

            if (image.Width < 32 || image.Height < 32)
            {
                return Op<string>.Failed("图片过小");
            }

            var id        = ID.Get();
            var thumbnail = string.Format(RawPattern, id);
            var withSize  = string.Format(ThumbnailWithSizePattern, thumbnail, w1, h1);
            engine.AddFile(new FileRecord
            {
                Id     = md5,
                Uri    = thumbnail,
                Width  = w,
                Height = h,
                Type   = ResourceType.Image
            });

            engine.Write(thumbnail, buffer);
            callback?.Invoke(new Album
            {
                Id     = thumbnail,
                Source = thumbnail,
                Width  = w1,
                Height = h1
            });
            image.Dispose();
            return Op<string>.Success(withSize);
        }
    }
}