using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace Acorisoft.FutureGL.Forest.Utils
{
    public class JSON
    {       
        private static readonly JsonSerializerSettings _settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All
        };
        
        public static T WriteSetting<T>(string fileName, T instance)
        {
            var json = JsonConvert.SerializeObject(instance, _settings);
            File.WriteAllText(fileName, json);
            return instance;
        }
        
        public static T OpenSetting<T>(string fileName, Func<T> factory)
        {
            if (File.Exists(fileName))
            {
                try
                {
                    var rawJsonContent = File.ReadAllText(fileName, Encoding.UTF8);
                    var instance = JsonConvert.DeserializeObject<T>(rawJsonContent, _settings);
                    instance ??= factory();
                    return instance;
                }
                catch
                {
                    return WriteSetting(fileName, factory());
                }
            }
            
            return WriteSetting(fileName, factory());
        }
    }
}