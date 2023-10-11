using System.Collections.Generic;
using System.Linq;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.MigaDB.Data.Socials;
using Acorisoft.FutureGL.MigaDB.Documents;
using Acorisoft.FutureGL.MigaDB.Utils;
using Acorisoft.FutureGL.MigaStudio.Controls.Socials;
using Acorisoft.FutureGL.MigaStudio.Pages.Interactions.Models;
using Acorisoft.FutureGL.MigaUtils.Collections;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Interactions
{
    public partial class CharacterChannelViewModel : TabViewModel
    {
        private readonly Dictionary<string, Dictionary<string, string>> MemberAliasMapper;
        private readonly Dictionary<string, MemberRole>                 MemberRoleMapper;
        private readonly Dictionary<string, string>                     MemberTitleMapper;
        private readonly Dictionary<string, MemberCache>                MemberMapper;
        private readonly ObservableCollection<MemberCache>              TotalMemberCollection;
        private readonly ObservableCollection<MemberCache>              AvailableMemberCollection;

        public CharacterChannelViewModel()
        {
            SocialEngine   = Studio.Engine<SocialEngine>();
            DocumentEngine = Studio.Engine<DocumentEngine>();

            MemberAliasMapper         = new Dictionary<string, Dictionary<string, string>>();
            MemberRoleMapper          = new Dictionary<string, MemberRole>();
            MemberTitleMapper         = new Dictionary<string, string>();
            MemberMapper              = new Dictionary<string, MemberCache>();
            TotalMemberCollection     = new ObservableCollection<MemberCache>();
            Members                   = new ReadOnlyObservableCollection<MemberCache>(TotalMemberCollection);
            Messages                  = new ObservableCollection<MessageUI>();
            LatestSpeakers            = new ObservableCollection<MemberCache>();
            AvailableMemberCollection = new ObservableCollection<MemberCache>();
            AvailableMembers          = new ReadOnlyObservableCollection<MemberCache>(AvailableMemberCollection);

            SaveCommand                  = AsyncCommand(SaveWithNotification);
            AddImageCommand              = Command(AddImageCommandImpl);
            RemoveMessageCommand         = AsyncCommand<MessageUI>(RemoveMessageImpl);
            AddMemberRoleCommand         = AsyncCommand(AddMemberRoleImpl);
            AddMemberTitleCommand        = AsyncCommand(AddMemberTitleImpl);
            RemoveMemberTitleCommand     = AsyncCommand<MemberCache>(RemoveMemberTitleImpl);
            RemoveMemberRoleCommand      = AsyncCommand<MemberCache>(RemoveMemberRoleImpl);
            AddMemberJoinCommand         = AsyncCommand(AddMemberJoinCommandImpl);
            AddMemberLeaveCommand        = AsyncCommand(AddMemberLeaveCommandImpl);
            AddMemberMutedCommand        = AsyncCommand(AddMemberMutedCommandImpl);
            AddMemberUnMutedCommand      = AsyncCommand(AddMemberUnMutedCommandImpl);
            AddTimestampCommand          = AsyncCommand(AddTimestampCommandImpl);
            AddPlainTextCommand          = Command(AddPlainTextCommandImpl);
            SetCompositionMessageCommand = AsyncCommand(SetCompositionMessageImpl);
            SwitchSpeakerCommand         = Command<MemberCache>(SwitchSpeakerCommandImpl);
        }

        private async Task SaveWithNotification()
        {
            Save();
            await this.Successful(SR.OperationOfSaveIsSuccessful);
        }

        private void Save()
        {
            //
            // 
            SetDirtyState(false);

            //
            // 保存信息
            SaveToChannel();
        }

        protected override void OnStart(Parameter parameter)
        {
            var arg = parameter.Args;

            if (arg is not null &&
                arg.Length >= 2)
            {
                Channel = arg[0] as Channel;
                Cache   = arg[1] as ChannelCache;
            }

            Channel ??= new Channel
            {
                Id               = ID.Get(),
                Members          = new List<MemberCache>(),
                Messages         = new List<MessageBase>(),
                AvailableMembers = new List<string>(),
                Alias            = new List<string>(),
                Roles            = new Dictionary<string, MemberRole>(),
                Titles           = new Dictionary<string, string>(),
            };

            Cache ??= new ChannelCache
            {
                Id               = Channel.Id,
                AvailableMembers = new List<string>()
            };

            //
            // 加载
            LoadFromChannel();

            //
            // 默认发言人
            Speaker ??= AvailableMembers.FirstOrDefault();
            Title   =   Channel.Name;

            base.OnStart(parameter);
        }

        public override void Suspend()
        {
            Save();
            base.Suspend();
        }

        protected override void OnStop()
        {
            Save();
            base.OnStop();
        }

        public SocialEngine SocialEngine { get; }
        public DocumentEngine DocumentEngine { get; }
    }
}