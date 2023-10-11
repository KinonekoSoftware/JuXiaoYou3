namespace Acorisoft.FutureGL.MigaDB.Data.Socials
{
    public class ChannelCache : StorageObject
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
        /// 所有成员
        /// </summary>
        public List<string> AvailableMembers { get; init; }
    }
}