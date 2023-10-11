using System.Collections.ObjectModel;

namespace Acorisoft.FutureGL.MigaDB.Data.Socials
{
    public class SocialChannel : StorageObject
    {
        /// <summary>
        /// 聊天室的名字
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 聊天室的头像
        /// </summary>
        public string Avatar { get; set; }
        
        /// <summary>
        /// 聊天室的简介
        /// </summary>
        public string Intro { get; set; }
        
        /// <summary>
        /// 聊天室未发出去的消息
        /// </summary>
        public string PendingContent { get; set; }
        
        /// <summary>
        /// 所有成员列表，包括已经在群的、退群的
        /// </summary>
        public List<string> MemberList { get; init; }
        
        /// <summary>
        /// 
        /// </summary>
        public List<string> AvailableMemberList { get; init; }

        /// <summary>
        /// 头衔的映射
        /// </summary>
        public Dictionary<string, string> TitleMapping { get; init; }
        
        /// <summary>
        /// 角色映射
        /// </summary>
        public Dictionary<string, MemberRole> RoleMapping { get; init; }
        
        /// <summary>
        /// 别名映射
        /// </summary>
        public Dictionary<string, Dictionary<string, string>> AliasMapping { get; init; }

        /// <summary>
        /// 最近发言的成员列表。
        /// </summary>
        public List<string> LatestSpeakers { get; init; }
        
        /// <summary>
        /// 当前发言的人
        /// </summary>
        public string Speaker { get; set; }

        /// <summary>
        /// 所有的消息
        /// </summary>
        public List<ChannelMessage> Messages { get; init; }
    }
}