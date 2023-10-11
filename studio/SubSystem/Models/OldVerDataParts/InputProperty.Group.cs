using System.Xml.Linq;
using Acorisoft.Miga.Xml;

namespace Acorisoft.Miga.Doc.Parts
{
    [Alias("group")]
    public class GroupProperty2 : InputProperty2, IAddChild
    {
        protected sealed override InputProperty2 CreateInstanceOverride()
        {
            return new GroupProperty2();
        }

        protected override void ShadowCopy(InputProperty2 target)
        {
            var tp = (GroupProperty2)target;
            
            tp.Values.AddMany(Values);
            
            base.ShadowCopy(target);
        }
        
        
        protected internal override XElement GetElementOverride()
        {
            var element = new XElement("group");
            
            //
            //
            Write(element);
            
            foreach (var value in Values)
            {
                element.Add(value.GetElementOverride());
            }

            return element;
        }

        public void Accept(object instance)
        {
            Values.Add(instance as OptionProperty2);
        }
        
        /// <summary>
        /// 
        /// </summary>
        [Ignore]
        public ObservableCollection<OptionProperty2> Values { get; set; }= new ObservableCollection<OptionProperty2>();
    }
}