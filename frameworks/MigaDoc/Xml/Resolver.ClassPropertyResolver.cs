using PropertySetter = Acorisoft.Miga.Utils.PropertySetter;

namespace Acorisoft.Miga.Xml
{
    public class ClassPropertyResolver : PropertyResolver
    {
        protected ClassPropertyResolver()
        {
            
        }
        
        public ClassPropertyResolver(ClassResolver resolver, string pn)
        {
            IndeedName = pn;
            Resolver = resolver;
        }

        public override void Assign(PropertySetter proxy, object targetInstance, string value)
        {
            var instance = Resolver.New();
            proxy[IndeedName, targetInstance] = instance;
        }

        public override void Assign(PropertySetter proxy, object targetInstance, object value)
        {
            proxy[IndeedName, targetInstance] = value;
        }
        
        public ClassResolver Resolver { get; }
    }

    public class ConverterPropertyResolver : ClassPropertyResolver
    {
        public ConverterPropertyResolver(CustomConverter converter, string pn)
        {
            Converter = converter;
            IndeedName = pn;
        }
        
        
        public override void Assign(PropertySetter proxy, object targetInstance, string value)
        {
            //
            // Use This
            Converter.Assign(proxy, targetInstance, value);
        }

        public override void Assign(PropertySetter proxy, object targetInstance, object value)
        {
            proxy[IndeedName, targetInstance] = value;
        }
        
        public CustomConverter Converter { get; }
    }
    
    public class CustomConverter
    {
        public virtual void Assign(PropertySetter proxy, object targetInstance, string value)
        {
            //
            // Use This
            
        }
    }
}