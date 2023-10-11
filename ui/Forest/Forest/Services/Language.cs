using System.Diagnostics;
using System.IO;



// ReSharper disable InlineOutVariableDeclaration
// ReSharper disable SuspiciousTypeConversion.Global

namespace Acorisoft.FutureGL.Forest.Services
{
    public static class Language
    {
        private static readonly Dictionary<string, string> _stringDictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        
        #region DependencyProperties

        public static readonly DependencyProperty NameProperty = DependencyProperty.RegisterAttached(
            "NameOverride",
            typeof(string),
            typeof(Language),
            new PropertyMetadata(default(string)));

        public static readonly DependencyProperty RootProperty = DependencyProperty.RegisterAttached(
            "RootOverride",
            typeof(string),
            typeof(Language),
            new PropertyMetadata(default(string)));

        public static readonly DependencyProperty IdProperty = DependencyProperty.RegisterAttached(
            "IdOverride",
            typeof(string),
            typeof(Language),
            new PropertyMetadata(default(string)));

        #endregion

        #region Id

        /// <summary>
        /// 用于指定特定的键，如果不是必要请使用 <see cref="SetName"/> 代替。
        /// </summary>
        /// <param name="element">要添加的元素</param>
        /// <param name="id">值</param>
        /// <remarks>
        /// <para>默认情况下<see cref="IdProperty"/>存储的内容为: {root}.{name}</para>
        /// <para>但是您可以通过调用该方法指定值来覆盖默认设置。</para>
        /// </remarks>
        public static void SetId(FrameworkElement element, string id)
        {
            element.DataContextChanged += OnElementDataContextChanged_SetId;
            element.Unloaded           += OnElementUnloaded_SetId;

            element.SetValue(IdProperty, id);
        }

        /// <summary>
        /// 用于指定特定的键，如果不是必要请使用 <see cref="SetName"/> 代替。
        /// </summary>
        /// <param name="element">要获取的元素</param>
        /// <returns>返回用于翻译的元素键</returns>
        public static string GetId(FrameworkElement element)
        {
            return (string)element.GetValue(IdProperty);
        }

        
        private static void OnElementUnloaded_SetId(object sender, RoutedEventArgs e)
        {
            if (sender is not FrameworkElement element)
            {
                return;
            }
            
            element.DataContextChanged -= OnElementDataContextChanged_SetId;
            element.Unloaded           -= OnElementUnloaded_SetId;
        }

        private static void OnElementDataContextChanged_SetId(object sender, DependencyPropertyChangedEventArgs e)
        {
            var element = (FrameworkElement)sender;
            var id      = GetId(element);
            
            //
            // Find RootName
            //
            var    adapter = element as ITextResourceAdapter ?? Xaml.Get<ITextResourceFactory>().Adapt(element);
            var    toolKey = $"{id}.tips";
            string text;

            if (GlobalStrings.TryGetValue(id, out text))
            {
                adapter.SetText(text);
            }

            if (GlobalStrings.TryGetValue(toolKey, out text))
            {
                adapter.SetToolTips(text);
            }
        }
        
        #endregion

        #region Root

        public static void SetRoot(FrameworkElement element, string value)
        {
            element.DataContextChanged += OnElementDataContextChanged_SetRoot;
            element.Unloaded           += OnElementUnloaded_SetRoot;

            element.SetValue(RootProperty, value);
        }

        public static string GetRoot(FrameworkElement element)
        {
            return (string)element.GetValue(RootProperty);
        }

        
        
        
        private static void OnElementUnloaded_SetRoot(object sender, RoutedEventArgs e)
        {
            if (sender is not FrameworkElement element)
            {
                return;
            }
            
            element.DataContextChanged -= OnElementDataContextChanged_SetRoot;
            element.Unloaded           -= OnElementUnloaded_SetRoot;
        }

        private static void OnElementDataContextChanged_SetRoot(object sender, DependencyPropertyChangedEventArgs e)
        {
            var element = (FrameworkElement)sender;
            var value   = GetRoot(element);
            
            //
            // Find RootName
            //
            if (e.NewValue is IViewModelLanguageService vmls)
            {
                vmls.RootName = value;
            }
        }
        
        #endregion

        #region GetText

        

        public static string GetText(string id)
        {
            #if DEBUG
            if (GlobalStrings.TryGetValue(id, out var text))
            {
                return text;
            }

            Debug.WriteLine($"本地化文本不存在，键:{id}");
            return id;
#else

            return GlobalStrings.TryGetValue(id, out var text) ? text : id;
#endif
        }

        public static string GetEnum<T>(string prefix, T value) where T : Enum
        {
            var key = $"enum.{prefix}.{value}";
            return GetText(key);
        }
        
        public static string GetEnum(string prefix, Enum value)
        {
            var key = $"enum.{prefix}.{value}";
            return GetText(key);
        }
        
        public static string GetEnum(Enum value)
        {
            var key = $"enum.{value.GetType().Name}.{value}";
            return GetText(key);
        }
        
        public static string GetEnum<T>(T value) where T : Enum
        {
            var key = $"enum.{typeof(T).Name}.{value}";
            return GetText(key);
        }

