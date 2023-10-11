namespace Acorisoft.FutureGL.MigaDB.Data
{
    /// <summary>
    /// <see cref="PrimitiveProperty{T}"/> 类型表示情感状态的声明
    /// </summary>
    public class PrimitiveProperty<T>
    {
        public Dictionary<string, T> Value { get; init; }

        public T this[string key]
        {
            get => Value.TryGetValue(key, out var v) ? v : default(T);
            set { Value[key] = value; }
        }
    }

    public class BooleanProperty : PrimitiveProperty<bool>
    {
        
    }
    
    public class Int32Property : PrimitiveProperty<int>
    {
        
    }
    
    public class DoubleProperty : PrimitiveProperty<double>
    {
        
    }
    
    public class StringProperty : PrimitiveProperty<string>
    {
        
    }
}