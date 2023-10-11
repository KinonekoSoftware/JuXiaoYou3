namespace Acorisoft.Miga.Doc.IO
{
    public class Resource
    {
        public string GetUri()
        {
            // miga://v1.img/param
            return $"miga://v{((int)Kind).ToString()}.img/{Location}";
        }
        //
        // public static Resource Parse(string literal) => ResourceScheme.Parse(literal);
        // public static readonly Resource Builtin_Missing = new Resource
        // {
        //     Location = "Missing",
        //     Kind     = ResourceKind.Builtin
        // };
        //
        internal string SchemeName { get; init; }
        
        /// <summary>
        /// 获取或设置资源路径
        /// </summary>
        public string Location { get; set; }
        
        /// <summary>
        /// 获取或设置资源路径
        /// </summary>
        public ResourceKind Kind { get; set; }
    }
}