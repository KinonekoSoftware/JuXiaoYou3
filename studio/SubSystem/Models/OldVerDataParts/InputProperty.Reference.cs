using System.Xml.Linq;
using Acorisoft.Miga.Xml;

namespace Acorisoft.Miga.Doc.Parts
{
    [Alias("ref")]
    public class ReferenceProperty2 : InputProperty2
    {
        protected sealed override InputProperty2 CreateInstanceOverride()
        {
            return new ReferenceProperty2();
        }

        protected override void ShadowCopy(InputProperty2 target)
        {
            var tp = (ReferenceProperty2)target;

            tp.Fallback    = Fallback;
            tp.Source      = Source;
            tp.MultiSelect = MultiSelect;

            base.ShadowCopy(target);
        }

        protected internal override XElement GetElementOverride()
        {
            var element = new XElement("ref");


            //
            //
            Write(element);

            element.Add(new XAttribute("fallback", Fallback ?? string.Empty));
            element.Add(new XAttribute("src", Source ?? string.Empty));
            element.Add(new XAttribute("multiSelect", MultiSelect));

            return element;
        }
        
        

        public override bool IsCompleted()
        {
            return base.IsCompleted() && !string.IsNullOrEmpty(Source);
        }

        private string _fallback;
        private string _source;
        private bool _multiSelect;

        /// <summary>
        /// 获取或设置当前文本属性的回滚属性
        /// </summary>
        public string Fallback
        {
            get => _fallback;
            set => SetValue(ref _fallback, value);
        }

        /// <summary>
        /// 获取或设置当前引用属性的数据源
        /// </summary>
        [Alias("src")]
        public string Source
        {
            get => _source;
            set => SetValue(ref _source, value);
        }


        /// <summary>
        /// 获取或设置当前引用属性的多选项
        /// </summary>
        [Alias("multiSelect")]
        [Ignore]
        public bool MultiSelect
        {
            get => _multiSelect;
            set => SetValue(ref _multiSelect, value);
        }
    }
}