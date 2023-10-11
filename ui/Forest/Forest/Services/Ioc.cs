using Acorisoft.FutureGL.Forest.Models;
using DryIoc;

namespace Acorisoft.FutureGL.Forest
{
    public static partial class Xaml
    {
        #region Use

        
        /// <summary>
        /// 注册实例。
        /// </summary>
        /// <param name="container">容器</param>
        /// <param name="instance">要注册的实例</param>
        /// <typeparam name="T">注册的类型</typeparam>
        /// <returns>返回实例。</returns>
        public static T Use<T>(this IContainer container, T instance)
        {
            if (instance is null || container is null)
            {
                return default(T);
            }

            container.RegisterInstance<T>(instance, IfAlreadyRegistered.Replace);
            return instance;
        }


        /// <summary>
        /// 注册实例。
        /// </summary>
        /// <param name="container">容器</param>
        /// <param name="instance">要注册的实例</param>
        /// <typeparam name="T">注册的类型</typeparam>
        /// <typeparam name="T1">接口1</typeparam>
        /// <returns>返回实例。</returns>
        public static T Use<T, T1>(this IContainer container, T instance) where T : notnull, T1
        {
            if (instance is null || container is null)
            {
                return default(T);
            }

            container.RegisterInstance(instance);
            container.UseInstance(typeof(T1), instance, IfAlreadyRegistered.AppendNewImplementation);

            return instance;
        }

        /// <summary>
        /// 注册实例。
        /// </summary>
        /// <param name="container">容器</param>
        /// <param name="instance">要注册的实例</param>
        /// <typeparam name="T">注册的类型</typeparam>
        /// <typeparam name="T1">接口1</typeparam>
        /// <typeparam name="T2">接口2</typeparam>
        /// <returns>返回实例。</returns>
        public static T Use<T, T1, T2>(this IContainer container, T instance) where T : notnull, T1, T2
        {
            if (instance is null || container is null)
            {
                return default(T);
            }

            container.RegisterInstance(instance);
            container.UseInstance(typeof(T1), instance, IfAlreadyRegistered.AppendNewImplementation);
            container.UseInstance(typeof(T2), instance, IfAlreadyRegistered.AppendNewImplementation);

            return instance;
        }

        /// <summary>
        /// 注册实例。
        /// </summary>
        /// <param name="container">容器</param>
        /// <param name="instance">要注册的实例</param>
        /// <typeparam name="T">注册的类型</typeparam>
        /// <typeparam name="T1">接口1</typeparam>
        /// <typeparam name="T2">接口2</typeparam>
        /// <typeparam name="T3">接口3</typeparam>
        /// <returns>返回实例。</returns>
        public static T Use<T, T1, T2, T3>(this IContainer container, T instance) where T : notnull, T1, T2, T3
        {
            if (instance is null || container is null)
            {
                return default(T);
            }

            container.RegisterInstance(instance);
            container.UseInstance(typeof(T1), instance, IfAlreadyRegistered.AppendNewImplementation);
            container.UseInstance(typeof(T2), instance, IfAlreadyRegistered.AppendNewImplementation);
            container.UseInstance(typeof(T3), instance, IfAlreadyRegistered.AppendNewImplementation);

            return instance;
        }

        /// <summary>
        /// 注册实例。
        /// </summary>
        /// <param name="container">容器</param>
        /// <param name="instance">要注册的实例</param>
        /// <typeparam name="T">注册的类型</typeparam>
        /// <typeparam name="T1">接口1</typeparam>
        /// <typeparam name="T2">接口2</typeparam>
        /// <typeparam name="T3">接口3</typeparam>
        /// <typeparam name="T4">接口4</typeparam>
        /// <returns>返回实例。</returns>
        public static T Use<T, T1, T2, T3, T4>(this IContainer container, T instance) where T : notnull, T1, T2, T3, T4
        {
            if (instance is null || container is null)
            {
                return default(T);
            }

            container.RegisterInstance(instance);
            container.UseInstance(typeof(T1), instance, IfAlreadyRegistered.AppendNewImplementation);
            container.UseInstance(typeof(T2), instance, IfAlreadyRegistered.AppendNewImplementation);
            container.UseInstance(typeof(T3), instance, IfAlreadyRegistered.AppendNewImplementation);
            container.UseInstance(typeof(T4), instance, IfAlreadyRegistered.AppendNewImplementation);

            return instance;
        }

