namespace Acorisoft.Miga.Doc.Groups
{
    public enum GroupType
    {
        None,
        Carry,
        Regular,
        Skill,
        TOBSkill,
        Titled,
        Pairing
    }
    
    public class GroupingInformation 
    {
        public string Id { get; init; }

        /// <summary>
        /// 获取或设置 <see cref="Avatar"/> 属性。
        /// </summary>
        public string Avatar{ get; set; }
        /// <summary>
        /// 获取或设置 <see cref="Summary"/> 属性。
        /// </summary>
        public string Summary{ get; set; }
        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Name
       { get; set; }
    }
}