        public static string GetTypeName(Type type)
        {
            var key = $"text.{type.Name}";
            return GetText(key);
        }

        #endregion
        
        #region Name

        /// <summary>
        /// 用于指定特定的键，如果不是必要请使用 <see cref="SetName"/> 代替。
        /// </summary>
        /// <param name="element">要添加的元素</param>
        /// <param name="value">值</param>
        /// <remarks>
        /// <para>默认情况下<see cref="IdProperty"/>存储的内容为: {root}.{name}</para>
        /// <para>但是您可以通过调用该方法指定值来覆盖默认设置。</para>
        /// </remarks>
        public static void SetName(FrameworkElement element, string value)
        {
            element.DataContextChanged += OnElementDataContextChanged_SetName;
            element.Unloaded           += OnElementUnloaded_SetName;

            element.SetValue(NameProperty, value);
        }

        private static void OnElementUnloaded_SetName(object sender, RoutedEventArgs e)
        {
            if (sender is not FrameworkElement element)
            {
                return;
            }
            
            element.DataContextChanged -= OnElementDataContextChanged_SetName;
            element.Unloaded           -= OnElementUnloaded_SetName;
        }

        private static void OnElementDataContextChanged_SetName(object sender, DependencyPropertyChangedEventArgs e)
        {
            var element = (FrameworkElement)sender;
            var value   = GetName(element);
            
            //
            // Find RootName
            //
            if (e.NewValue is IViewModelLanguageService vmls)
            {
                var id = $"{vmls.RootName}.{value}";
                var adapter = element as ITextResourceAdapter ?? Xaml.Get<ITextResourceFactory>().Adapt(element);
                var textKey = $"{id}";
                var toolKey = $"{id}.tips";
                string text;

                if (GlobalStrings.TryGetValue(textKey, out text))
                {
                    adapter.SetText(text);
                }

                if (GlobalStrings.TryGetValue(toolKey, out text))
                {
                    adapter.SetToolTips(text);
                }

                element.SetValue(IdProperty, id);
            }
        }

        /// <summary>
        /// 用于指定特定的键，如果不是必要请使用 <see cref="SetName"/> 代替。
        /// </summary>
        /// <param name="element">要获取的元素</param>
        /// <returns>返回用于翻译的元素键</returns>
        public static string GetName(FrameworkElement element)
        {
            return (string)element.GetValue(NameProperty);
        }

        #endregion

        #region LanguageSource

        private static void AppendTextSource(Dictionary<string, string> temp, IEnumerable<string> lines)
        {
            foreach (var line in lines)
            {                   
                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }
                var separator = line.IndexOf('=');
                var id        = line[..separator].Trim();
                var value = line[(separator + 1)..].Trim()
                                                   .Replace("\\t", "\x20\x20")
                                                   .Replace("\\n", "\n");

                temp.TryAdd(id, value);
            }
        }
        
        
        /// <summary>
        /// 设置语言
        /// </summary>
        /// <param name="fileName">文件名</param>
        public static void SetLanguageSource(string fileName)
        {
            // pageRoot.Id.Function
            try
            {
                var lines = File.ReadAllLines(fileName);
                _stringDictionary.Clear();
                AppendTextSource(_stringDictionary, lines);
            }
            catch
            {
                //
            }
        }

        /// <summary>
        /// 添加语言
        /// </summary>
        /// <param name="fileName">文件名</param>
        public static void AppendLanguageSource(string fileName)
        {
            // pageRoot.Id.Function
            try
            {
                var lines = File.ReadAllLines(fileName);

                AppendTextSource(_stringDictionary, lines);
            }
            catch
            {
                //
            }
        }
        
        /// <summary>
        /// 添加语言
        /// </summary>
        /// <param name="textSource">所有数据源</param>
        public static void AppendLanguageSource(IEnumerable<string> textSource)
        {
            // pageRoot.Id.Function
            try
            {

                foreach (var line in textSource)
                {                   
                    if (string.IsNullOrEmpty(line))
                    {
                        continue;
                    }
                    var separator = line.IndexOf('=');
                    var id        = line[..separator].Trim();
                    var value     = line[(separator + 1)..].Trim();

                    _stringDictionary.TryAdd(id, value);
                }
            }
            catch
            {
                //
            }
        }

        #endregion

        #region Language Properties

        

        
        /// <summary>
        /// 全局内容文本
        /// </summary>
        public static IReadOnlyDictionary<string, string> GlobalStrings => _stringDictionary;

        /// <summary>
        /// 指定文化
        /// </summary>
        /// <remarks>语言的切换必须重启。</remarks>
        public static CultureArea Culture { get; set; }
        
        #endregion

        /// <summary>
        /// 确定
        /// </summary>
        public static string ConfirmText
        {
            get
            {
                return Culture switch
                {
                    CultureArea.English => "Ok",
                    CultureArea.French => "fr.ini",
                    CultureArea.Japanese => "jp.ini",
                    CultureArea.Korean => "kr.ini",
                    CultureArea.Russian => "ru.ini",
                    _ => "确定"
                };
            }
        }
        