        /// <summary>
        /// 注册实例。
        /// </summary>
        /// <param name="container">容器</param>
        /// <param name="instance">要注册的实例</param>
        /// <typeparam name="T">注册的类型</typeparam>
        /// <typeparam name="T1">接口1</typeparam>
        /// <typeparam name="T2">接口2</typeparam>
        /// <typeparam name="T3">接口3</typeparam>
        /// <typeparam name="T4">接口4</typeparam>
        /// <typeparam name="T5">接口5</typeparam>
        /// <returns>返回实例。</returns>
        public static T Use<T, T1, T2, T3, T4, T5>(this IContainer container, T instance)
            where T : notnull, T1, T2, T3, T4, T5
        {
            if (instance is null || container is null)
            {
                return default(T);
            }

            container.RegisterInstance(instance);
            container.UseInstance(typeof(T1), instance, IfAlreadyRegistered.AppendNewImplementation);
            container.UseInstance(typeof(T2), instance, IfAlreadyRegistered.AppendNewImplementation);
            container.UseInstance(typeof(T3), instance, IfAlreadyRegistered.AppendNewImplementation);
            container.UseInstance(typeof(T4), instance, IfAlreadyRegistered.AppendNewImplementation);
            container.UseInstance(typeof(T5), instance, IfAlreadyRegistered.AppendNewImplementation);

            return instance;
        }

        /// <summary>
        /// 注册实例。
        /// </summary>
        /// <param name="container">容器</param>
        /// <param name="instance">要注册的实例</param>
        /// <typeparam name="T">注册的类型</typeparam>
        /// <typeparam name="T1">接口1</typeparam>
        /// <typeparam name="T2">接口2</typeparam>
        /// <typeparam name="T3">接口3</typeparam>
        /// <typeparam name="T4">接口4</typeparam>
        /// <typeparam name="T5">接口5</typeparam>
        /// <typeparam name="T6">接口6</typeparam>
        /// <returns>返回实例。</returns>
        public static T Use<T, T1, T2, T3, T4, T5, T6>(this IContainer container, T instance)
            where T : notnull, T1, T2, T3, T4, T5, T6
        {
            if (instance is null || container is null)
            {
                return default(T);
            }

            container.RegisterInstance(instance);
            container.UseInstance(typeof(T1), instance, IfAlreadyRegistered.AppendNewImplementation);
            container.UseInstance(typeof(T2), instance, IfAlreadyRegistered.AppendNewImplementation);
            container.UseInstance(typeof(T3), instance, IfAlreadyRegistered.AppendNewImplementation);
            container.UseInstance(typeof(T4), instance, IfAlreadyRegistered.AppendNewImplementation);
            container.UseInstance(typeof(T5), instance, IfAlreadyRegistered.AppendNewImplementation);
            container.UseInstance(typeof(T6), instance, IfAlreadyRegistered.AppendNewImplementation);

            return instance;
        }

        /// <summary>
        /// 注册实例。
        /// </summary>
        /// <param name="container">容器</param>
        /// <param name="instance">要注册的实例</param>
        /// <typeparam name="T">注册的类型</typeparam>
        /// <typeparam name="T1">接口1</typeparam>
        /// <typeparam name="T2">接口2</typeparam>
        /// <typeparam name="T3">接口3</typeparam>
        /// <typeparam name="T4">接口4</typeparam>
        /// <typeparam name="T5">接口5</typeparam>
        /// <typeparam name="T6">接口6</typeparam>
        /// <typeparam name="T7">接口7</typeparam>
        /// <returns>返回实例。</returns>
        public static T Use<T, T1, T2, T3, T4, T5, T6, T7>(this IContainer container, T instance)
            where T : notnull, T1, T2, T3, T4, T5, T6, T7
        {
            if (instance is null || container is null)
            {
                return default(T);
            }

            container.RegisterInstance(instance);
            container.UseInstance(typeof(T1), instance, IfAlreadyRegistered.AppendNewImplementation);
            container.UseInstance(typeof(T2), instance, IfAlreadyRegistered.AppendNewImplementation);
            container.UseInstance(typeof(T3), instance, IfAlreadyRegistered.AppendNewImplementation);
            container.UseInstance(typeof(T4), instance, IfAlreadyRegistered.AppendNewImplementation);
            container.UseInstance(typeof(T5), instance, IfAlreadyRegistered.AppendNewImplementation);
            container.UseInstance(typeof(T6), instance, IfAlreadyRegistered.AppendNewImplementation);
            container.UseInstance(typeof(T7), instance, IfAlreadyRegistered.AppendNewImplementation);

            return instance;
        }


