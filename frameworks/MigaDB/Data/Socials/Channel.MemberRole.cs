namespace Acorisoft.FutureGL.MigaDB.Data.Socials
{
    public enum MemberRole
    {
        /// <summary>
        /// 群主
        /// </summary>
        Owner,
        
        /// <summary>
        /// 管理
        /// </summary>
        Manager,
        
        /// <summary>
        /// 特殊头衔
        /// </summary>
        Special,
        
        /// <summary>
        /// 成员
        /// </summary>
        Member,
    }

    public class MemberCache : StorageObject
    {
        public string Avatar { get; set; }
        public string Name { get; set; }
    }
}