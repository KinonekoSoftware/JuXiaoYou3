using System.IO;
using System.Linq;
using System.Windows.Media;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.MigaUtils;
using Acorisoft.FutureGL.MigaUtils.Collections;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Tools
{
    public class FileState : ObservableObject
    {
        private string _fileName;
        private bool   _isError;
        private bool _processFailed;
        
        public Image<Rgba32> Image { get; set; }

        /// <summary>
        /// 获取或设置 <see cref="ProcessFailed"/> 属性。
        /// </summary>
        public bool ProcessFailed
        {
            get => _processFailed;
            set => SetValue(ref _processFailed, value);
        }
        /// <summary>
        /// 获取或设置 <see cref="IsError"/> 属性。
        /// </summary>
        public bool IsError
        {
            get => _isError;
            set => SetValue(ref _isError, value);
        }
        
        /// <summary>
        /// 获取或设置 <see cref="FileName"/> 属性。
        /// </summary>
        public string FileName
        {
            get => _fileName;
            set => SetValue(ref _fileName, value);
        }
    }
    
    public class SentenceImageCombineViewModel : TabViewModel
    {
        private bool         _isPreview;
        private MemoryStream _ms;
        private ImageSource  _preview;
        private int          _yOffset;
        private int          _height;

        private async Task CreatePreviewImageImpl()
        {
            if (FileCollection.Count == 0)
            {
                return;
            }

            using (var session = Xaml.Get<IBusyService>()
                                     .CreateSession())
            {
                try
                {
                    session.Update(SubSystemString.Processing);
                    await session.Await();

                    FileCollection.ForEach(x => x.IsError = !File.Exists(x.FileName));

                    //
                    var availableImageFile = FileCollection.Where(x => !x.IsError)
                                                           .ToArray();

                    if (availableImageFile.Length == 0)
                    {
                        session.Update(Language.GetText("text.EmptyData"));
                        await session.Await();
                        return;
                    }

                    //
                    // 加载图片
                    session.Update(SubSystemString.LoadingImage);
                    await session.Await();
                    availableImageFile.ForEach(x => x.Image ??= Image.Load<Rgba32>(x.FileName));

                    //
                    // 合并
                    var basedHeight = availableImageFile[0].Image
                                                           .Height;
                    var basedWidth = availableImageFile[0].Image
                                                          .Width;

                    var availableHeight  = Math.Clamp(Height, 64, 640);
                    var totalHeight      = basedHeight + availableHeight * (availableImageFile.Length - 1);
                    var combinationImage = new Image<Rgba32>(basedWidth, totalHeight);
                    
                    combinationImage.ProcessPixelRows(x =>
                    {
                        for (var i = 1; i < availableImageFile.Length; i++)
                        {
                            
                        }
                    });
                }
                catch(Exception ex)
                {
                    session.Update(ex.Message);
                    await session.Await();
                }
            }
        }

        /// <summary>
        /// 获取或设置 <see cref="Height"/> 属性。
        /// </summary>
        public int Height
        {
            get => _height;
            set => SetValue(ref _height, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="YOffset"/> 属性。
        /// </summary>
        public int YOffset
        {
            get => _yOffset;
            set => SetValue(ref _yOffset, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="Preview"/> 属性。
        /// </summary>
        public ImageSource Preview
        {
            get => _preview;
            set => SetValue(ref _preview, value);
        }

        public ObservableCollection<FileState> FileCollection { get; }

        public RelayCommand SelectInputFolderCommand { get; }
        public RelayCommand SelectInputPicturesCommand { get; }
        public RelayCommand ClearInputCommand { get; }
        public AsyncRelayCommand PreviewImageCommand { get; }
        public AsyncRelayCommand SaveImageCommand { get; }
    }
}