        /// <summary>
        /// 注册实例。
        /// </summary>
        /// <param name="container">容器</param>
        /// <param name="instance">要注册的实例</param>
        /// <typeparam name="T">注册的类型</typeparam>
        /// <typeparam name="T1">接口1</typeparam>
        /// <typeparam name="T2">接口2</typeparam>
        /// <typeparam name="T3">接口3</typeparam>
        /// <typeparam name="T4">接口4</typeparam>
        /// <typeparam name="T5">接口5</typeparam>
        /// <typeparam name="T6">接口6</typeparam>
        /// <typeparam name="T7">接口7</typeparam>
        /// <typeparam name="T8">接口8</typeparam>
        /// <returns>返回实例。</returns>
        public static T Use<T, T1, T2, T3, T4, T5, T6, T7, T8>(this IContainer container, T instance)
            where T : notnull, T1, T2, T3, T4, T5, T6, T7, T8
        {
            if (instance is null || container is null)
            {
                return default(T);
            }

            container.RegisterInstance(instance);
            container.UseInstance(typeof(T1), instance, IfAlreadyRegistered.AppendNewImplementation);
            container.UseInstance(typeof(T2), instance, IfAlreadyRegistered.AppendNewImplementation);
            container.UseInstance(typeof(T3), instance, IfAlreadyRegistered.AppendNewImplementation);
            container.UseInstance(typeof(T4), instance, IfAlreadyRegistered.AppendNewImplementation);
            container.UseInstance(typeof(T5), instance, IfAlreadyRegistered.AppendNewImplementation);
            container.UseInstance(typeof(T6), instance, IfAlreadyRegistered.AppendNewImplementation);
            container.UseInstance(typeof(T7), instance, IfAlreadyRegistered.AppendNewImplementation);
            container.UseInstance(typeof(T8), instance, IfAlreadyRegistered.AppendNewImplementation);

            return instance;
        }

        /// <summary>
        /// 注册实例。
        /// </summary>
        /// <param name="instance">要注册的实例</param>
        /// <typeparam name="T">注册的类型</typeparam>
        /// <returns>返回实例。</returns>
        public static T Use<T>(T instance)
        {
            if (instance is null)
            {
                return default(T);
            }

            Container.RegisterInstance<T>(instance, IfAlreadyRegistered.Replace);
            return instance;
        }


        /// <summary>
        /// 注册实例。
        /// </summary>
        /// <param name="instance">要注册的实例</param>
        /// <typeparam name="T">注册的类型</typeparam>
        /// <typeparam name="T1">接口1</typeparam>
        /// <returns>返回实例。</returns>
        public static T Use<T, T1>(T instance) where T : notnull, T1
        {
            if (instance is null)
            {
                return default(T);
            }

            Container.RegisterInstance(instance);
            Container.UseInstance(typeof(T1), instance, IfAlreadyRegistered.AppendNewImplementation);

            return instance;
        }

        /// <summary>
        /// 注册实例。
        /// </summary>
        /// <param name="instance">要注册的实例</param>
        /// <typeparam name="T">注册的类型</typeparam>
        /// <typeparam name="T1">接口1</typeparam>
        /// <typeparam name="T2">接口2</typeparam>
        /// <returns>返回实例。</returns>
        public static T Use<T, T1, T2>(T instance) where T : notnull, T1, T2
        {
            if (instance is null)
            {
                return default(T);
            }

            Container.RegisterInstance(instance);
            Container.UseInstance(typeof(T1), instance, IfAlreadyRegistered.AppendNewImplementation);
            Container.UseInstance(typeof(T2), instance, IfAlreadyRegistered.AppendNewImplementation);

            return instance;
        }

        /// <summary>
        /// 注册实例。
        /// </summary>
        /// <param name="instance">要注册的实例</param>
        /// <typeparam name="T">注册的类型</typeparam>
        /// <typeparam name="T1">接口1</typeparam>
        /// <typeparam name="T2">接口2</typeparam>
        /// <typeparam name="T3">接口3</typeparam>
        /// <returns>返回实例。</returns>
        public static T Use<T, T1, T2, T3>(T instance) where T : notnull, T1, T2, T3
        {
            if (instance is null)
            {
                return default(T);
            }

            Container.RegisterInstance(instance);
            Container.UseInstance(typeof(T1), instance, IfAlreadyRegistered.AppendNewImplementation);
            Container.UseInstance(typeof(T2), instance, IfAlreadyRegistered.AppendNewImplementation);
            Container.UseInstance(typeof(T3), instance, IfAlreadyRegistered.AppendNewImplementation);

            return instance;
        }

