namespace Acorisoft.FutureGL.MigaDB.Data.Socials
{
    public class Channel : ChannelCache
    {
        /// <summary>
        /// 所有角色
        /// </summary>
        public Dictionary<string, MemberRole> Roles { get; init; }

        /// <summary>
        /// 所有映射
        /// </summary>
        public List<string> Alias { get; init; }

        /// <summary>
        /// 所有头衔
        /// </summary>
        public Dictionary<string, string> Titles { get; init; }

        /// <summary>
        /// 所有成员
        /// </summary>
        public List<MemberCache> Members { get; init; }

        /// <summary>
        /// 所有消息
        /// </summary>
        public List<MessageBase> Messages { get; init; }

        /// <summary>
        /// 正在发送的消息
        /// </summary>
        public string CompositionMessage { get; set; }
    }
}