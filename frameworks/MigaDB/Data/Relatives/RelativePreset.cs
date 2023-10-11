namespace Acorisoft.FutureGL.MigaDB.Data.Relatives
{
    public class RelativePreset : StorageUIObject
    {
        private string _callOfSource;
        private string _callOfTarget;
        private bool   _directRelative;
        private bool   _collateralRelative;
        private bool   _conjugalRelative;
        private string _name;

        /// <summary>
        /// 旁系亲属
        /// </summary>
        public bool CollateralRelative
        {
            get => _collateralRelative;
            set => SetValue(ref _collateralRelative, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetValue(ref _name, value);
        }

        /// <summary>
        /// 夫妻关系
        /// </summary>
        public bool ConjugalRelative
        {
            get => _conjugalRelative;
            set => SetValue(ref _conjugalRelative, value);
        }

        /// <summary>
        /// 直系亲属
        /// </summary>
        public bool DirectRelative
        {
            get => _directRelative;
            set => SetValue(ref _directRelative, value);
        }

        /// <summary>
        /// 右边的称呼
        /// </summary>
        public string CallOfTarget
        {
            get => _callOfTarget;
            set => SetValue(ref _callOfTarget, value);
        }

        /// <summary>
        /// 左边的称呼
        /// </summary>
        public string CallOfSource
        {
            get => _callOfSource;
            set => SetValue(ref _callOfSource, value);
        }
    }
}