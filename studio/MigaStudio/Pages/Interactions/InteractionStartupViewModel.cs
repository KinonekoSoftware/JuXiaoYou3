using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms.VisualStyles;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.Views;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Data;
using Acorisoft.FutureGL.MigaDB.Data.Socials;
using Acorisoft.FutureGL.MigaDB.Documents;
using Acorisoft.FutureGL.MigaDB.Interfaces;
using Acorisoft.FutureGL.MigaDB.Services;
using Acorisoft.FutureGL.MigaDB.Utils;
using Acorisoft.FutureGL.MigaStudio.Pages.Commons;
using Acorisoft.FutureGL.MigaStudio.Pages.Interactions.Dialogs;
using Acorisoft.FutureGL.MigaUtils.Collections;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Interactions
{
    public class InteractionStartupViewModel : InteractionViewModelBase
    {
        private MemberCache _selectedCharacter;

        public InteractionStartupViewModel()
        {
            SocialEngine   = Studio.Engine<SocialEngine>();
            DocumentEngine = Studio.Engine<DocumentEngine>();

            Channels   = new ObservableCollection<ChannelCache>();
            Characters = new ObservableCollection<MemberCache>();

            AddCharacterCommand    = AsyncCommand(AddCharacterImpl);
            RemoveCharacterCommand = AsyncCommand<MemberCache>(RemoveCharacterImpl);
            AddChannelCommand      = AsyncCommand<MemberCache>(AddChannelImpl);
            OpenChannelCommand     = Command<ChannelCache>(OpenChannelImpl);
            RemoveChannelCommand   = AsyncCommand<ChannelCache>(RemoveChannelImpl);
        }

        protected override void OnStart()
        {
            Characters.AddMany(SocialEngine.GetMembers(), true);

            var id = Studio.Database()
                           .Get<PresetProperty>()
                           .LastSocialCharacterID;

            if (string.IsNullOrEmpty(id) && Characters.Count > 0)
            {
                SelectedCharacter = Characters.First();
            }

            RaiseUpdated(nameof(HasCharacters));
            RaiseUpdated(nameof(IsCharacterSelected));
            RaiseUpdated(nameof(HasCharacterAndNotChannel));
            base.OnStart();
        }

        protected override void OnResume()
        {
            if (Version != SocialEngine.Version)
            {
                Version = SocialEngine.Version;
                Characters.AddMany(SocialEngine.GetMembers(), true);
            }

            base.OnResume();
        }

        private async Task AddCharacterImpl()
        {
            var hash = Characters.Select(x => x.Id)
                                 .ToHashSet();

            var r = await NewMemberViewModel.Add(hash);

            if (!r.IsFinished)
            {
                return;
            }

            foreach (var member in r.Value)
            {
                SelectedCharacter ??= member;
                SocialEngine.AddCharacter(member);
                Characters.Add(member);
            }

            RaiseUpdated(nameof(HasCharacters));
        }

        private async Task RemoveCharacterImpl(MemberCache cache)
        {
            if (cache is null)
            {
                return;
            }

            if (!await this.Error(SR.AreYouSureRemoveIt))
            {
                return;
            }

            SocialEngine.RemoveCharacter(cache);
            Characters.Remove(cache);

            if (ReferenceEquals(SelectedCharacter, cache))
            {
                SelectedCharacter = Characters.FirstOrDefault();
            }
        }


        private async Task AddChannelImpl(MemberCache cache)
        {
            cache ??= SelectedCharacter;

            if (cache is null)
            {
                return;
            }

            var r = await SingleLineViewModel.String(SR.EditNameTitle);

            if (!r.IsFinished)
            {
                return;
            }

            var it = new ChannelCache
            {
                Id               = ID.Get(),
                AvailableMembers = new List<string> { cache.Id },
                Name             = r.Value,
            };

            SocialEngine.AddChannel(it);
            Channels.Add(it);
            RaiseUpdated(nameof(HasCharacters));
            RaiseUpdated(nameof(IsCharacterSelected));
            RaiseUpdated(nameof(HasCharacterAndNotChannel));
        }


        private void OpenChannelImpl(ChannelCache cache)
        {
            if (cache is null)
            {
                return;
            }

            var channel = SocialEngine.GetChannel(cache.Id);

            if (channel is null)
            {
                channel = new Channel
                {
                    Id               = cache.Id,
                    Name             = cache.Name,
                    AvailableMembers = cache.AvailableMembers,
                    Alias            = new List<string>(),
                    Members          = new List<MemberCache>(),
                    Roles            = new Dictionary<string, MemberRole>(),
                    Messages         = new List<MessageBase>(),
                    Titles           = new Dictionary<string, string>()
                };

                foreach (var item in cache.AvailableMembers)
                {
                    channel.Roles
                           .Add(item, MemberRole.Owner);

                    var ch = Characters.FirstOrDefault(x => x.Id == item);

                    if (ch is not null)
                    {
                        channel.Members
                               .Add(ch);
                    }
                }

                SocialEngine.AddChannel(channel);
            }

            Controller.New<CharacterChannelViewModel>(channel.Id, new Parameter
            {
                Args = new object[]
                {
                    channel,
                    cache
                }
            });
        }


        private async Task RemoveChannelImpl(ChannelCache cache)
        {
            if (cache is null)
            {
                return;
            }

            if (!await this.Error(SR.AreYouSureRemoveIt))
            {
                return;
            }

            SocialEngine.RemoveChannel(cache);
            Channels.Remove(cache);
        }

        /// <summary>
        /// 获取或设置 <see cref="SelectedCharacter"/> 属性。
        /// </summary>
        public MemberCache SelectedCharacter
        {
            get => _selectedCharacter;
            set
            {
                SetValue(ref _selectedCharacter, value);

                if (value is null)
                {
                    RaiseUpdated(nameof(HasCharacterAndNotChannel));
                    RaiseUpdated(nameof(IsCharacterSelected));
                    return;
                }

                //
                // 设置最后一个选择的人物
                var prop = Studio.Database()
                                 .Get<PresetProperty>();

                prop.LastSocialCharacterID = value.Id;

                Studio.Database()
                      .Update(prop);

                //
                // 获得有关的频道
                var channels = SocialEngine.GetChannels(value.Id);
                Channels.AddMany(channels, true);

                RaiseUpdated(nameof(HasCharacterAndNotChannel));
                RaiseUpdated(nameof(IsCharacterSelected));
            }
        }

        /// <summary>
        /// 获取或设置 <see cref="HasCharacters"/> 属性。
        /// </summary>
        public bool HasCharacters => Characters.Count > 0;


        /// <summary>
        /// 获取或设置 <see cref="IsCharacterSelected"/> 属性。
        /// </summary>
        public bool IsCharacterSelected => SelectedCharacter is not null;

        /// <summary>
        /// 获取或设置 <see cref="HasCharacterAndNotChannel"/> 属性。
        /// </summary>
        public bool HasCharacterAndNotChannel => SelectedCharacter is not null && Channels.Count == 0;

        public override bool Removable => false;
        public override bool Uniqueness => true;

        public ObservableCollection<ChannelCache> Channels { get; }
        public AsyncRelayCommand AddCharacterCommand { get; }

        public AsyncRelayCommand<MemberCache> AddChannelCommand { get; }
        public AsyncRelayCommand<ChannelCache> RemoveChannelCommand { get; }
        public RelayCommand<ChannelCache> OpenChannelCommand { get; }
        public AsyncRelayCommand<MemberCache> RemoveCharacterCommand { get; }
        public SocialEngine SocialEngine { get; }
        public DocumentEngine DocumentEngine { get; }
        public ObservableCollection<MemberCache> Characters { get; }
    }
}