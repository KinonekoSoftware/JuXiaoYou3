using LocalPropertyResolvers = System.Collections.Generic.Dictionary<string, Acorisoft.Miga.Xml.PropertyResolver>;

namespace Acorisoft.Miga.Xml
{
    public class ClassResolver : Resolver
    {
        private readonly LocalPropertyResolvers _localResolver;
        private readonly Func<object> _expression;

        public ClassResolver(LocalPropertyResolvers resolvers, Func<object> expression)
        {
            _localResolver = resolvers ?? throw new ArgumentNullException(nameof(resolvers));
            _expression = expression ?? throw new ArgumentNullException(nameof(expression));
        }

        public object New()
        {
            return _expression();
        }

        public bool GetResolver(string name, out PropertyResolver resolver)
        {
            return _localResolver.TryGetValue(name, out resolver);
        }
    }
}