        /// <summary>
        /// 确定
        /// </summary>
        public static string ItemDuplicatedText
        {
            get
            {
                return Culture switch
                {
                    CultureArea.English  => "Item duplicated",
                    CultureArea.French   => "fr.ini",
                    CultureArea.Japanese => "jp.ini",
                    CultureArea.Korean   => "kr.ini",
                    CultureArea.Russian  => "ru.ini",
                    _                    => "选项重复！"
                };
            }
        }
        
        /// <summary>
        /// 确定
        /// </summary>
        public static string ContentDuplicatedText
        {
            get
            {
                return Culture switch
                {
                    CultureArea.English  => "Ok",
                    CultureArea.French   => "fr.ini",
                    CultureArea.Japanese => "jp.ini",
                    CultureArea.Korean   => "kr.ini",
                    CultureArea.Russian  => "ru.ini",
                    _                    => "内容重复！"
                };
            }
        }
        
        /// <summary>
        /// 确定
        /// </summary>
        public static string CompleteText
        {
            get
            {
                return Culture switch
                {
                    CultureArea.English  => "Complete",
                    CultureArea.French   => "fr.ini",
                    CultureArea.Japanese => "jp.ini",
                    CultureArea.Korean   => "kr.ini",
                    CultureArea.Russian  => "ru.ini",
                    _                    => "完成"
                };
            }
        }

        /// <summary>
        /// 拒绝
        /// </summary>
        public static string RejectText
        {
            get
            {
                return Culture switch
                {
                    CultureArea.English => "Reject",
                    CultureArea.French => "fr.ini",
                    CultureArea.Japanese => "jp.ini",
                    CultureArea.Korean => "kr.ini",
                    CultureArea.Russian => "ru.ini",
                    _ => "拒绝"
                };
            }
        }
        
        /// <summary>
        /// 拒绝
        /// </summary>
        public static string NotifyText
        {
            get
            {
                return Culture switch
                {
                    CultureArea.English  => "Notify",
                    CultureArea.French   => "fr.ini",
                    CultureArea.Japanese => "jp.ini",
                    CultureArea.Korean   => "kr.ini",
                    CultureArea.Russian  => "ru.ini",
                    _                    => "提示"
                };
            }
        }

        /// <summary>
        /// 拒绝
        /// </summary>
        public static string CancelText
        {
            get
            {
                return Culture switch
                {
                    CultureArea.English => "Cancel",
                    CultureArea.French => "Annuler",
                    CultureArea.Japanese => "取り消す",
                    CultureArea.Korean => "취소",
                    CultureArea.Russian => "Отмена",

                    _ => "放弃"
                };
            }
        }
        
        /// <summary>
        /// 拒绝
        /// </summary>
        public static string RemoveItemText
        {
            get
            {
                return Culture switch
                {
                    CultureArea.English  => "Are you sure you want to delete this data?",
                    CultureArea.French   => "Êtes-vous sûr de vouloir supprimer ces données?",
                    CultureArea.Japanese => "このデータを削除することは確実ですか?",
                    CultureArea.Korean   => "이 데이터를 삭제하시겠습니까?",
                    CultureArea.Russian  => "Вы уверены, что хотите удалить данные?",

                    _ => "你确定要删除这项数据吗？"
                };
            }
        }
        
        /// <summary>
        /// 拒绝
        /// </summary>
        public static string RemoveAllItemText
        {
            get
            {
                return Culture switch
                {
                    CultureArea.English  => "Are you sure you want to delete all your data?",
                    CultureArea.French   => "Êtes-vous sûr de vouloir supprimer toutes les données?",
                    CultureArea.Japanese => "全てのデータを削除することは確実ですか?",
                    CultureArea.Korean   => "모든 데이터를 삭제하시겠습니까?",
                    CultureArea.Russian  => "Вы уверены, что хотите удалить все данные?",

                    _ => "你确定要删除全部数据吗？"
                };
            }
        }
        
        /// <summary>
        /// 新建
        /// </summary>
        public static string NewText
        {
            get
            {
                return Culture switch
                {
                    CultureArea.English  => "Create",
                    CultureArea.French   => "Nouvelles",
                    CultureArea.Japanese => "新築です",
                    CultureArea.Korean   => "신축",
                    CultureArea.Russian  => "нов",

                    _ => "新建"
                };
            }
        }
        
        /// <summary>
        /// 打开
        /// </summary>
        public static string OpenText
        {
            get
            {
                return Culture switch
                {
                    CultureArea.English  => "Open",
                    CultureArea.French   => "Ouvrir",
                    CultureArea.Japanese => "開けます",
                    CultureArea.Korean   => "열어",
                    CultureArea.Russian  => "откр",
                    _                    => "打开"
                };
            }
        }
        
        /// <summary>
        /// 拒绝
        /// </summary>
        public static string SaveText
        {
            get
            {
                return Culture switch
                {
                    CultureArea.English  => "Save",
                    CultureArea.French   => "La conservation",
                    CultureArea.Japanese => "保存します",
                    CultureArea.Korean   => "저장",
                    CultureArea.Russian  => "сохран",
                    _                    => "保存"
                };
            }
        }
    }
}