namespace Acorisoft.Miga.Xml
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Property)]
    public class IgnoreAttribute : Attribute
    {
        
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Property)]
    public class AliasAttribute : Attribute
    {
        public AliasAttribute(){}
        public AliasAttribute(string alias) => Alias = alias;
        
        
        public string Alias { get; init; }
    }
    
    [AttributeUsage(AttributeTargets.Class , AllowMultiple = true)]
    public class ImportAttribute : Attribute
    {
        public ImportAttribute(){}
        public ImportAttribute(Type alias) => DataType = alias;
        
        
        public Type DataType { get; init; }
    }
    
    [AttributeUsage(AttributeTargets.Property)]
    public class TypeConverterAttribute : Attribute
    {
        public TypeConverterAttribute(Type converter) => Converter = converter;
        
        
        public Type Converter { get; init; }
    }
}