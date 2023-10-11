namespace Acorisoft.FutureGL.MigaDB.Data.Templates.Modules
{
    public class OptionItem : ObservableObject
    {
        private string _name;
        private string _value;

        /// <summary>
        /// 获取或设置 <see cref="Value"/> 属性。
        /// </summary>
        public string Value
        {
            get => _value;
            set => SetValue(ref _value, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetValue(ref _name, value);
        }
    }

    public class OptionItemUI
    {
        private readonly Action<OptionItemUI> _expression;

        public OptionItemUI(string id, string value, OptionItem item, Action<OptionItemUI> expression)
        {
            Id            = id;
            SelectedValue = value;
            Item          = item;
            Name          = item.Name;
            Value         = item.Value;
            _expression   = expression;
        }

        public string SelectedValue { get; }
        /// <summary>
        /// 获取或设置 <see cref="Expression"/> 属性。
        /// </summary>
        public Action<OptionItemUI> Expression
        {
            get => _expression;
        }

        public OptionItem Item { get; }
        public string Id { get; init; }
        public string Name { get; init; }
        public string Value { get; init; }
    }
}