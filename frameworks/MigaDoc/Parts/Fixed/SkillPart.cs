using System.Collections.ObjectModel;

namespace Acorisoft.Miga.Doc.Parts
{
    public class SkillPart : FixedDataPart
    {
        private DocumentIndexCopy _passive;
        private DocumentIndexCopy _cost;
        private DocumentIndexCopy _skill1;
        private DocumentIndexCopy _skill2;
        private DocumentIndexCopy _skill3;
        private DocumentIndexCopy _skill4;

        /// <summary>
        /// 获取或设置 <see cref="Skill4"/> 属性。
        /// </summary>
        public DocumentIndexCopy Skill4{ get; set; }
        /// <summary>
        /// 获取或设置 <see cref="Skill3"/> 属性。
        /// </summary>
        public DocumentIndexCopy Skill3{ get; set; }
        /// <summary>
        /// 获取或设置 <see cref="Skill2"/> 属性。
        /// </summary>
        public DocumentIndexCopy Skill2{ get; set; }
        /// <summary>
        /// 获取或设置 <see cref="Skill1"/> 属性。
        /// </summary>
        public DocumentIndexCopy Skill1{ get; set; }
        /// <summary>
        /// 获取或设置 <see cref="Cost"/> 属性。
        /// </summary>
        public DocumentIndexCopy Cost{ get; set; }
        /// <summary>
        /// 获取或设置 <see cref="Passive"/> 属性。
        /// </summary>
        public DocumentIndexCopy Passive{ get; set; }
        
        public ObservableCollection<DocumentIndexCopy> Skills { get; set; }
    }
}