using Acorisoft.FutureGL.Forest.AppModels;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.MigaDB.Interfaces;

namespace Acorisoft.FutureGL.MigaStudio.Core
{
    public static class NavigationParameter
    {
        /// <summary>
        /// 测试
        /// </summary>
        /// <returns>返回一个新的导航参数。</returns>
        public static RoutingEventArgs Empty()
        {
            return RoutingEventArgs.Empty;
        }

        /// <summary>
        /// 新建一个导航参数
        /// </summary>
        /// <param name="id">数据</param>
        /// <param name="controller">控制器</param>
        /// <returns>返回一个新的导航参数。</returns>
        public static RoutingEventArgs NewPage(string id, ITabViewController controller)
        {
            return new RoutingEventArgs
            {
                Id         = id,
                Controller = controller,
                Parameter = new Parameter
                {
                    Args = new object[4]
                }
            };
        }

        /// <summary>
        /// 新建一个导航参数
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="controller">控制器</param>
        /// <param name="parameter">控制器</param>
        /// <returns>返回一个新的导航参数。</returns>
        public static RoutingEventArgs NewPage(string id, ITabViewController controller, Parameter parameter)
        {
            return new RoutingEventArgs
            {
                Id         = id,
                Controller = controller,
                Parameter  = parameter
            };
        }

        /// <summary>
        /// 新建一个导航参数
        /// </summary>
        /// <param name="cache">索引</param>
        /// <param name="controller">控制器</param>
        /// <returns>返回一个新的导航参数。</returns>
        public static RoutingEventArgs OpenDocument(IDataCache cache, ITabViewController controller)
        {
            return new RoutingEventArgs
            {
                Id         = cache.Id,
                Controller = controller,
                Parameter = new Parameter
                {
                    Args = new object[]
                    {
                        cache,
                        null,
                        null,
                        null
                    }
                }
            };
        }

        /// <summary>
        /// 新建一个导航参数
        /// </summary>
        /// <param name="fileName">数据</param>
        /// <param name="controller">控制器</param>
        /// <returns>返回一个新的导航参数。</returns>
        public static RoutingEventArgs OpenFile(string fileName, ITabViewController controller)
        {
            return new RoutingEventArgs
            {
                Id         = fileName,
                Controller = controller,
                Parameter = new Parameter
                {
                    Args = new object[]
                    {
                        fileName,
                        null,
                        null,
                        null
                    }
                }
            };
        }

        /// <summary>
        /// 新建一个导航参数
        /// </summary>
        /// <param name="fileName">数据</param>
        /// <param name="parameter">索引</param>
        /// <param name="controller">控制器</param>
        /// <returns>返回一个新的导航参数。</returns>
        public static RoutingEventArgs OpenFile(string fileName, Parameter parameter, ITabViewController controller)
        {
            return new RoutingEventArgs
            {
                Id         = fileName,
                Controller = controller,
                Parameter  = parameter
            };
        }

        /// <summary>
        /// 新建一个导航参数
        /// </summary>
        /// <param name="viewModel">视图模型</param>
        /// <param name="controller">控制器</param>
        /// <returns>返回一个新的导航参数。</returns>
        public static RoutingEventArgs NewPage(ITabViewModel viewModel, ITabViewController controller)
        {
            if (string.IsNullOrEmpty(viewModel.PageId))
            {
                if (string.IsNullOrEmpty(viewModel.PageId) &&
                    !viewModel.Uniqueness)
                {
                    return new RoutingEventArgs
                    {
                        Id = Guid.NewGuid()
                                 .ToString("N"),
                        Controller = controller,
                        Parameter = new Parameter
                        {
                            Args = new object[4]
                        }
                    };
                }                
            }
            return new RoutingEventArgs
            {
                Controller = controller,
                Parameter = new Parameter
                {
                    Args = new object[4]
                }
            };
        }

        /// <summary>
        /// 新建一个导航参数
        /// </summary>
        /// <param name="viewModel">视图模型</param>
        /// <param name="controller">控制器</param>
        /// <param name="parameter">控制器</param>
        /// <returns>返回一个新的导航参数。</returns>
        public static RoutingEventArgs NewPage(ITabViewModel viewModel, ITabViewController controller, Parameter parameter)
        {
            if (string.IsNullOrEmpty(viewModel.PageId) &&
                !viewModel.Uniqueness)
            {
                return new RoutingEventArgs
                {
                    Id = Guid.NewGuid()
                             .ToString("N"),
                    Controller = controller,
                    Parameter = parameter
                };
            }

            return new RoutingEventArgs
            {
                Controller = controller,
                Parameter = parameter
            };
        }
    }
}