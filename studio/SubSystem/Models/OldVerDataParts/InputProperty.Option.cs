using System.Xml.Linq;
using Acorisoft.Miga.Xml;

namespace Acorisoft.Miga.Doc.Parts
{
    public enum OptionKind
    {
        Radio,
        Toggle,
        Favorite,
        Opposite
    }
    
    [Alias("option")]
    public class OptionProperty2 : InputProperty2, IFallbackSupport
    {
        protected sealed override InputProperty2 CreateInstanceOverride()
        {
            return new OptionProperty2();
        }

        protected override void ShadowCopy(InputProperty2 target)
        {
            var tp = (OptionProperty2)target;
            
            tp._fallback     = _fallback;
            tp.PositiveValue = PositiveValue;
            tp.NegativeValue = NegativeValue;
            tp.Kind          = Kind;
            
            base.ShadowCopy(target);
        }
        
         
        protected internal override XElement GetElementOverride()
        {
            var element = new XElement("option");
            
            //
            //
            Write(element);

            element.Add(new XAttribute("fallback", Fallback ?? string.Empty));
            element.Add(new XAttribute("p-val", PositiveValue ?? string.Empty));
            element.Add(new XAttribute("n-val", NegativeValue ?? string.Empty));
            element.Add(new XAttribute("kind", Kind));

            return element;
        }


        private string _fallback;

        /// <summary>
        /// 获取或设置当前文本属性的回滚属性
        /// </summary>
        public string Fallback
        {
            get => _fallback;
            set => SetValue(ref _fallback, value);
        }

        /// <summary>
        /// 数据。
        /// </summary>
        [Alias("p-val")]
        public string PositiveValue { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [Alias("n-val")]
        public string NegativeValue { get; set; }
        
        /// <summary>
        /// 选项
        /// </summary>
        [Alias("kind")]
        public OptionKind Kind { get; set; }
    }
}