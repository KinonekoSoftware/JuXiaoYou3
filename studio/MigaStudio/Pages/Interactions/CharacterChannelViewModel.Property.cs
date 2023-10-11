using System.Collections.Generic;
using Acorisoft.FutureGL.MigaDB.Data.Socials;
using Acorisoft.FutureGL.MigaDB.Documents;
using Acorisoft.FutureGL.MigaStudio.Pages.Interactions.Models;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Interactions
{
    partial class CharacterChannelViewModel
    {
        private          string                                         _message;
        private          MemberCache                                    _speaker;
        

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<MemberCache> LatestSpeakers { get; }
        
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<MessageUI> Messages { get; }
        
        public ReadOnlyObservableCollection<MemberCache> AvailableMembers { get; }
        public ReadOnlyObservableCollection<MemberCache> Members { get; }

        /// <summary>
        /// 获取或设置 <see cref="Speaker"/> 属性。
        /// </summary>
        public MemberCache Speaker
        {
            get => _speaker;
            set
            {
                SetValue(ref _speaker, value);

                if (_speaker is null)
                {
                    return;
                }
                
                //
                //
                SwitchSpeakerMemberBefore(_speaker);
                SwitchSpeakerMemberAfter(_speaker.Id);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Channel Channel { get; private set; }
        public ChannelCache Cache { get; private set; }

        /// <summary>
        /// 获取或设置 <see cref="Intro"/> 属性。
        /// </summary>
        public string Intro
        {
            get => Channel.Intro;
            set
            {
                Cache.Intro   = value;
                Channel.Intro = value;
                RaiseUpdated();
            }
        }
        
        /// <summary>
        /// 获取或设置 <see cref="CompositionMessage"/> 属性。
        /// </summary>
        public string CompositionMessage
        {
            get => Channel.CompositionMessage;
            set
            {
                Channel.CompositionMessage = value;
                RaiseUpdated();
            }
        }

        /// <summary>
        /// 获取或设置 <see cref="Message"/> 属性。
        /// </summary>
        public string Message
        {
            get => _message;
            set => SetValue(ref _message, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="ChannelName"/> 属性。
        /// </summary>
        public string ChannelName
        {
            get => Channel.Name;
            set
            {
                Cache.Name  = value;
                Channel.Name = value;
                RaiseUpdated();
            }
        }
        
        
        /// <summary>
        /// 获取或设置 <see cref="ChannelName"/> 属性。
        /// </summary>
        public string ChannelAvatar
        {
            get => Channel.Avatar;
            set
            {
                Cache.Avatar   = value;
                Channel.Avatar = value;
                RaiseUpdated();
            }
        }
    }
}