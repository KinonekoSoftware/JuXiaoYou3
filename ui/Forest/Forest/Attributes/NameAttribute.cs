namespace Acorisoft.FutureGL.Forest.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class NameAttribute : Attribute
    {
        public NameAttribute(){}
        public NameAttribute(string name) => Name = name;
        public string Name { get; init; }
    }
}