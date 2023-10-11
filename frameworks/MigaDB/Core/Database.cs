using System.Collections.Concurrent;
using System.Diagnostics;
using Acorisoft.FutureGL.MigaDB.Data;
using Acorisoft.FutureGL.MigaDB.Utils;

namespace Acorisoft.FutureGL.MigaDB.Core
{
    [DebuggerDisplay("{GetHashCode()}")]
    public class Database : Disposable, IDatabase, IObjectCollection, IDisposableCollector
    {
        private readonly string                   _databaseDirectory;
        private readonly string                   _databaseFileName;
        private readonly string                   _databaseIndexFileName;
        private readonly Dictionary<Type, object> _DataConsistencyCache;

        internal readonly LiteDatabase                  _database;
        internal readonly ILiteCollection<BsonDocument> _props;
        private readonly  DisposableCollector           _collector;
        private readonly  DatabaseMode                  _mode;

        
        public Database(LiteDatabase kernel, string root, string fileName, string indexFileName, DatabaseMode mode)
        {
            _DataConsistencyCache  = new Dictionary<Type, object>();
            _collector             = new DisposableCollector();
            _database              = kernel ?? throw new ArgumentNullException(nameof(kernel));
            _databaseDirectory     = root;
            _databaseFileName      = fileName;
            _databaseIndexFileName = indexFileName;
            _props                 = _database.GetCollection<BsonDocument>(Constants.PropertyCollectionName);
            _mode                  = mode;

            Property = Initialize();
            CheckPrimitiveProperty();
        }

        internal Database(LiteDatabase kernel)
        {
            _collector             = new DisposableCollector();
            _database              = kernel ?? throw new ArgumentNullException(nameof(kernel));
            _props                 = _database.GetCollection<BsonDocument>(Constants.PropertyCollectionName);
            _databaseDirectory     = AppDomain.CurrentDomain.BaseDirectory;
            _databaseFileName      = Path.Combine(_databaseDirectory, Constants.DatabaseFileName);
            _databaseIndexFileName = Path.Combine(_databaseDirectory, Constants.DatabaseIndexFileName);

            Property = Get<DatabaseProperty>();
            CheckPrimitiveProperty();
        }

        private void CheckPrimitiveProperty()
        {
            this.IfSet<DoubleProperty>(new DoubleProperty
            {
                Value = new Dictionary<string, double>()
            });
            
            this.IfSet<BooleanProperty>(new BooleanProperty
            {
                Value = new Dictionary<string, bool>()
            });
            
            this.IfSet<Int32Property>(new Int32Property
            {
                Value = new Dictionary<string, int>()
            });
            
            this.IfSet<StringProperty>(new StringProperty
            {
                Value = new Dictionary<string, string>()
            });
        }

        private DatabaseProperty Initialize()
        {
            if (_mode == DatabaseMode.Debug)
            {
                return Set<DatabaseProperty>(new DatabaseProperty
                {
                    Name = "DEBUG",
                    Author = "DEBUG",
                    ForeignName = "DEBUG",
                    Id = ID.Get()
                });
            }
            
            return Get<DatabaseProperty>();
        }

        private DatabaseVersion GetVersion()
        {
            if (_mode == DatabaseMode.Debug)
            {
                return this.IfSet<DatabaseVersion>(new DatabaseVersion
                {
                    TimeOfCreated = DateTime.Now,
                    TimeOfModified = DateTime.Now,
                    Version = 1
                });
            }
            
            return Get<DatabaseVersion>();
        }

        protected override void ReleaseManagedResources()
        {
            _database.Checkpoint();
            _database.Checkpoint();
            _database.Dispose();
            _collector.Dispose();
        }


        /// <summary>
        /// 获取数据库集合。
        /// </summary>
        /// <param name="collectionName">集合名</param>
        /// <typeparam name="T">数据类型。</typeparam>
        /// <returns>返回一个数据类型。</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ILiteCollection<T> GetCollection<T>(string collectionName) where T : class => _database.GetCollection<T>(collectionName);
        
        
        /// <summary>
        /// 删除数据库集合。
        /// </summary>
        /// <param name="collectionName">集合名</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DropCollection(string collectionName)  => _database.DropCollection(collectionName);

        /// <summary>
        /// 获取指定的集合是否存在。
        /// </summary>
        /// <param name="collectionName">集合名</param>
        /// <returns>返回指定的集合是否存在。</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Exists(string collectionName) => _database.CollectionExists(collectionName);

       
        
