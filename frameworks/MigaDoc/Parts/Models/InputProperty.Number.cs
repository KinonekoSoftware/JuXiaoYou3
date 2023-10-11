using System.Xml.Linq;
using Acorisoft.Miga.Xml;

namespace Acorisoft.Miga.Doc.Parts
{
    [Alias("number")]
    public class NumberProperty : InputProperty, IFallbackSupport
    {
        private string _fallback;
        private string _unit;

        protected sealed override InputProperty CreateInstanceOverride()
        {
            return new NumberProperty();
        }

        protected override void ShadowCopy(InputProperty target)
        {
            var tp = (NumberProperty)target;

            tp._fallback = _fallback;
            tp._unit     = _unit;

            base.ShadowCopy(target);
        }

        protected internal override XElement GetElementOverride()
        {
            var element = new XElement("number");

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