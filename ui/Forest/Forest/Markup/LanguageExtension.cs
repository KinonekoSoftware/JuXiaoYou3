using System.Windows.Markup;
using Acorisoft.FutureGL.Forest.Services;
using Acorisoft.FutureGL.MigaUtils;

namespace Acorisoft.FutureGL.Forest.Markup
{
    public class LanguageExtension : MarkupExtension
    {
        public LanguageExtension(){}
        public LanguageExtension(string key) => Key = key;

        public LanguageExtension(object value)
        {
            Key = GetKeyFromObject(value);
        }

        public static string GetKeyFromObject(object value)
        {
            if (value is ITextService txtSrv)
            {
                var txtSrc = txtSrv.GetTextSource();
                return txtSrv.UseLanguageService() ?  Language.GetText(txtSrc) : txtSrc;
            }

            if (value is Enum @enum)
            {
                return Language.GetEnum(@enum);
            }

            return value?.ToString();
        }
        
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Language.GetText(Key);
        }
        
        public object Value { get; set; }
        public string Key { get; set; }
    }
}