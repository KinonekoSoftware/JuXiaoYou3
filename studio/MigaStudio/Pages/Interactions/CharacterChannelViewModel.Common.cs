using System.Collections.Generic;
using System.Linq;
using System.Windows.Interop;
using Acorisoft.FutureGL.MigaDB.Data.Socials;
using Acorisoft.FutureGL.MigaDB.Documents;
using Acorisoft.FutureGL.MigaStudio.Pages.Interactions.Models;
using Acorisoft.FutureGL.MigaUtils;
using Acorisoft.FutureGL.MigaUtils.Collections;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Interactions
{
    partial class CharacterChannelViewModel
    {
        private const char Separator = '\x20';

        protected void SwitchSpeakerMemberBefore(MemberCache cache)
        {
            if(LatestSpeakers.Count < 10 &&
               !LatestSpeakers.Contains(cache))
            {
                LatestSpeakers.Add(cache);
            }
            else if (!LatestSpeakers.Contains(cache))
            {
                LatestSpeakers.Insert(0, cache);
                LatestSpeakers.RemoveAt(10);
            }
        }

        protected void SwitchSpeakerMemberAfter(string memberID)
        {
            
            foreach (var message in Messages)
            {
                message.IsSelf = message.MemberID == memberID;
                if (message is IMessageUpdateSupport mus)
                {
                    mus.Update();
                }
            }
        }

        #region Load

        private void LoadFromChannel()
        {
            if (Channel is null ||
                Cache is null)
            {
                return;
            }

            LoadMembers();

            //
            // 加载映射
            LoadMapper();

            LatestSpeakers.Clear();

            Channel.Messages
                   .ForEach(x => AddMessageTo(x, false));
        }

        private void LoadMembers()
        {
            foreach (var member in Channel.Members
                                          .Where(member => MemberMapper.TryAdd(member.Id, member)))
            {
                var cache = DocumentEngine.GetCache(member.Id);

                if (cache is not null)
                {
                    var hasChanged = false;

                    if (member.Name != cache.Name)
                    {
                        member.Name = cache.Name;
                        hasChanged  = true;
                    }

                    if (member.Avatar != cache.Avatar)
                    {
                        member.Avatar = cache.Avatar;
                        hasChanged    = true;
                    }

                    if (hasChanged) SocialEngine.AddCharacter(member);
                }

                TotalMemberCollection.Add(member);
            }

            var available = Channel.AvailableMembers
                                   .Select(x => MemberMapper.TryGetValue(x, out var c) ? c : null)
                                   .Where(x => x is not null);

            AvailableMemberCollection.AddMany(available, true);
        }

        private void LoadMapper()
        {
            MemberAliasMapper.Clear();
            MemberRoleMapper.Clear();
            MemberTitleMapper.Clear();

            //
            // 别名
            foreach (var alias in Channel.Alias)
            {
                var formatted = alias.Split(Separator);

                if (formatted is null ||
                    formatted.Length != 3)
                {
                    continue;
                }

                var source = formatted[0];
                var target = formatted[1];
                var callee = formatted[2];

                if (string.IsNullOrEmpty(source) ||
                    string.IsNullOrEmpty(target) ||
                    string.IsNullOrEmpty(callee))
                {
                    continue;
                }

                if (MemberAliasMapper.TryGetValue(source, out var dict))
                {
                    dict[target] = callee;
                }
                else
                {
                    dict = new Dictionary<string, string>
                    {
                        [target] = callee
                    };
                    MemberAliasMapper.TryAdd(source, dict);
                }
            }

            MemberTitleMapper.AddMany(Channel.Titles);
            MemberRoleMapper.AddMany(Channel.Roles);
        }

        #endregion


        private void SaveToChannel()
        {
            if (Channel is null ||
                Cache is null)
            {
                return;
            }

            Channel.Alias.Clear();
            Channel.Roles.Clear();
            Channel.Titles.Clear();

            // ReSharper disable ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
            foreach (var alias in MemberAliasMapper)
            {
                foreach (var aliasItem in alias.Value)
                {
                    var v = $"{alias.Key}{Separator}{aliasItem.Key}{Separator}{aliasItem.Value}";

                    Channel.Alias.Add(v);
                }
            }

            var members = Channel.AvailableMembers
                                 .ToArray();
            
            Cache.AvailableMembers
                 .AddMany(members, true);

            Channel.Titles
                   .AddMany(MemberTitleMapper, true);

            Channel.Roles
                   .AddMany(MemberRoleMapper, true);

            Channel.Messages
                   .AddMany(Messages.Select(x => x.Source), true);

            //
            //
            SocialEngine.AddChannel(Cache);
            SocialEngine.AddChannel(Channel);
        }


        private void AddMessageTo(MessageBase mb, bool addToChannel = true)
        {
            if (mb is ChannelMessage msg)
            {
                MessageUI ui = msg.Type switch
                {
                    MessageType.PlainText   => new PlainTextUI(msg, FindMemberById),
                    MessageType.Emoji       => new EmojiUI(msg, FindMemberById),
                    MessageType.Image       => new ImageUI(msg, FindMemberById),
                    MessageType.Timestamp   => new TimestampUI(msg),
                    MessageType.Muted       => new MutedAndUnMutedEventUI(msg, FindMemberNameById, FindEventContentByType),
                    MessageType.MemberJoin  => new MemberJoinEventUI(msg, FindMemberNameById),
                    MessageType.MemberLeave => new MemberLeaveEventUI(msg, FindMemberNameById),
                    MessageType.UnMuted     => new MutedAndUnMutedEventUI(msg, FindMemberNameById, FindEventContentByType),
                    _                       => throw new ArgumentOutOfRangeException()
                };

                ui.IsSelf = Speaker is not null && Speaker.Id == ui.MemberID;
                Messages.Add(ui);
                if (addToChannel) Channel.Messages.Add(msg);
            }

            SetDirtyState();
        }

        private void RemoveMessage(MessageUI msg)
        {
            var src = msg.Source;
            Messages.Remove(msg);
            Channel.Messages
                   .Remove(src);
            SetDirtyState();
        }

        private Tuple<string, string, MemberRole, string> FindMemberById(string id)
        {
            string name;
            string avatar;

            if (MemberMapper.TryGetValue(id, out var mem))
            {
                name   = mem.Name;
                avatar = mem.Avatar;
            }
            else
            {
                name   = string.Empty;
                avatar = string.Empty;
            }

            //
            // get name
            if (Speaker is not null &&
                MemberAliasMapper.TryGetValue(Speaker.Id, out var dict) &&
                dict.TryGetValue(id, out var alias))
            {
                name = alias;
            }


            if (!MemberTitleMapper.TryGetValue(id, out var title)) title = string.Empty;
            if (!MemberRoleMapper.TryGetValue(id, out var role)) role    = MemberRole.Member;

            if (role == MemberRole.Owner ||
                role == MemberRole.Manager)
            {
                title = Language.GetEnum(role);
            }

            return new Tuple<string, string, MemberRole, string>(name, avatar, role, title);
        }

        private string FindMemberNameById(string id)
        {
            if (Speaker is not null &&
                MemberAliasMapper.TryGetValue(Speaker.Id, out var dict))
            {
                if (dict.TryGetValue(id, out var name))
                {
                    return name;
                }
            }

            return MemberMapper.TryGetValue(id, out var mem) ? mem.Name : Language.GetText("text.Untitled");
        }

        private string FindEventContentByType(MessageType type, object parameter)
        {
            return type switch
            {
                MessageType.Muted   => string.Format(Language.GetText("text.Interaction.MemberMuted"), parameter),
                MessageType.UnMuted => Language.GetText("text.Interaction.MemberUnMuted"),
                _                   => string.Empty
            };
        }
    }
}