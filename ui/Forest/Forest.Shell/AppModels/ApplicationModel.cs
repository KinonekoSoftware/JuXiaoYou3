using System.IO;
// ReSharper disable MemberCanBeMadeStatic.Local

namespace Acorisoft.FutureGL.Forest.AppModels
{
    public class ApplicationModel
    {
        public static string CheckDirectory(string directory)
        {
            if (!string.IsNullOrEmpty(directory) &&
                !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            return directory;
        }
        
        public ApplicationModel Initialize()
        {
            CheckDirectory(Logs);
            CheckDirectory(Settings);
            CheckDirectory(Feedbacks);
            return this;
        }
        
        /// <summary>
        /// 日志目录
        /// </summary>
        public string Logs { get; init; }
        
        /// <summary>
        /// 设置目录
        /// </summary>
        public string Settings { get; init; }
        
        /// <summary>
        /// 设置目录
        /// </summary>
        public string Feedbacks { get; init; }
    }
}