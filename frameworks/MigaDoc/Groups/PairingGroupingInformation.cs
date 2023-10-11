namespace Acorisoft.Miga.Doc.Groups
{
    public class PairingGroupingInformation : TitledTeamGroupingInformation
    {
        /// <summary>
        /// 获取或设置 <see cref="CarrySummary"/> 属性。
        /// </summary>
        public string CarrySummary { get; set; }

        /// <summary>
        /// 获取或设置 <see cref="Carry"/> 属性。
        /// </summary>
        public DocumentIndexCopy Carry { get; set; }
    }
}