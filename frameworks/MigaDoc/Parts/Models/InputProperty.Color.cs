using System.Xml.Linq;
using Acorisoft.Miga.Xml;

namespace Acorisoft.Miga.Doc.Parts
{
    
    [Alias("color")]
    public class ColorProperty : InputProperty, IFallbackSupport
    {
        protected sealed override InputProperty CreateInstanceOverride()
        {
            return new ColorProperty();
        }

        protected override void ShadowCopy(InputProperty target)
        {
            var tp = (ColorProperty)target;
            
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