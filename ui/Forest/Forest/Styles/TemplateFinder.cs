namespace Acorisoft.FutureGL.Forest.Styles
{
    public interface ITemplatePartFinder
    {
        /// <summary>
        /// 
        /// </summary>
        void Find();
        
        /// <summary>
        /// 
        /// </summary>
        Func<string, DependencyObject> GetTemplatePart { get; }
    }

    internal class Finder : ITemplatePartFinder
    {
        public Finder(Func<string, DependencyObject> expr)
        {
            GetTemplatePart = expr;
            Expressions     = new List<Action>();
        }

        public void Find()
        {
            foreach (var expression in Expressions)
            {
                expression();
            }
            
            Expression?.Invoke();
        }

        /// <summary>
        /// 
        /// </summary>
        public Func<string, DependencyObject> GetTemplatePart { get; }
        
        /// <summary>
        /// 
        /// </summary>
        public List<Action> Expressions { get; }
        
        /// <summary>
        /// 
        /// </summary>
        public Action Expression { get; set; }
    }
    
    public static class TemplatePartFinder
    {
        public static ITemplatePartFinder Find<TControl>(this ITemplatePartFinder finder, string name, Action<TControl> callback) where TControl : DependencyObject
        {
            var impl = (Finder)finder;

            void FindExpr()
            {
                var control = (TControl)finder.GetTemplatePart(name);
                callback(control);
            }

            impl.Expressions.Add((Action)FindExpr);

            return finder;
        }

        public static ITemplatePartFinder Done(this ITemplatePartFinder finder, Action expr)
        {
            var impl = (Finder)finder;

            impl.Expression = expr;

            return impl;
        }
    }
}