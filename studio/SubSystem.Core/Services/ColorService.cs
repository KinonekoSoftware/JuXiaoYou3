using System.Collections.Generic;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Data;
using DryIoc;

namespace Acorisoft.FutureGL.MigaStudio.Core
{
    public interface IColorService : IInMemoryDatabaseService
    {
        string GetColor(string keyword);
        void Changed(IEnumerable<string> items, string color);
        
        void AddOrUpdate(string name, string color);

        void Remove(string name);
    }
    
    public class ColorService : IColorService
    {
        private readonly Dictionary<string, string> _Color = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private          IDatabaseManager           _databaseManager;
        
        public string GetColor(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return "#007ACC";
            }

            return _Color.TryGetValue(keyword, out var color) ? color : "#007ACC";
        }

        public void Changed(IEnumerable<string> items, string color)
        {
            if (items is null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(color))
            {
                return;
            }

            foreach (var item in items)
            {
                AddOrUpdate(item, color);
            }
        }

        public void AddOrUpdate(string name, string color)
        {
            _Color[name] = color;
        }

        public void Remove(string name)
        {
            _Color.Remove(name);
        }

        public void Start(IDatabaseManager databaseManager)
        {
            _databaseManager = databaseManager ?? throw new InvalidOperationException();
            InvalidateDataSource();
        }

        public void InvalidateDataSource()
        {
            var db = _databaseManager.Database
                                     .CurrentValue;
            var p = db.Get<ColorServiceProperty>();
            
            TrackingVersion = p.Version;
            
            if (p is null)
            {
                return;
            }

            _Color.Clear();
            foreach (var mapping in p.Mappings)
            {
                foreach (var keyword in mapping.Keywords)
                {
                    _Color.TryAdd(keyword, mapping.Color);
                }
            }
        }

        public void Register(IContainer container)
        {
            container.Use<ColorService, IColorService>(this);
        }

        public void Unregister(IContainer container)
        {
            container.Unregister<IColorService>();
            container.Unregister<ColorService>();
        }

        public void Stop()
        {
            _Color.Clear();
        }
        
        public int TrackingVersion { get; private set; }
    }
}