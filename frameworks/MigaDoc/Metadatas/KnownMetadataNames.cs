using System.Collections.ObjectModel;

namespace Acorisoft.Miga.Doc.Metadatas
{
    public static class KnownMetadataNames
    {
        #region Fields & Properties

        public static ReadOnlyCollection<string> Names => Lazy.Value;

        private static readonly Lazy<ReadOnlyCollection<string>> Lazy;

        #endregion


        static KnownMetadataNames()
        {
            var namesImpl = new List<string>
            {
                Name,
                Sex,
                Age,
                Weight,
                Height,
                Birth,
                Country,
                BloodType
            };
            Lazy = new Lazy<ReadOnlyCollection<string>>(new ReadOnlyCollection<string>(namesImpl));
        }

        public const string Name       = "@name";
        public const string Sex        = "@sex";
        public const string Age        = "@age";
        public const string Weight     = "@weight";
        public const string Height     = "@height";
        public const string Birth      = "@birth";
        public const string Country    = "@country";
        public const string BloodType  = "@b_type";
        public const string Avatar     = "@avatar";
        public const string Background = "@background";
        public const string Identity   = "@identity";
        public const string Slogan     = "@slogan";
        public const string Species    = "@species";

        // 分类
        public const string Skill_Type = "@skill_type";
        
        //
        public const string Skill_Identifier = "@skill_identifier";

        // 能力弹道
        public const string Skill_Attack = "@skill_attack";
        
        // 施放类型
        public const string Skill_Cast = "@skill_cast";
        
        
        // 属性
        public const string Elemental = "@elemental";
        
        // 稀有度
        public const string Rarity = "@rarity";
        
        
        // 类型
        public const string Skill_Slot = "@skill_slot";
        
        
        // 类型
        public const string Skill_Content = "@skill_content";
    }
}