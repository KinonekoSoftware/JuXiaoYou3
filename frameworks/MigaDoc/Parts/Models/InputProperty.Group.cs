using System.Collections.ObjectModel;
using System.Xml.Linq;
using Acorisoft.Miga.Xml;

namespace Acorisoft.Miga.Doc.Parts
{
    [Alias("group")]
    public class GroupProperty : InputProperty, IAddChild
    {
        protected sealed override InputProperty CreateInstanceOverride()
        {
            return new GroupProperty();
        }

        protected override void ShadowCopy(InputProperty target)
        {
            var tp = (GroupProperty)target;
            
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
            Values.Add(instance as OptionProperty);
        }
        
        /// <summary>
        /// 
        /// </summary>
        [Ignore]
        public ObservableCollection<OptionProperty> Values { get; set; }= new ObservableCollection<OptionProperty>();
    }
}