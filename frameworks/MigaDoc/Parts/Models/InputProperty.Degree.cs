using System.Xml.Linq;
using Acorisoft.Miga.Xml;

namespace Acorisoft.Miga.Doc.Parts
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// <para><see cref="DegreeProperty"/>类型主要是为了避免频繁重复的设计ViewModel创建的大类型</para>
    /// </remarks>
    [Alias("degree")]
    public class DegreeProperty : InputProperty
    {
        private string _fallback;
        private string _positive;
        private string _negative;

        protected sealed override InputProperty CreateInstanceOverride()
        {
            return new DegreeProperty();
        }

        protected override void ShadowCopy(InputProperty target)
        {
            var tp = (DegreeProperty)target;
            
            tp._fallback  = _fallback;
            tp._negative = _negative;
            tp._positive = _positive;
            
            base.ShadowCopy(target);
        }
        
        
        protected internal override XElement GetElementOverride()
        {
            var element = new XElement("degree");
            
            //
            //
            Write(element);


            element.Add(new XAttribute("fallback", Fallback ?? string.Empty));
            element.Add(new XAttribute("p", Positive ?? string.Empty));
            element.Add(new XAttribute("n", Negative ?? string.Empty));

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
        [Alias("p")]
        public string Positive
        {
            get => _positive;
            set => SetValue(ref _positive, value);
        }

        [Alias("n")]
        public string Negative
        {
            get => _negative;
            set => SetValue(ref _negative, value);
        }
    }
}