using System.IO;
using System.Linq;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using IDialogService = Acorisoft.FutureGL.Forest.Interfaces.IDialogService;

// ReSharper disable MethodHasAsyncOverload
// ReSharper disable ConvertToUsingDeclaration

namespace Acorisoft.FutureGL.MigaStudio.Utilities
{
    public class ImageOpResult
    {
        public bool IsFinished { get; init; }
        public string FileName { get; init; }
        public MemoryStream Buffer { get; init; }
    }


    public static partial class ImageUtilities
    {

        #region Avatar
        
        
        private static MemoryStream SaveToPNG(Image<Rgba32> image, int length)
        { //
            // 
            var origin = new MemoryStream(length);
            image.SaveAsPng(origin);
            origin.Seek(0, SeekOrigin.Begin);

            return origin;
        }

        private static MemoryStream ResizeTo1080P(int w, int h,out int w1, out int h1, bool horizontal, Image<Rgba32> image, int length)
        {
            //
            // save image to png
            var scale = horizontal ? 1920d / w : 1080d / h;
            h  = (int)(h * scale);
            w  = (int)(w * scale);
            h1 = h;
            w1 = w;
            image.Mutate(x => { x.Resize(new Size(w, h)); });
            
            return SaveToPNG(image, length);
        }
        
        private static MemoryStream ResizeToSquare(int w, int h, Image<Rgba32> image, int length)
        {
            var scale = 256d / image.Width;
            image.Mutate(x => { x.Resize(new Size((int)(w * scale), (int)(h * scale))); });
            
            return SaveToPNG(image, length);
        }

        public static async Task<ImageOpResult> Avatar()
        {
            var opendlg = Studio.Open(SubSystemString.ImageFilter);

            if (opendlg.ShowDialog() != true)
            {
                return new ImageOpResult { IsFinished = false };
            }

            Configuration.Default
                         .MemoryAllocator
                         .ReleaseRetainedResources();
            MemoryStream result;
            var          fileName = opendlg.FileName;
            var          session  = Xaml.Get<IBusyService>().CreateSession();
            var          buffer   = await File.ReadAllBytesAsync(fileName);
            var          origin   = new MemoryStream(buffer);
            var          image    = Image.Load<Rgba32>(buffer);
            var          h        = image.Height;
            var          w        = image.Width;
            var          h1       = 0;
            var          w1       = 0;


            if (w < 32 || h < 32)
            {
                image.Dispose();
                origin.Dispose();
                await Xaml.Get<IBuiltinDialogService>()
                          .Notify(CriticalLevel.Danger, SubSystemString.Notify, SubSystemString.ImageTooSmall);;
                return new ImageOpResult { IsFinished = false };
            }

            if (w > 4096 ||
                h > 4096)
            {
                await Xaml.Get<IBuiltinDialogService>()
                          .Notify(CriticalLevel.Danger, SubSystemString.Notify, SubSystemString.ImageTooBig);

                image.Dispose();
                origin.Dispose();
                return new ImageOpResult { IsFinished = false };
            }


            if (w != h)
            {
                var horizontal = w > 640;

                if (horizontal || h > 480)
                {
                    await Task.Run(async () =>
                    {
                        session.Update(SubSystemString.Processing);
                        await session.Await();
                        
                        //
                        // resize
                        origin = ResizeTo1080P(
                            w, 
                            h,
                            out w1,
                            out h1,
                            horizontal, 
                            image, 
                            buffer.Length);
                        session.Dispose();
                    });
                }

                var r = await Xaml.Get<IDialogService>()
                                  .Dialog<MemoryStream, ImageEditViewModel>(new Parameter
                                  {
                                      Args = new object[]
                                      {
                                          image,
                                          origin
                                      }
                                  });

                if (!r.IsFinished)
                {
                    origin.Dispose();
                    return new ImageOpResult { IsFinished = false };
                }

                result = r.Value;
                result.Seek(0, SeekOrigin.Begin);
                origin.Dispose();
                image.Dispose();
                return new ImageOpResult
                {
                    IsFinished = true,
                    Buffer     = result,
                    FileName   = opendlg.FileName
                };
            }

            if (image.Width > 256 ||
                image.Width > 256)
            {
                await Task.Run(async () =>
                {
                    session.Update(SubSystemString.Processing);
                    await session.Await();
                    origin = ResizeToSquare(w, h, image, buffer.Length);
                    session.Dispose();
                });

                return new ImageOpResult
                {
                    IsFinished = true,
                    Buffer     = origin,
                    FileName   = opendlg.FileName
                };
            }


            origin.Dispose();
            result = SaveToPNG(image, buffer.Length);
            session.Dispose();
            image.Dispose();
            return new ImageOpResult
            {
                IsFinished = true,
                Buffer     = result,
                FileName   = opendlg.FileName
            };
        }


        public static async Task Avatar(ImageEngine engine, Action<string> callback)
        {
            var r = await Avatar();

            if (!r.IsFinished)
            {
                return;
            }

            if (!r.IsFinished)
            {
                return;
            }

            var    buffer = r.Buffer;
            var    raw    = await Pool.MD5.ComputeHashAsync(buffer);
            var    md5    = Convert.ToBase64String(raw);
            string avatar;

            if (engine.HasFile(md5))
            {
                var fr = engine.Records
                               .FindById(md5);
                avatar = fr.Uri;
            }
            else
            {
                avatar = GetAvatarName();
                buffer.Seek(0, SeekOrigin.Begin);
                engine.WriteAvatar(buffer, avatar);

                var record = new FileRecord
                {
                    Id   = md5,
                    Uri  = avatar,
                    Width = 256,
                    Height = 256,
                    Type = ResourceType.Image
                };

                engine.AddFile(record);
            }
            callback?.Invoke(avatar);
        }
        #endregion
    }
}