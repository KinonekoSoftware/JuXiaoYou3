using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using Acorisoft.Miga.Xml;

namespace Acorisoft.Miga.Doc.Parts
{
    [Alias("sequence")]
    public class SequenceProperty : InputProperty, IAddChild, IFallbackSupport
    {
        private string _fallback;
        
        protected sealed override InputProperty CreateInstanceOverride()
        {
            return new SequenceProperty();
        }

        protected override void ShadowCopy(InputProperty target)
        {
            var tp = (SequenceProperty)target;

            tp.Fallback = Fallback;
            tp.Values.AddMany(Values);

            base.ShadowCopy(target);
        }
        
        protected internal override XElement GetElementOverride()
        {
            var element = new XElement("sequence");
            //
            //
            Write(element);


            element.Add(new XAttribute("fallback", Fallback ?? string.Empty));

            foreach (var value in Values)
            {
                var val = new XElement("value")
                {
                    Value = value.Text
                };
                element.Add(val);
            }
            
            return element;
        }
        
        public override bool IsCompleted()
        {
            return base.IsCompleted() && Values.Count > 0 && Values.All(x => !string.IsNullOrEmpty(x.Text));
        }

        public void Accept(object instance)
        {
            Values.Add(instance as Value);
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
        /// 
        /// </summary>
        public ObservableCollection<Value> Values { get; set; }= new ObservableCollection<Value>();
    }
}