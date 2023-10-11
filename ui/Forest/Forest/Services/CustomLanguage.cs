using System.IO;

namespace Acorisoft.FutureGL.Forest.Services
{
    public class CustomLanguage
    {
        private readonly Dictionary<string, string> _stringDictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        
        /// <summary>
        /// 设置语言
        /// </summary>
        /// <param name="fileName">文件名</param>
        public void SetLanguage(string fileName)
        {
            // pageRoot.Id.Function
            try
            {
                var lines = File.ReadAllLines(fileName);
                var temp  = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

                foreach (var line in lines)
                {
                    if (string.IsNullOrEmpty(line))
                    {
                        continue;
                    }
                    
                    var separator = line.IndexOf('=');
                    var id        = line[..separator].Trim();
                    var value     = line[(separator + 1)..].Trim();

                    temp.TryAdd(id, value);
                }

                _stringDictionary.Clear();

                foreach (var kv in temp)
                {
                    _stringDictionary.Add(kv.Key, kv.Value);
                }
            }
            catch
            {
                //
            }
        }
        
        
        /// <summary>
        /// 全局内容文本
        /// </summary>
        public IReadOnlyDictionary<string, string> GlobalStrings => _stringDictionary;
    }
}