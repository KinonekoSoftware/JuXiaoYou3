namespace Acorisoft.FutureGL.Forest.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class ConnectedAttribute : Attribute
    {
        public Type View { get; init; }

        public Type ViewModel { get; init; }
    }
    
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class TitleAttribute : Attribute
    {
        public string Group { get; set; }

        public bool UseResourceKey { get; set; }

        public string Name { get; set; }
    }
}