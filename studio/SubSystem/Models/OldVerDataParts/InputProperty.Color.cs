using System.Xml.Linq;
using Acorisoft.Miga.Xml;

namespace Acorisoft.Miga.Doc.Parts
{
    
    [Alias("color")]
    public class ColorProperty2 : InputProperty2, IFallbackSupport
    {
        protected sealed override InputProperty2 CreateInstanceOverride()
        {
            return new ColorProperty2();
        }

        protected override void ShadowCopy(InputProperty2 target)
        {
            var tp = (ColorProperty2)target;
            
            tp._fallback = _fallback;
            
            base.ShadowCopy(target);
        }
        
        
        protected internal override XElement GetElementOverride()
        {
            var element = new XElement("color");
            
            //
            //
            Write(element);

            element.Add(new XAttribute("fallback", Fallback ?? string.Empty));

            return element;
        }

        private string _fallback;

        /// <summary>
        /// 获取或设置 <see cref="Fallback"/> 属性。
        /// </summary>
        public string Fallback
        {
            get => _fallback;
            set => SetValue(ref _fallback, value);
        }
    }
}