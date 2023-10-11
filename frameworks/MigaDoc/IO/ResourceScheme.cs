namespace Acorisoft.Miga.Doc.IO
{
    internal class ResourceScheme
    {
        // public static Resource Parse(string src)
        // {
        //     if (string.IsNullOrEmpty(src))
        //     {
        //         return Resource.Builtin_Missing;
        //     }
        //     
        //     // miga://v1.img/param
        //     return src.Length < 14 ? Resource.Builtin_Missing : ParseImpl(src);
        // }
        //
        // internal static Resource ParseImpl(string src)
        // {
        //     if (string.IsNullOrEmpty(src))
        //     {
        //         return Resource.Builtin_Missing;
        //     }
        //     
        //     // miga://v1.img/param
        //     if (src.Length < 14)
        //     {
        //         return Resource.Builtin_Missing;
        //     }
        //
        //     var schemeName = src[..4];
        //     var schemeVerPrefix = src[7];
        //     var schemeVer = int.TryParse(src.AsSpan(8,1), out var val) ? val : 0;
        //     var param = src[14..];
        //
        //     // ReSharper disable once MergeIntoLogicalPattern
        //     if (schemeVerPrefix != 'v' && schemeVerPrefix != 'V')
        //     {
        //         return Resource.Builtin_Missing;
        //     }
        //
        //     if (schemeVer is < 0 or > 3)
        //     {
        //         return Resource.Builtin_Missing;
        //     }
        //     
        //
        //     return schemeName.EqualsWithIgnoreCase("miga") ? ParseImpl(schemeName, (ResourceKind)schemeVer, param) : Resource.Builtin_Missing;
        // }
        //
        // internal static Resource ParseImpl(string name, ResourceKind kind, string param)
        // {
        //     return new Resource
        //     {
        //         Kind     = kind,
        //         Location = param,
        //         SchemeName = name
        //     };
        // }
    }
}