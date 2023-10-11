using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Acorisoft.FutureGL.Forest.AppModels;
using DryIoc;

namespace Acorisoft.FutureGL.MigaStudio.Core
{
    public interface ISubSystemModule
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ITabViewController GetController();
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="container"></param>
        void Initialize(CultureArea culture, IContainer container);

        /// <summary>
        /// 预览
        /// </summary>
        CultureArea Language { get; }


        /// <summary>
        /// 当前名字
        /// </summary>
        string FriendlyName { get; }
        string ModuleId { get; }
    }

    public abstract class SubSystemModule : ISubSystemModule
    {
        public abstract ITabViewController GetController();

        public void Initialize(CultureArea culture, IContainer container)
        {
            //
            // 设置语言
            Language     = culture;
            
            //
            // 设置名字
            FriendlyName = GetSubSystemName(culture);
            
            //
            // 注册语言
            var textSource = InstallLanguages(culture);

            //
            //
            Forest.Services
                  .Language
                  .AppendLanguageSource(textSource);

            //
            // 注册视图
            InstallView(container);

            //
            // 注册服务
            InstallServices(container);
        }

        protected abstract string GetSubSystemName(CultureArea language);

        protected IEnumerable<string> InstallLanguages(CultureArea culture)
        {
            var assembly       = Assembly.GetAssembly(GetType());
            var manifestStream = assembly?.GetManifestResourceStream(GetManifestFileStream(culture));
            
            if(manifestStream is null)
            {
                return Array.Empty<string>();
            }

            var languages = new List<string>();
            
            //
            //
            using (var sr = new StreamReader(manifestStream))
            {
                while (!sr.EndOfStream)
                {
                    languages.Add(sr.ReadLine());
                }
            }

            //
            //
            return languages;
        }

        private string GetManifestFileStream(CultureArea culture)
        {
            var p = GetLanguageFilePrefix();
            var s = culture switch
            {
                CultureArea.Chinese  => "cn.ini",
                CultureArea.Korean   => "kr.ini",
                CultureArea.Japanese => "jp.ini",
                CultureArea.Russian  => "ru.ini",
                CultureArea.English  => "en.ini",
                _                    => "fr.ini"
            };

            return $"{p}{s}";
        }

        protected abstract string GetLanguageFilePrefix();
        protected abstract void InstallView(IContainer container);
        protected abstract void InstallServices(IContainer container);
        public CultureArea Language { get; private set; }
        public string FriendlyName { get; private set; }
        public abstract string ModuleId { get; }
    }
}