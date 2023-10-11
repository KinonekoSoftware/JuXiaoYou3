using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Acorisoft.Miga.Utils
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class ValidateAttribute : Attribute
    {
        
    }
    
    public static class AssemblyExtensions
    {
        public static Stream GetResourceStream(this Assembly assembly, string fileName)
        {
            var assemblyFullNames = assembly?.FullName?.Split(',');

            if (assemblyFullNames is null)
            {
                return Stream.Null;
            }

            var moduleName = assemblyFullNames[0];
            var resxName = $"{moduleName}.{fileName}";

            return assembly.GetManifestResourceStream(resxName);
        }
    }
    
    /// <summary>
    /// 类型帮助类。
    /// </summary>
    public static class OldVerClasses
    {
        
        /// <summary>
        /// 创建指定类型的实例。
        /// </summary>
        /// <typeparam name="T">指定要创建的实例类型。</typeparam>
        /// <returns>返回一个新创建的实例类型。</returns>
        public static T CreateInstance<T>() => TypeCache<T>.Activator();

        /// <summary>
        /// 缓存Lambda类型。
        /// </summary>
        /// <typeparam name="T">需要创建的实例类型。</typeparam>
        // ReSharper disable once ClassNeverInstantiated.Local
        private class TypeCache<T>
        {
            public static readonly Func<T> Activator = Classes<T>.CreateInstance(typeof(T));
        }

        #region GetDescriptionAttribute

        /// <summary>
        /// 获取枚举值中的指定特性。
        /// </summary>
        /// <param name="instance">指定要获取的枚举。</param>
        /// <returns>返回获取的特性，如果不存在则返回null。</returns>
        public static string GetDescriptionAttribute(object instance)
        {
            if (instance is null)
            {
                return string.Empty;
            }

            if (instance is Enum @enum)
            {
                return GetDescriptionAttribute(@enum);
            }

            return instance.GetType()
                    .GetCustomAttribute(typeof(DescriptionAttribute)) is DescriptionAttribute
                description
                ? description.Description
                : string.Empty;
        }
        
        /// <summary>
        /// 获取枚举值中的指定特性。
        /// </summary>
        /// <param name="instance">指定要获取的枚举。</param>
        /// <returns>返回获取的特性，如果不存在则返回null。</returns>
        public static string GetDescriptionAttribute(Enum instance)
        {
            if (instance is null)
            {
                return string.Empty;
            }

            var field = instance.GetType().GetField(instance.ToString())!;

            return Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is not DescriptionAttribute attribute ? 
                instance.ToString() : 
                attribute.Description;
        }
                
        /// <summary>
        /// 获取枚举值中的指定特性。
        /// </summary>
        /// <param name="instance">指定要获取的枚举。</param>
        /// <typeparam name="T">指定要获取的特性类型。</typeparam>
        /// <returns>返回获取的特性，如果不存在则返回null。</returns>
        public static T GetAttribute<T>(Enum instance)
        {
            if (instance is null)
            {
                return default(T);
            }

            return instance.GetType()
                .GetCustomAttribute(typeof(T)) is T description
                ? description
                : default(T);
        }
        
        /// <summary>
        /// 获取枚举值中的指定特性。
        /// </summary>
        /// <param name="instance">指定要获取的枚举。</param>
        /// <param name="type">指定要获取的特性类型。</param>
        /// <returns>返回获取的特性，如果不存在则返回null。</returns>
        public static Attribute GetAttribute(Enum instance, Type type)
        {
            return instance?.GetType().GetCustomAttribute(type);
        }
        
        #endregion

        #region GetProxy

        public static PropertySetter GetSetter(object instance)
        {
            return new PropertySetter(instance.GetType());
        }

        
        /// <summary>
        /// 获取某个类型的属性代理（Setter、Getter）。
        /// </summary>
        /// <param name="instance">要获取的对象实例。</param>
        /// <param name="force">是否开启Validate检测</param>
        /// <returns>返回一个对象实例的属性代理</returns>
        public static PropertyProxy GetProxy(object instance, bool force = true)
        {
            return instance is null ? null : GetProxy(instance.GetType(), instance, force);
        }

        /// <summary>
        /// 获取某个类型的属性代理（Setter、Getter）。
        /// </summary>
        /// <param name="classType">要获取的类型</param>
        /// <param name="force">是否开启Validate检测</param>
        /// <returns>返回一个对象实例的属性代理</returns>
        public static PropertyProxy GetProxy(Type classType, bool force = true)
        {
            return GetProxy(classType, null, force);
        }


        /// <summary>
        /// 获取某个类型的属性代理（Setter、Getter）。
        /// </summary>
        /// <param name="classType">要获取的类型</param>
        /// <param name="instance">要获取的对象实例。</param>
        /// <param name="force">要获取的对象实例。</param>
        /// <returns>返回一个对象实例的属性代理</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static PropertyProxy GetProxy(Type classType, object instance, bool force = true)
        {
            if (classType is null)
            {
                throw new ArgumentNullException(nameof(classType));
            }

            return new PropertyProxy(classType, instance, force);
        }
        
        #endregion

        #region FindInterfaceImplmentation

        /// <summary>
        /// 寻找某个接口的所有实现类型。
        /// </summary>
        /// <param name="assembly">指定要检索的程序集，要求不能为空。</param>
        /// <param name="interfaceType">指定要检索的接口类型，要求不能为空。</param>
        /// <returns>返回操作的结果，如果不存在则返回null。</returns>
        public static Type FindInterfaceImplementation(Assembly assembly, Type interfaceType)
        {
            if (assembly is null || interfaceType is null)
            {
                return null;
            }

            return assembly.GetTypes().FirstOrDefault(x => x.GetInterfaces().Contains(interfaceType));
        }
        
        /// <summary>
        /// 寻找某个接口的所有实现类型。
        /// </summary>
        /// <param name="assembly">指定要检索的程序集，要求不能为空。</param>
        /// <returns>返回操作的结果，如果不存在则返回null。</returns>
        public static Type FindInterfaceImplementation<T>(Assembly assembly)
        {
            return assembly?.GetTypes().FirstOrDefault(x => x.GetInterfaces().Contains(typeof(T)));
        }
        
        /// <summary>
        /// 寻找某个接口的所有实现类型。
        /// </summary>
        /// <param name="assembly">指定要检索的程序集，要求不能为空。</param>
        /// <returns>返回操作的结果，如果不存在则返回null。</returns>
        public static IEnumerable<Type> FindInterfaceImplementations<T>(Assembly assembly)
        {
            return assembly?.GetTypes().Where(x => x.GetInterfaces().Contains(typeof(T)));
        }
        
        /// <summary>
        /// 寻找某个接口的所有实现类型。
        /// </summary>
        /// <param name="assembly">指定要检索的程序集，要求不能为空。</param>
        /// <param name="interfaceType">指定要检索的接口类型，要求不能为空。</param>
        /// <returns>返回操作的结果，如果不存在则返回null。</returns>
        public static IEnumerable<Type> FindInterfaceImplementations(Assembly assembly, Type interfaceType)
        {
            if (assembly is null || interfaceType is null)
            {
                return null;
            }

            return assembly.GetTypes().Where(x => x.GetInterfaces().Contains(interfaceType));
        }

        #endregion
        
    }
    
    
    public class PropertySetter
    {
        private readonly Dictionary<string, Action<object, object>> _setterMappers;
        
        internal PropertySetter(Type classType)
        {
            DataType = classType;
            
            var instance_props = classType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty);

            _setterMappers = new Dictionary<string, Action<object, object>>(StringComparer.OrdinalIgnoreCase);

            foreach (var instanceProp in instance_props)
            { 
                
                var setMethod = instanceProp.SetMethod;

                void SetterExpr(object i, object val) => setMethod!.Invoke(i, new[] { val });
                
                _setterMappers.TryAdd(instanceProp.Name, SetterExpr);
            }
        }

        public Type DataType { get; }

        public object this[string name, object instance]
        {
            set
            {
                var target = instance;

                if (string.IsNullOrEmpty(name))
                {
                    return;
                }

                if (_setterMappers.TryGetValue(name, out var expression))
                {
                    expression(target, value);
                }
            }
        }
    }

    public class PropertyProxy
    {
        private readonly Dictionary<string, Func<object, object>> _getterMappers;
        private readonly Dictionary<string, Action<object, object>> _setterMappers;
        private readonly object _instance;
        private static readonly object Empty = null; 
        
        internal PropertyProxy(Type classType, object instance, bool force)
        {
            DataType = classType;
            
            var static_props = classType.GetProperties(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            var instance_props = classType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            _instance = instance;
            _getterMappers = new Dictionary<string, Func<object, object>>(StringComparer.CurrentCultureIgnoreCase);
            _setterMappers = new Dictionary<string, Action<object, object>>(StringComparer.CurrentCultureIgnoreCase);

            foreach (var staticProp in static_props)
            {
                var setMethod = staticProp.SetMethod;
                var getMethod = staticProp.GetMethod;
                
                Expression<Action<object, object>> setterExpr = (_, val) => setMethod.Invoke(null, new[] {val});
                Expression<Func<object, object>> getterExpr = _ => getMethod.Invoke(null, null);

                if (!force && staticProp.GetCustomAttribute<ValidateAttribute>() is null)
                {
                    continue;
                }

                _getterMappers.TryAdd(staticProp.Name, getterExpr.Compile());
                _setterMappers.TryAdd(staticProp.Name, setterExpr.Compile());
            }

            foreach (var instanceProp in instance_props)
            { 
                //
                // Lambda
                //
                // var setMethod = instanceProp.SetMethod;
                // var getMethod = instanceProp.GetMethod;
                //
                // Expression<Action<object, object>> setterExpr = (i, val) => setMethod.Invoke(i, new[] {val});
                // Expression<Func<object, object>> getterExpr = i => getMethod.Invoke(i, null);
                //
                // if (!force && instanceProp.GetCustomAttribute<ValidateAttribute>() is null)
                // {
                //     continue;
                // }
                //
                // _getterMappers.TryAdd(instanceProp.Name, getterExpr.Compile());
                // _setterMappers.TryAdd(instanceProp.Name, setterExpr.Compile());
                
                var setMethod = instanceProp.SetMethod;
                var getMethod = instanceProp.GetMethod;

                void SetterExpr(object i, object val) => setMethod!.Invoke(i, new[] { val });
                object GetterExpr(object i) => getMethod!.Invoke(i, null);

                if (!force && instanceProp.GetCustomAttribute<ValidateAttribute>() is null)
                {
                    continue;
                }
                
                _getterMappers.TryAdd(instanceProp.Name, GetterExpr);
                _setterMappers.TryAdd(instanceProp.Name, SetterExpr);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<string> PropertyNames => _getterMappers.Keys;
        
        public Type DataType { get; }

        public object this[string name, object instance]
        {
            get
            {
                var target = _instance ?? instance;

                if (string.IsNullOrEmpty(name))
                {
                    return Empty;
                }

                return !_getterMappers.TryGetValue(name, out var expression) ? Empty : expression(target);
            }

            set
            {
                var target = _instance ?? instance;

                if (string.IsNullOrEmpty(name))
                {
                    return;
                }

                if (_setterMappers.TryGetValue(name, out var expression))
                {
                    expression(target, value);
                }
            }
        }
    }

    internal static class Classes<T>
    {
        public static Func<T> CreateInstance(Type type)
        {
            //
            // 为什么使用TypeInfo而不是Type
            // 
            //  https://docs.microsoft.com/en-us/dotnet/api/system.reflection.typeinfo?view=net-5.0
            //
            // 通过浏览官方网站的定义我们发现TypeInfo主要用于Microsoft Store程序里面。
            Debug.Assert(type is not null);
            Debug.Assert(typeof(T).GetTypeInfo().IsAssignableFrom(type.GetTypeInfo()));

            //
            // 找到构造函数中非静态构造函数并且无参数的构造函数。
            var constructor = type.GetTypeInfo()
                .DeclaredConstructors
                .FirstOrDefault(x => !x.IsStatic && !x.GetParameters().Any());

            if (constructor is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            //
            // 使用Lambda表达式实现创建指定类型的实例。
            return Expression.Lambda<Func<T>>(Expression.New(constructor) , null).Compile();
        }
    }
}