        /// <summary>
        /// 注册实例。
        /// </summary>
        /// <param name="instance">要注册的实例</param>
        /// <typeparam name="T">注册的类型</typeparam>
        /// <typeparam name="T1">接口1</typeparam>
        /// <typeparam name="T2">接口2</typeparam>
        /// <typeparam name="T3">接口3</typeparam>
        /// <typeparam name="T4">接口4</typeparam>
        /// <returns>返回实例。</returns>
        public static T Use<T, T1, T2, T3, T4>(T instance) where T : notnull, T1, T2, T3, T4
        {
            if (instance is null)
            {
                return default(T);
            }

            Container.RegisterInstance(instance);
            Container.UseInstance(typeof(T1), instance, IfAlreadyRegistered.AppendNewImplementation);
            Container.UseInstance(typeof(T2), instance, IfAlreadyRegistered.AppendNewImplementation);
            Container.UseInstance(typeof(T3), instance, IfAlreadyRegistered.AppendNewImplementation);
            Container.UseInstance(typeof(T4), instance, IfAlreadyRegistered.AppendNewImplementation);

            return instance;
        }

        /// <summary>
        /// 注册实例。
        /// </summary>
        /// <param name="instance">要注册的实例</param>
        /// <typeparam name="T">注册的类型</typeparam>
        /// <typeparam name="T1">接口1</typeparam>
        /// <typeparam name="T2">接口2</typeparam>
        /// <typeparam name="T3">接口3</typeparam>
        /// <typeparam name="T4">接口4</typeparam>
        /// <typeparam name="T5">接口5</typeparam>
        /// <returns>返回实例。</returns>
        public static T Use<T, T1, T2, T3, T4, T5>(T instance) where T : notnull, T1, T2, T3, T4, T5
        {
            if (instance is null || Container is null)
            {
                return default(T);
            }

            Container.RegisterInstance(instance);
            Container.UseInstance(typeof(T1), instance, IfAlreadyRegistered.AppendNewImplementation);
            Container.UseInstance(typeof(T2), instance, IfAlreadyRegistered.AppendNewImplementation);
            Container.UseInstance(typeof(T3), instance, IfAlreadyRegistered.AppendNewImplementation);
            Container.UseInstance(typeof(T4), instance, IfAlreadyRegistered.AppendNewImplementation);
            Container.UseInstance(typeof(T5), instance, IfAlreadyRegistered.AppendNewImplementation);

            return instance;
        }

        /// <summary>
        /// 注册实例。
        /// </summary>
        /// <param name="instance">要注册的实例</param>
        /// <typeparam name="T">注册的类型</typeparam>
        /// <typeparam name="T1">接口1</typeparam>
        /// <typeparam name="T2">接口2</typeparam>
        /// <typeparam name="T3">接口3</typeparam>
        /// <typeparam name="T4">接口4</typeparam>
        /// <typeparam name="T5">接口5</typeparam>
        /// <typeparam name="T6">接口6</typeparam>
        /// <returns>返回实例。</returns>
        public static T Use<T, T1, T2, T3, T4, T5, T6>(T instance) where T : notnull, T1, T2, T3, T4, T5, T6
        {
            if (instance is null)
            {
                return default(T);
            }

            Container.RegisterInstance(instance);
            Container.UseInstance(typeof(T1), instance, IfAlreadyRegistered.AppendNewImplementation);
            Container.UseInstance(typeof(T2), instance, IfAlreadyRegistered.AppendNewImplementation);
            Container.UseInstance(typeof(T3), instance, IfAlreadyRegistered.AppendNewImplementation);
            Container.UseInstance(typeof(T4), instance, IfAlreadyRegistered.AppendNewImplementation);
            Container.UseInstance(typeof(T5), instance, IfAlreadyRegistered.AppendNewImplementation);
            Container.UseInstance(typeof(T6), instance, IfAlreadyRegistered.AppendNewImplementation);

            return instance;
        }

        /// <summary>
        /// 注册实例。
        /// </summary>
        /// <param name="instance">要注册的实例</param>
        /// <typeparam name="T">注册的类型</typeparam>
        /// <typeparam name="T1">接口1</typeparam>
        /// <typeparam name="T2">接口2</typeparam>
        /// <typeparam name="T3">接口3</typeparam>
        /// <typeparam name="T4">接口4</typeparam>
        /// <typeparam name="T5">接口5</typeparam>
        /// <typeparam name="T6">接口6</typeparam>
        /// <typeparam name="T7">接口7</typeparam>
        /// <returns>返回实例。</returns>
        public static T Use<T, T1, T2, T3, T4, T5, T6, T7>(T instance) where T : notnull, T1, T2, T3, T4, T5, T6, T7
        {
            if (instance is null)
            {
                return default(T);
            }

            Container.RegisterInstance(instance);
            Container.UseInstance(typeof(T1), instance, IfAlreadyRegistered.AppendNewImplementation);
            Container.UseInstance(typeof(T2), instance, IfAlreadyRegistered.AppendNewImplementation);
            Container.UseInstance(typeof(T3), instance, IfAlreadyRegistered.AppendNewImplementation);
            Container.UseInstance(typeof(T4), instance, IfAlreadyRegistered.AppendNewImplementation);
            Container.UseInstance(typeof(T5), instance, IfAlreadyRegistered.AppendNewImplementation);
            Container.UseInstance(typeof(T6), instance, IfAlreadyRegistered.AppendNewImplementation);
            Container.UseInstance(typeof(T7), instance, IfAlreadyRegistered.AppendNewImplementation);

            return instance;
        }


