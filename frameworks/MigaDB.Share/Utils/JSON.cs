using System.Text;
using Newtonsoft.Json;

namespace Acorisoft.FutureGL.MigaDB.Utils
{
    public static class JSON
    {
        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All
        };
        
        public static T FromFile<T>(string fileName)
        {
            var rawJsonContent = File.ReadAllText(fileName, Encoding.UTF8);
            var instance = JsonConvert.DeserializeObject<T>(rawJsonContent, Settings);
            return instance;
        }
        
        public static async Task<T> FromFileAsync<T>(string fileName)
        {
            var rawJsonContent = await File.ReadAllTextAsync(fileName, Encoding.UTF8);
            var instance = JsonConvert.DeserializeObject<T>(rawJsonContent, Settings);
            return instance;
        }
        
        public static T FromJson<T>(string json)
        {
            var instance = JsonConvert.DeserializeObject<T>(json, Settings);
            return instance;
        }

        public static void ToFile<T>(T instance, string fileName)
        {
            var json = JsonConvert.SerializeObject(instance, Settings);
            File.WriteAllText(fileName, json);
        }
        
        public static string Serialize<T>(T instance)
        {
            var json = JsonConvert.SerializeObject(instance, Settings);
            return json;
        }
        
        public static async Task ToFileAsync<T>(T instance, string fileName)
        {
            var json = JsonConvert.SerializeObject(instance, Settings);
            await File.WriteAllTextAsync(fileName, json);
        }
    }
}