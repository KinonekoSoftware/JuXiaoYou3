using System.Threading.Tasks;
using System.Windows;
using Acorisoft.FutureGL.Forest.ViewModels;
using Acorisoft.FutureGL.MigaDB;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Services;
using CommunityToolkit.Mvvm.Input;
using Ookii.Dialogs.Wpf;

namespace Acorisoft.FutureGL.MigaStudio
{
    public static class Studio
    {
        private static readonly Lazy<IDatabaseManager> _databaseField = new Lazy<IDatabaseManager>(Xaml.Get<IDatabaseManager>);

        public static void CheckFlag(string id, Action callback)
        {
            var db = Database();
            if (!db.Boolean(id))
            {
                callback?.Invoke();
                db.Boolean(id, true);
            }
        }
        
        //
        // public static async Task CheckFlag(string id, Action callback)
        // {
        //     var db = Database();
        //     if (!db.Boolean(id))
        //     {
        //         await callback?.Invoke();
        //         db.Boolean(id, true);
        //     }
        // }


        public static IDatabaseManager DatabaseManager() => _databaseField.Value;

        public static T This<T>() where T : class => _databaseField.Value
                                                                   .Database
                                                                   .CurrentValue
                                                                   .Get<T>();

        public static IDatabase Database() => _databaseField.Value
                                                            .Database
                                                            .CurrentValue;

        public static T Engine<T>() where T : DataEngine => _databaseField.Value.GetEngine<T>();

        public const string ZipExt = "*.zip";
        public const string PngExt =  "*.png";
        
        public static string ZipFilter
        {
            get => Language.Culture switch
            {
                CultureArea.English  => "Zip File|*.zip",
                CultureArea.French   => "Fichier Zip|*.zip",
                CultureArea.Japanese => "Zip ファイル|*.zip",
                CultureArea.Korean   => "Zip 파일|*.zip",
                CultureArea.Russian  => "Файл Zip|*.zip",
                _                    => "Zip压缩文件|*.zip",
            };
        }
        public static string PngFilter
        {
            get => Language.Culture switch
            {
                CultureArea.English  => "PNG File|*.png",
                CultureArea.French   => "Fichier PNG|*.png",
                CultureArea.Japanese => "PNG ファイル|*.png",
                CultureArea.Korean   => "PNG 파일|*.png",
                CultureArea.Russian  => "Файл PNG|*.png",
                _                    => "图片文件|*.png",
            };
        }
        
        
        public static string ImageFilter
        {
            get => Language.Culture switch
            {
                CultureArea.English  => "Image File|*.png;*.jpg;*.bmp;*.jpeg",
                CultureArea.French   => "Fichier image|*.png;*.jpg;*.bmp;*.jpeg",
                CultureArea.Japanese => "画像ファイル|*.png;*.jpg;*.bmp;*.jpeg",
                CultureArea.Korean   => "Image 파일|*.png;*.jpg;*.bmp;*.jpeg",
                CultureArea.Russian  => "Файл изображения|*.png;*.jpg;*.bmp;*.jpeg",
                _                    => "图片文件|*.png;*.jpg;*.bmp;*.jpeg",
            };
        }
        
        public static async Task CaptureAsync(FrameworkElement element)
        {
            if (element is null)
            {
                return;
            }

            var savedlg = Save(PngFilter, PngExt);

            if (savedlg.ShowDialog() != true)
            {
                return;
            }

            var ms = Xaml.CaptureToStream(element);
            await System.IO.File.WriteAllBytesAsync(savedlg.FileName, ms.GetBuffer());
        }

        public static void GotoPage(Type element)
        {
            if (element is null)
            {
                return;
            }

            var shellCore = Xaml.Get<TabBaseAppViewModel>();

            if (shellCore is null)
            {
                return;
            }

            var controller = shellCore.Controller as ShellCore;
            controller?.New(element);
        }

        public static VistaSaveFileDialog Save(string filter, string defaultExt, string fileName = null)
        {
            return new VistaSaveFileDialog
            {
                FileName     = fileName,
                Filter       = filter,
                AddExtension = true,
                DefaultExt   = defaultExt
            };
        }

        public static VistaOpenFileDialog Open(string filter, bool multiselect = false)
        {
            return new VistaOpenFileDialog
            {
                Filter      = filter,
                Multiselect = multiselect
            };
        }
        
        public static AsyncRelayCommand<FrameworkElement> CaptureCommand { get; } = new AsyncRelayCommand<FrameworkElement>(CaptureAsync);
        public static RelayCommand<Type> GotoPageCommand { get; } = new RelayCommand<Type>(GotoPage);
    }
}