        /// <summary>
        /// 注册实例。
        /// </summary>
        /// <param name="instance">要注册的实例</param>
        /// <typeparam name="T">注册的类型</typeparam>
        /// <typeparam name="T1">接口1</typeparam>
        /// <typeparam name="T2">接口2</typeparam>
        /// <typeparam name="T3">接口3</typeparam>
        /// <typeparam name="T4">接口4</typeparam>
        /// <typeparam name="T5">接口5</typeparam>
        /// <typeparam name="T6">接口6</typeparam>
        /// <typeparam name="T7">接口7</typeparam>
        /// <typeparam name="T8">接口8</typeparam>
        /// <returns>返回实例。</returns>
        public static T Use<T, T1, T2, T3, T4, T5, T6, T7, T8>(T instance)
            where T : notnull, T1, T2, T3, T4, T5, T6, T7, T8
        {
            if (instance is null)
            {
                return default(T);
            }

            Container.RegisterInstance(instance);
            Container.UseInstance(typeof(T1), instance, IfAlreadyRegistered.AppendNewImplementation);
            Container.UseInstance(typeof(T2), instance, IfAlreadyRegistered.AppendNewImplementation);
            Container.UseInstance(typeof(T3), instance, IfAlreadyRegistered.AppendNewImplementation);
            Container.UseInstance(typeof(T4), instance, IfAlreadyRegistered.AppendNewImplementation);
            Container.UseInstance(typeof(T5), instance, IfAlreadyRegistered.AppendNewImplementation);
            Container.UseInstance(typeof(T6), instance, IfAlreadyRegistered.AppendNewImplementation);
            Container.UseInstance(typeof(T7), instance, IfAlreadyRegistered.AppendNewImplementation);
            Container.UseInstance(typeof(T8), instance, IfAlreadyRegistered.AppendNewImplementation);

            return instance;
        }

        #endregion
        
        /// <summary>
        /// 判断是否已经注册。
        /// </summary>
        /// <param name="type">指定要判断的类型。</param>
        /// <returns>已经注册将会返回true，否则返回false。</returns>
        public static bool IsRegistered(Type type) => type is not null && Container.IsRegistered(type);
        
        /// <summary>
        /// 判断是否已经注册。
        /// </summary>
        /// <typeparam name="T">指定要判断的类型。</typeparam>
        /// <returns>已经注册将会返回true，否则返回false。</returns>
        public static bool IsRegistered<T>() => Container.IsRegistered<T>();
        
        /// <summary>
        /// 获得指定服务
        /// </summary>
        /// <typeparam name="T">指定的服务类型</typeparam>
        /// <returns>返回指定的服务</returns>
        public static T Get<T>() => Container.Resolve<T>();
        
        /// <summary>
        /// 获得指定服务
        /// </summary>
        /// <typeparam name="T">指定的服务类型</typeparam>
        /// <param name="type">指定要获得的类型。</param>
        /// <returns>返回指定的服务</returns>
        public static T Get<T>(Type type) => (T)Container.Resolve(type);
        
        /// <summary>
        /// 表示一个Ioc容器
        /// </summary>
        public static IContainer Container { get; }
        
        
        public class Installer : IViewInstaller
        {
            public void Install(BindingInfo info)
            {
                InstallView(info);
            }

            public void Install(IEnumerable<BindingInfo> bindingInfos)
            {
                if (bindingInfos is null)
                {
                    return;
                }
                
                foreach (var info in bindingInfos)
                {
                    InstallView(info);
                }
            }

            public void Install(IBindingInfoProvider provider)
            {
                if (provider is null)
                {
                    return;
                }
                Install(provider.GetBindingInfo());
            }

            public void Install(IEnumerable<IBindingInfoProvider> providers)
            {
                
                if (providers is null)
                {
                    return;
                }
                
                foreach (var info in providers)
                {
                    Install(info);
                }
            }
        }
    }
}