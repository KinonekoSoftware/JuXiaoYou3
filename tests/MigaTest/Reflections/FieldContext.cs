using System.Reflection;

namespace Acorisoft.FutureGL.MigaTest.Reflections
{
    public class FieldContext
    {
        public FieldContext(FieldInfo info)
        {
            FieldName   = info.Name;
            FieldType   = info.FieldType;
            Information = info;
        }

        public object GetValue(object instance)
        {
            return Information.GetValue(instance);
        }

        /// <summary>
        /// 是否为类
        /// </summary>
        public bool IsClass => FieldType.IsClass;

        /// <summary>
        /// 是否为值类型
        /// </summary>
        public bool IsValueType => FieldType.IsValueType;
        
        /// <summary>
        /// 
        /// </summary>
        protected bool IsGenericType => FieldType.IsGenericType;

        /// <summary>
        /// 是否为枚举
        /// </summary>
        public bool IsEnum => FieldType.IsEnum;

        /// <summary>
        /// 是否为接口
        /// </summary>
        public bool IsInterface => FieldType.IsInterface;

        /// <summary>
        /// 是否为字符串
        /// </summary>
        public bool IsString => IsClass && FieldType == typeof(string);

        /// <summary>
        /// 是否为只读字段
        /// </summary>
        public bool IsReadOnly => Information.IsInitOnly;

        public bool IsNullable => IsValueType &&
                                  IsGenericType &&
                                  !IsClass;

        public Type BaseType => IsNullable
            ? FieldType.GenericTypeArguments
                       .First()
            : null;
        
        public FieldInfo Information { get; }

        public string FieldName { get; }

        public Type FieldType { get; }
        
        /// <summary>
        /// 边界测试
        /// </summary>
        public object BoundaryUnitTest { get; }
    }
}