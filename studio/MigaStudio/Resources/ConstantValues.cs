using System.Collections.Generic;
using Acorisoft.FutureGL.MigaDB.Core;

namespace Acorisoft.FutureGL.MigaStudio.Resources
{
    public class ConstantValues
    {
        public static IEnumerable<object> Themes => new object[] { MainTheme.Light, MainTheme.Dark };

        public static IEnumerable<object> Languages => new object[]
        {
            CultureArea.Chinese,
            CultureArea.English,
            CultureArea.Japanese,
            CultureArea.Russian,
            CultureArea.Korean,
            CultureArea.French
        };

        public static IEnumerable<object> DebugMode => new object[]
        {
            DatabaseMode.Release,
            DatabaseMode.Attached,
        };


        internal const string Setting_MainTheme = "setting.theme";
        internal const string Setting_DebugMode = "setting.debugMode";
        internal const string Setting_Language  = "setting.language";
        internal const string Setting_AutoSavePeriod  = "setting.AutoSavePeriod";
        internal const string PageName_Home     = "global.home";
        internal const string PageName_Setting  = "global.setting";
    }
}