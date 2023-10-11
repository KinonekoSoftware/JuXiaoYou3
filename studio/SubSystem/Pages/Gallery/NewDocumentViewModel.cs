using System.IO;
using System.Windows;
using System.Windows.Media;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.IO;
using Acorisoft.FutureGL.MigaStudio.Resources;
using Acorisoft.FutureGL.MigaStudio.Utilities;
using CommunityToolkit.Mvvm.Input;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Gallery
{
    public class NewDocumentViewModel : ImplicitDialogVM
    {
        private        bool         _selectedAvatar;
        private        string       _name;
        private        ImageSource  _avatar;
        private        MemoryStream _buffer;
        private static DocumentType _type = DocumentType.Character;
        private        Visibility   _visibility;
        
        public NewDocumentViewModel()
        {
            SetAvatarCommand = new AsyncRelayCommand(SetAvatarImpl);
        }

        protected override void OnStart(RoutingEventArgs parameter)
        {
            var p = parameter.Parameter;
            var a = p.Args;

            if (a[0] is DocumentType type)
            {
                Type       = type;
                Visibility = Visibility.Collapsed;
            }
            else
            {
                var iterator = a[0] as IEnumerable<DocumentType> ?? Constants.DocumentTypes;
                Types.AddMany(iterator);
                Visibility = Visibility.Visible;
            }
        }

        public static Task<Op<DocumentCache>> New()
        {
            return New(Constants.DocumentTypes);
        }
        
        public static Task<Op<DocumentCache>> New(DocumentType[] types)
        {
            return DialogService().Dialog<DocumentCache, NewDocumentViewModel>(new Parameter
            {
                Args = new object[] { types }
            });
        }
        
        public static Task<Op<DocumentCache>> New(DocumentType type)
        {
            return DialogService().Dialog<DocumentCache, NewDocumentViewModel>(new Parameter
            {
                Args = new object[] { type }
            });
        }
        
        private async Task SetAvatarImpl()
        {
            //
            //
            var op = await ImageUtilities.Avatar();

            //
            //
            if (!op.IsFinished)
            {
                return;
            }

            //
            //
            try
            {
                Avatar          = Xaml.FromStream(op.Buffer, 256, 256);
                _buffer         = op.Buffer;
                _selectedAvatar = true;
            }
            catch
            {
                await Xaml.Get<IBuiltinDialogService>().Notify(CriticalLevel.Danger, SubSystemString.Notify, SubSystemString.BadImage);
            }
        }


        protected override bool IsCompleted()
        {
            return !string.IsNullOrEmpty(Name);
        }

        private string ApplyAvatar()
        {
            var dm = Studio.DatabaseManager();

            //
            //
            if (!dm.IsOpen.CurrentValue)
            {
                Xaml.Get<IBuiltinDialogService>().Notify(
                    CriticalLevel.Warning,
                    SubSystemString.Notify,
                    SubSystemString.GetDatabaseResult(DatabaseFailedReason.DatabaseNotOpen));
                return string.Empty;
            }

            var    ie  = dm.GetEngine<ImageEngine>();
            var    raw = Pool.MD5.ComputeHash(_buffer);
            var    md5 = Convert.ToBase64String(raw);
            
            if (ie.HasFile(md5))
            {
                var fr = ie.Records.FindById(md5);
                return fr.Uri;
            }

            var avatar = ImageEngine.GetAvatarUri();
            ie.WriteAvatar(_buffer, avatar);

            var record = new FileRecord
            {
                Id   = md5,
                Uri  = avatar,
                Type = ResourceType.Image,
                Width = 256,
                Height = 256,
            };

            ie.AddFile(record);
            return avatar;
        }

        protected override void Finish()
        {
            var document = new DocumentCache
            {
                Id             = ID.Get(),
                Avatar         = _selectedAvatar ? ApplyAvatar() : null,
                Name           = _name,
                Removable      = false,
                Type           = _type,
                TimeOfCreated  = DateTime.Now,
                TimeOfModified = DateTime.Now,
                Version        = 0x10,
                IsDeleted      = false,
            };

            Result = document;
        }

        protected override string Failed() => SubSystemString.Unknown;
        
        /// <summary>
        /// 是否隐藏
        /// </summary>
        public Visibility Visibility
        {
            get => _visibility;
            set => SetValue(ref _visibility, value);
        }

        /// <summary>
        /// 文档类型
        /// </summary>
        public DocumentType Type
        {
            get => _type;
            set => SetValue(ref _type, value);
        }

        /// <summary>
        /// 头像
        /// </summary>
        public ImageSource Avatar
        {
            get => _avatar;
            set => SetValue(ref _avatar, value);
        }

        /// <summary>
        /// 名字
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetValue(ref _name, value);
        }

        public ObservableCollection<DocumentType> Types { get; }
        public AsyncRelayCommand SetAvatarCommand { get; }
    }
}