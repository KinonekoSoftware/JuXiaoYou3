using Acorisoft.Miga.Doc.Documents;
using Acorisoft.Miga.Xml;

namespace Acorisoft.Miga.Doc.Parts
{
    public class Module : ObservableObject, IAddChild
    {
        private string       _name;
        private string       _id;
        private string       _author;
        private string       _organization;
        private string       _contract;
        private int          _version;
        private DocumentKind _type;

        public void Accept(object instance)
        {
            Items.Add(instance as InputProperty2);
        }

        public ObservableCollection<InputProperty2> Items { get; } = new ObservableCollection<InputProperty2>();

        /// <summary>
        /// 获取或设置唯一标识符
        /// </summary>
        public string Id
        {
            get => _id;
            set => SetValue(ref _id, value);
        }

        /// <summary>
        /// 获取或设置作者
        /// </summary>
        public string Author
        {
            get => _author;
            set => SetValue(ref _author, value);
        }

        /// <summary>
        /// 获取或设置组织
        /// </summary>
        [Alias("org")]
        public string Organization
        {
            get => _organization;
            set => SetValue(ref _organization, value);
        }

        /// <summary>
        /// 获取或设置联系方式
        /// </summary>
        public string Contract
        {
            get => _contract;
            set => SetValue(ref _contract, value);
        }

        /// <summary>
        /// 获取或设置模组名称
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetValue(ref _name, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="Version"/> 属性。
        /// </summary>
        [Alias("ver")]
        public int Version
        {
            get => _version;
            set => SetValue(ref _version, value);
        }

        [Alias("type")]
        public DocumentKind Type
        {
            get => _type;
            set => SetValue(ref _type, value);
        }
    }
}