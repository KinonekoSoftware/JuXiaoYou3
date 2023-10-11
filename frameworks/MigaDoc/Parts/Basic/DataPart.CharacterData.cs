using System.Collections.ObjectModel;

namespace Acorisoft.Miga.Doc.Parts
{
     public class CharacterData : DocumentData
    {
        public SkillData Skills { get; set; }
        public ObservableCollection<DocumentIndex> SkillData { get; set; }
        public ObservableCollection<string> MainTheme { get; set; }
        public ObservableCollection<object> StoryBoard { get; set; }
        public ObservableCollection<Relationship> Relationships { get; set; }
        public ObservableCollection<string> Lines { get; set; }
    }

    public class SkillData
    {

        /// <summary>
        /// 获取或设置 <see cref="Skill4"/> 属性。
        /// </summary>
        public DocumentIndex Skill4{ get; set; }

        /// <summary>
        /// 获取或设置 <see cref="Skill3"/> 属性。
        /// </summary>
        public DocumentIndex Skill3{ get; set; }
        /// <summary>
        /// 获取或设置 <see cref="Skill2"/> 属性。
        /// </summary>
        public DocumentIndex Skill2{ get; set; }
        /// <summary>
        /// 获取或设置 <see cref="Skill1"/> 属性。
        /// </summary>
        public DocumentIndex Skill1{ get; set; }
        /// <summary>
        /// 获取或设置 <see cref="Cost"/> 属性。
        /// </summary>
        public DocumentIndex Cost{ get; set; }

        /// <summary>
        /// 获取或设置 <see cref="Passive"/> 属性。
        /// </summary>
        public DocumentIndex Passive{ get; set; }
    }
}