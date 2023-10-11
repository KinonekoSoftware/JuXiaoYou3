using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using Acorisoft.FutureGL.MigaUtils.Collections;
using DynamicData;

namespace Acorisoft.FutureGL.MigaDB.Utils
{
    /// <summary>
    /// <see cref="IncludeSet"/> 类型表示一个能过获取关系的级联表。
    /// </summary>
    [SuppressMessage("ReSharper", "MemberCanBeMadeStatic.Global")]
    public class IncludeSet
    {
        public class IncludeNode
        {
            public IncludeNode()
            {
                Map         = new Dictionary<string, IncludeNode>();
                ChildrenMap = new ReadOnlyDictionary<string, IncludeNode>(Map);
            }

            internal Dictionary<string, IncludeNode> Map { get; }
            public IEnumerable<IncludeNode> Children => Map.Values;
            public IReadOnlyDictionary<string, IncludeNode> ChildrenMap { get; }
            public bool IsRoot { get; init; }
            public int Height { get; init; }
            public string Key { get; init; }
            public IncludeNode Root { get; init; }
            public IncludeNode Parent { get; init; }
        }

        private readonly object                          _sync;
        private readonly Dictionary<string, IncludeNode> _global;
        private readonly Dictionary<string, IncludeNode> _root;

        public IncludeSet()
        {
            _sync   = new object();
            _global = new Dictionary<string, IncludeNode>();
            _root   = new Dictionary<string, IncludeNode>();
        }

        public bool Add(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return false;
            }

            lock (_sync)
            {
                if (_global.ContainsKey(id))
                {
                    return false;
                }

                var root = new IncludeNode
                {
                    IsRoot = true,
                    Root   = null,
                    Parent = null,
                    Height = 1,
                    Key    = id
                };

                _root.Add(id, root);
                _global.Add(id, root);
                return true;
            }
        }
        
        public bool Add(string owner, string id)
        {
            if (string.IsNullOrEmpty(owner))
            {
                return false;
            }

            if (string.IsNullOrEmpty(id))
            {
                return false;
            }

            lock (_sync)
            {
                if (!_global.TryGetValue(owner, out var parent))
                {
                    return false;
                }

                IncludeNode node;

                if (parent.IsRoot)
                {
                    node = new IncludeNode
                    {
                        Root   = parent,
                        Parent = parent,
                        Height = parent.Height + 1,
                        Key    = id
                    };
                }
                else
                {
                    node = new IncludeNode
                    {
                        Root   = parent.Root,
                        Parent = parent,
                        Height = parent.Height + 1,
                        Key    = id
                    };
                }

                parent.Map.Add(id, node);
                _global.Add(id, node);
                return true;
            }
        }

        public bool Remove(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return false;
            }

            lock (_sync)
            {
                if (!_global.TryGetValue(id, out var node))
                {
                    return false;
                }

                if (node is null)
                {
                    return false;
                }

                if (node.IsRoot)
                {
                    RemoveRoot(id, node);
                }
                else
                {
                    RemoveNode(id, node);
                }

                return true;
            }
        }

        public void Clear()
        {
            lock (_sync)
            {
                _global.Clear();
                _root.Clear();
            }
        }

        public bool Contains(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return false;
            }
            
            lock (_sync)
            {
                return _global.ContainsKey(id);
            }
        }

        public bool IsParent(string id, string target)
        {
            if (string.IsNullOrEmpty(target))
            {
                return false;
            }

            if (string.IsNullOrEmpty(id))
            {
                return false;
            }

            lock (_sync)
            {
                if (!_global.ContainsKey(id))
                {
                    return false;
                }
                
                if (!_global.TryGetValue(target, out var nodeB))
                {
                    return false;
                }

                while (nodeB is not null)
                {
                    if (nodeB.Key == id)
                    {
                        return true;
                    }

                    nodeB = nodeB.Parent;
                }
                
                return false;
            }
        }

        public IEnumerable<T> Find<T>(ILiteCollection<T> collection, Func<T, string> keySelector, string id)
        {
            var set = GetChildren(id);
            return collection.Find(x => set.Contains(keySelector(x)));
        }
        
        public IEnumerable<T> Find<T>(ILiteCollection<T> collection, string id) where T : IDataCache
        {
            var set = GetChildren(id);
            return collection.Find(x => set.Contains(x.Id));
        }

        public HashSet<string> GetChildren(string id)
        {
            var set = new HashSet<string>();

            lock (_sync)
            {
                if (!string.IsNullOrEmpty(id) &&
                    _global.TryGetValue(id, out var node))
                {
                    GetChildren(set, node);
                }
            }
            
            return set;
        }

        private static void GetChildren(ISet<string> set, IncludeNode node)
        {
            set.Add(node.Key);
            if (node.Map.Count > 0)
            {
                foreach (var child in node.Map.Values)
                {
                    set.Add(child.Key);
                    GetChildren(set, child);
                }
            }
        }

        private void RemoveRoot(string id, IncludeNode root)
        {
            _root.Remove(id);
            _global.Remove(id);

            if (root.Map.Count == 0)
            {
                return;
            }

            ReorderChildren(root, true, _root);
        }

        private void RemoveNode(string id, IncludeNode node)
        {
            var root = node.Parent.Map;
            _global.Remove(id);
            root.Remove(id);

            if (node.Map.Count == 0)
            {
                return;
            }

            ReorderChildren(node, false, root);
        }

        private void ReorderChildren(IncludeNode node, bool isRoot, IDictionary<string, IncludeNode> root)
        {
            foreach (var child in node.Children)
            {
                var newNode = new IncludeNode
                {
                    Root   = isRoot ? null : node.Root,
                    IsRoot = isRoot,
                    Parent = isRoot ? null : node,
                    Key    = child.Key,
                    Height = isRoot ? 1 : child.Height,
                };
                root.Add(child.Key, newNode);
                _global[child.Key] = newNode;
            }
        }

        /// <summary>
        /// 根节点
        /// </summary>
        public IEnumerable<IncludeNode> GetRootElements() => _root.Values;
    }
}