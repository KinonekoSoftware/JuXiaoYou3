using Acorisoft.FutureGL.MigaDB.Data;

namespace Acorisoft.FutureGL.MigaDB
{
    public static class Feature
    {
        public static LiteDatabase GetLiteDatabase(this IDatabase database)
        {
            return ((Database)database)._database;
        }

        public static void InternalSetName(this ModuleBlock block, string value)
        {
            block._name = value;
        }

        /// <summary>
        /// 获得布尔值
        /// </summary>
        /// <param name="database">数据库</param>
        /// <param name="key">键</param>
        /// <returns>返回结果</returns>
        public static bool Boolean(this IDatabase database, string key)
        {
            return database.Get<BooleanProperty>().Value
                           .TryGetValue(key, out var v) && v;
        }

        /// <summary>
        /// 设置布尔值
        /// </summary>
        /// <param name="database">数据库</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns>返回结果</returns>
        public static bool Boolean(this IDatabase database, string key, bool value)
        {
            var b = database.Get<BooleanProperty>();
            var d = b.Value;
            d[key] = value;

            database.Update(b);
            return value;
        }

        /// <summary>
        /// 获得数字值
        /// </summary>
        /// <param name="database">数据库</param>
        /// <param name="key">键</param>
        /// <returns>返回结果</returns>
        public static int? Int(this IDatabase database, string key)
        {
            return database.Get<Int32Property>().Value
                           .TryGetValue(key, out var v)
                ? v
                : null;
        }

        /// <summary>
        /// 设置数字值
        /// </summary>
        /// <param name="database">数据库</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns>返回结果</returns>
        public static int Int(this IDatabase database, string key, int value)
        {
            var b = database.Get<Int32Property>();
            var d = b.Value;
            d[key] = value;

            database.Update(b);
            return value;
        }

        /// <summary>
        /// 获得字符串
        /// </summary>
        /// <param name="database">数据库</param>
        /// <param name="key">键</param>
        /// <returns></returns>
        public static string String(this IDatabase database, string key)
        {
            return database.Get<StringProperty>().Value
                           .TryGetValue(key, out var v)
                ? v
                : null;
        }

        /// <summary>
        /// 设置字符串
        /// </summary>
        /// <param name="database">数据库</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns>返回结果</returns>
        public static string String(this IDatabase database, string key, string value)
        {
            var b = database.Get<StringProperty>();
            var d = b.Value;
            d[key] = value;

            database.Update(b);
            return value;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="database">数据库</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Upsert<T>(this IDatabase database,T instance) where T : class
        {
            
            return database.Has<T>() ? database.Update(instance) : database.Set(instance);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="database">数据库</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T IfSet<T>(this IDatabase database, T instance) where T : class
        {
            if (! database.Has<T>())
            {
                database.Set(instance);
                return instance;
            }

            return database.Get<T>();
        }

        public const string TextEditorFeatureMissing = "feature.TextEditor";
        public const int    CurrentDocumentVersion   = 0x10;
    }
}