        /// <summary>
        /// 删除值。
        /// </summary>
        /// <typeparam name="T">值类型。</typeparam>
        /// <returns>获得值。</returns>
        public T Delete<T>() where T : class
        {
            var key = typeof(T).FullName;

            if (Has<T>())
            {
                var d = Get<T>();
                _props.Delete(key);
                return d;
            }
            
            return default(T);
        }

        /// <summary>
        /// 获得值。
        /// </summary>
        /// <typeparam name="T">值类型。</typeparam>
        /// <returns>获得值。</returns>
        public T Get<T>() where T : class
        {
            var keyA = typeof(T);
            var key  = keyA.FullName;
            
            if (_DataConsistencyCache.TryGetValue(typeof(T), out var v))
            {
                return (T)v;
            }
            
            
            var document = _props.FindById(key)?.AsDocument;
            var obj      = document is null ? default(T) : BsonMapper.Global.Deserialize<T>(document);
            
            //
            // add to cache
            v = obj;
            _DataConsistencyCache.TryAdd(keyA, v);
            return obj;
        }

        /// <summary>
        /// 删除值。
        /// </summary>
        /// <typeparam name="T">值类型。</typeparam>
        /// <returns>获得值是否存在，true为存在否则表示不存在。</returns>
        public bool Has<T>() where T : class
        {
            return _DataConsistencyCache.ContainsKey(typeof(T)) || 
                   _props.HasID(typeof(T).FullName);
        }
        
        
        /// <summary>
        /// 删除值。
        /// </summary>
        /// <typeparam name="T">值类型。</typeparam>
        /// <returns>获得值是否存在，true为存在否则表示不存在。</returns>
        public T Update<T>(T instance) where T : class
        {
            var keyA = typeof(T);
            var key  = keyA.FullName;
            if (instance is null)
                return default(T);
            
            if (_DataConsistencyCache.ContainsKey(keyA))
            {
                _DataConsistencyCache[keyA] = instance;
            }
            else
            {
                _DataConsistencyCache.TryAdd(keyA, instance);
            }
            
            var document = BsonMapper.Global
                                     .Serialize(instance)
                                     .AsDocument;
            document[Constants.LiteDB_IdField] = key;
            _props.Update(document);
            return instance;
        }

        /// <summary>
        /// 设置值。
        /// </summary>
        /// <param name="instance">实例</param>
        /// <typeparam name="T">值类型。</typeparam>
        /// <returns>返回这个值本身。</returns>
        public T Set<T>(T instance) where T : class
        {
            var keyA = typeof(T);
            var key  = keyA.FullName;
            if (instance is null)
                return default(T);

            if (_DataConsistencyCache.ContainsKey(keyA))
            {
                _DataConsistencyCache[keyA] = instance;
            }
            else
            {
                _DataConsistencyCache.TryAdd(keyA, instance);
            }

            var document = BsonMapper.Global
                                     .Serialize(instance)
                                     .AsDocument;
            document[Constants.LiteDB_IdField] = key;
            _props.Insert(document);
            return instance;
        }

        /// <summary>
        /// 升级版本。
        /// </summary>
        public void UpdateVersion(int version)
        {
            if (version == Version)
            {
                return;
            }

            //
            // Update
            var information = Get<DatabaseVersion>();
            information.Version        = version;
            information.TimeOfModified = DateTime.Now;

            //
            // Update
            Update(information);
        }

        /// <summary>
        /// 收集垃圾
        /// </summary>
        /// <param name="disposable"></param>
        public void Collect(IDisposable disposable)
        {
            _collector.Collect(disposable);
        }

        /// <summary>
        /// 获取当前数据库属性。
        /// </summary>
        public string DatabaseFileName => _databaseFileName;

        /// <summary>
        /// 获取当前数据库属性。
        /// </summary>
        public string DatabaseIndexFileName => _databaseIndexFileName;

        /// <summary>
        /// 获取当前数据库属性。
        /// </summary>
        public string DatabaseDirectory => _databaseDirectory;

        /// <summary>
        /// 获取当前数据库版本
        /// </summary>
        /// <remarks>操作为非缓存操作，避免数据不一致。</remarks>
        public int Version => GetVersion().Version;

        /// <summary>
        /// 获取当前数据库属性。
        /// </summary>
        public DatabaseProperty Property { get; }

        /// <summary>
        /// 
        /// </summary>
        ILiteCollection<BsonDocument> IObjectCollection.Props => _props;
    }
}