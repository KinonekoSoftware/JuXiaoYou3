namespace Acorisoft.FutureGL.Forest.Models
{
    public class BasicAppSetting
    {
        /// <summary>
        /// 主题色
        /// </summary>
        public int Theme{ get; set; }

        /// <summary>
        /// 语言
        /// </summary>
        public CultureArea Language { get; set; }

        public static string GetName(CultureArea value)
        {
            return value switch
            {
                CultureArea.English  => "English",
                CultureArea.French   => "Français",
                CultureArea.Japanese => "日本語",
                CultureArea.Korean   => "한국어",
                CultureArea.Russian  => "Русский",
                _                    => "简体中文"
            };
        }

        public static string GetFileName(string dir, CultureArea value)
        {
            var fileName = value switch
            {
                CultureArea.English  => "en.ini",
                CultureArea.French   => "fr.ini",
                CultureArea.Japanese => "jp.ini",
                CultureArea.Korean   => "kr.ini",
                CultureArea.Russian  => "ru.ini",
                _                    => "cn.ini",
            };

            return Path.Combine(dir, fileName);
        }
    }
    
    public enum CultureArea
        {
            /// <summary>
            /// 简中
            /// </summary>
            Chinese,
            
            /// <summary>
            /// 韩语
            /// </summary>
            Korean,
            
            /// <summary>
            /// 日语
            /// </summary>
            Japanese,
            
            /// <summary>
            /// 俄语
            /// </summary>
            Russian,
            
            /// <summary>
            /// 英语
            /// </summary>
            English,
            
            /// <summary>
            /// 法语
            /// </summary>
            French,
            
            
        }
}