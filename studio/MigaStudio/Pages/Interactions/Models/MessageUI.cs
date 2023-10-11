using System.Windows;
using System.Windows.Media;
using Acorisoft.FutureGL.MigaDB.Data.Socials;
using Acorisoft.FutureGL.MigaUtils;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Interactions.Models
{
    public delegate Tuple<string, string, MemberRole, string> MemberInformationFinder(string id);

    public abstract class MessageUI : ObservableObject
    {
        private bool _isSelf;

        /// <summary>
        /// 获得消息源
        /// </summary>
        public abstract MessageBase Source { get; }

        /// <summary>
        /// 发言人ID
        /// </summary>
        public string MemberID { get; init; }

        /// <summary>
        /// 是否为本人发言。
        /// </summary>
        public bool IsSelf
        {
            get => _isSelf;
            set => SetValue(ref _isSelf, value);
        }
    }

    public abstract class UserBasedMessageUI : MessageUI, IMessageUpdateSupport
    {
        private readonly MemberInformationFinder _memberFinder;

        private static readonly LinearGradientBrush OwnerBrush = new LinearGradientBrush
        {
            StartPoint = new Point(0, 0),
            EndPoint   = new Point(1, 1),
            GradientStops = new GradientStopCollection
            {
                new GradientStop(Color.FromRgb(255, 199, 60), 0),
                new GradientStop(Color.FromRgb(255, 221, 54), 1),
            }
        };

        private static readonly LinearGradientBrush ManagerBrush = new LinearGradientBrush
        {
            StartPoint = new Point(0, 0),
            EndPoint   = new Point(1, 1),
            GradientStops = new GradientStopCollection
            {
                new GradientStop(Color.FromRgb(51, 216, 202), 0),
                new GradientStop(Color.FromRgb(69, 234, 220), 1),
            }
        };

        private string     _name;
        private string     _avatar;
        private string     _title;
        private MemberRole _role;

        protected UserBasedMessageUI(ChannelMessage msg, MemberInformationFinder memberFinder)
        {
            _memberFinder = memberFinder;
            MemberID      = msg.MemberID;
            MessageSource = msg;
            Update();
        }

        public void Update()
        {
            var (name, avatar, role, title) = _memberFinder(MessageSource.MemberID);
            Name                            = name;
            Avatar                          = avatar;
            Role                            = role;
            Title                           = title;
            RaiseUpdated(nameof(HasTitle));
            RaiseUpdated(nameof(Brush));
        }

        protected virtual void OnUpdate()
        {
        }


        public ChannelMessage MessageSource { get; }


        public sealed override MessageBase Source => MessageSource;


        /// <summary>
        /// 获取或设置 <see cref="HasTitle"/> 属性。
        /// </summary>
        public bool HasTitle => !string.IsNullOrEmpty(Title);

        /// <summary>
        /// 获取或设置 <see cref="Brush"/> 属性。
        /// </summary>
        public Brush Brush
        {
            get
            {
                if (Role == MemberRole.Owner)
                {
                    return OwnerBrush;
                }

                return Role == MemberRole.Member ? null : ManagerBrush;
            }
        }

        /// <summary>
        /// 获取或设置 <see cref="Role"/> 属性。
        /// </summary>
        public MemberRole Role
        {
            get => _role;
            set => SetValue(ref _role, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="Title"/> 属性。
        /// </summary>
        public string Title
        {
            get => _title;
            set => SetValue(ref _title, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="Avatar"/> 属性。
        /// </summary>
        public string Avatar
        {
            get => _avatar;
            set => SetValue(ref _avatar, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetValue(ref _name, value);
        }
    }

    public interface IMessageUpdateSupport
    {
        void Update();
    }
}