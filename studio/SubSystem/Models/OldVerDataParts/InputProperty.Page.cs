using System.Xml.Linq;
using Acorisoft.Miga.Xml;

namespace Acorisoft.Miga.Doc.Parts
{
    [Alias("page")]
    public class PageProperty2 : InputProperty2, IFallbackSupport
    {
        private string _fallback;
        private string _unit;

        protected override void ShadowCopy(InputProperty2 target)
        {
            var tp = (PageProperty2)target;

            tp.Fallback = Fallback;
            tp.Unit     = Unit;

            base.ShadowCopy(target);
        }


        
        protected override InputProperty2 CreateInstanceOverride()
        {
            return new PageProperty2();
        }
        
        protected internal override XElement GetElementOverride()
        {
            var element = new XElement("page");
            
            //
            //
            Write(element);

            element.Add(new XAttribute("fallback", Fallback ?? string.Empty));
            element.Add(new XAttribute("unit", Unit ?? string.Empty));

            return element;
        }

        /// <summary>
        /// 获取或设置当前文本属性的回滚属性
        /// </summary>
        public string Fallback
        {
            get => _fallback;
            set => SetValue(ref _fallback, value);
        }

        /// <summary>
        /// 获取或设置当前文本属性的后缀
        /// </summary>
        [Alias("unit")]
        public string Unit
        {
            get => _unit;
            set => SetValue(ref _unit, value);
        }
    }
}