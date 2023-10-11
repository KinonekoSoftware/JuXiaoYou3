namespace Acorisoft.FutureGL.MigaTest.Reflections
{
    public class ClassContext
    {
        public static ClassContext FromType(Type type)
        {
            var fields  = type.GetFields();
            var props   = type.GetProperties();
            var methods = type.GetMethods();


            var pi = props.Select(x => new PropertyContext(x))
                          .ToList();
            var fi = fields.Select(x => new FieldContext(x))
                           .ToList();
            var mi = methods.Select(x => new MethodContext(x))
                            .ToList();

            return new ClassContext(type, pi, fi, mi);
        }

        private ClassContext(Type type, List<PropertyContext> p, List<FieldContext> f, List<MethodContext> m)
        {
            ClassType  = type;
            Properties = p.AsReadOnly();
            Fields     = f.AsReadOnly();
            Methods    = m.AsReadOnly();
        }

        public void Initialize()
        {
            if (!Initialized)
            {
                Initialized = true;
                Instance    = Activator.CreateInstance(ClassType);
            }
        }
        
        public bool Initialized { get; private set; }
        public object Instance { get; private set; }
        public Type ClassType { get; }
        public IReadOnlyList<PropertyContext> Properties { get; }
        public IReadOnlyList<FieldContext> Fields { get; }
        public IReadOnlyList<MethodContext> Methods { get; }
    }
}