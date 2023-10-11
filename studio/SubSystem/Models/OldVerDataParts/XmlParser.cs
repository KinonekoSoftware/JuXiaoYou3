using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using Acorisoft.Miga.Utils;
using GlobalTypeClassResolvers = System.Collections.Generic.Dictionary<System.Type, Acorisoft.Miga.Xml.ClassResolver>;
using GlobalStringClassResolvers = System.Collections.Generic.Dictionary<string, Acorisoft.Miga.Xml.ClassResolver>;
using LocalPropertyResolvers = System.Collections.Generic.Dictionary<string, Acorisoft.Miga.Xml.PropertyResolver>;
using PropertySetter = Acorisoft.Miga.Utils.PropertySetter;

namespace Acorisoft.Miga.Xml
{
    public class XmlParser
    {
        private readonly GlobalTypeClassResolvers         ByTypeResolver;
        private readonly GlobalStringClassResolvers       ByStringResolver;
        private readonly Dictionary<Type, PropertySetter> ByTypeSetter = new Dictionary<Type, PropertySetter>(13);

        private XmlParser(long tct, long tcm, GlobalStringClassResolvers bsr, GlobalTypeClassResolvers btr)
        {
            ByStringResolver    = bsr ?? throw new ArgumentNullException(nameof(bsr));
            ByTypeResolver      = btr ?? throw new ArgumentNullException(nameof(btr));
            ElapsedMilliseconds = tcm;
            ElapsedTicks        = tct;
        }

        #region Parse

        public Compilation<T> Parse<T>(XDocument document)
        {
            if (document is null)
            {
                return new Compilation<T>
                {
                    IsFinished = false,
                };
            }

            var counter = new Stopwatch();

            //
            //
            var root = document.Root;

            //
            //
            if (root is null)
            {
                //
                //
                counter.Stop();
                return new Compilation<T>
                {
                    IsFinished = false,
                };
            }

            //
            //
            counter.Start();
            var errors = new List<ErrorArgs>(64);
            var instance = HandleRoot(root, errors);

            //
            //
            counter.Stop();

            return new Compilation<T>
            {
                Result     = (T)instance,
                IsFinished = true,
                Errors     = errors,
                Document   = document,
                TCM        = counter.ElapsedMilliseconds,
                TCT        = counter.ElapsedTicks
            };
        }

        private object HandleRoot(XElement element, IList<ErrorArgs> errors)
        {
            var name = element.Name.LocalName;
            var lineInfo = (IXmlLineInfo)element;

            if (ByStringResolver.TryGetValue(name, out var resolver))
            {
                var instance = resolver.New();
                var proxy = GetSetter(instance);

                //
                //
                foreach (var attribute in element.Attributes())
                {
                    //
                    // 处理属性
                    HandleProperty(proxy, instance, resolver, attribute, errors);
                }

                //
                //
                if (element.HasElements)
                {
                    foreach (var content in element.Elements())
                    {
                        HandleContent(instance, resolver, content, errors);
                    }
                }
                else
                {
                    //
                    // handle text node
                    if (instance is ITextNode textNode)
                    {
                        textNode.AddText(element.Value);
                    }
                }

                return instance;
            }


            errors.Add(new ErrorArgs
            {
                LineNumber   = lineInfo.LineNumber,
                LinePosition = lineInfo.LinePosition,
                Message      = $"无法识别指定的元素{name}"
            });
            //
            //
            return null;
        }

        private void HandleContent(object parent,
            ClassResolver parentResolver,
            XElement element,
            ICollection<ErrorArgs> errors)
        {
            var name = element.Name.LocalName;
            var lineInfo = (IXmlLineInfo)element;

            //
            // alias property
            if (!HandleContentWithAlias(parent, parentResolver, element, out var instance, out var proxy, out var resolver) &&
                !HandleContentWithNoAlias(parent, element, out instance, out proxy, out resolver))
            {
                errors.Add(new ErrorArgs
                {
                    LineNumber   = lineInfo.LineNumber,
                    LinePosition = lineInfo.LinePosition,
                    Message      = $"无法识别指定的元素{name}"
                });

                return;
            }

            //
            //
            foreach (var attribute in element.Attributes())
            {
                //
                // 处理属性
                HandleProperty(proxy, instance, resolver, attribute, errors);
            }

            //
            //
            if (element.HasElements)
            {
                foreach (var content in element.Elements())
                {
                    HandleContent(instance, resolver, content, errors);
                }
            }
            else
            {
                //
                // handle text node
                if (instance is ITextNode textNode)
                {
                    textNode.AddText(element.Value);
                }
            }

            AssignToParent(
                proxy,
                lineInfo,
                name,
                parent,
                instance,
                parentResolver,
                errors);
        }

        private bool HandleContentWithAlias(object parent,
            ClassResolver parentResolver,
            XElement element,
            out object instance,
            out PropertySetter proxy,
            out ClassResolver resolver)
        {
            var name = element.Name.LocalName;
            resolver = null;
            instance = null;
            proxy    = null;

            //
            // alias property
            if (!parentResolver.GetResolver(name, out var resolver1) &&
                ByStringResolver.TryGetValue(name, out resolver))
            {
                return false;
            }

            if (resolver is null &&
                resolver1 is null)
            {
                return false;
            }

            resolver = ((ClassPropertyResolver)resolver1).Resolver;
            instance = resolver.New();
            proxy    = GetSetter(parent);
            return true;
        }

        private bool HandleContentWithNoAlias(object parent,
            XElement element,
            out object instance,
            out PropertySetter proxy,
            out ClassResolver resolver)
        {
            var name = element.Name.LocalName;

            instance = null;
            proxy    = null;

            //
            // alias property
            if (!ByStringResolver.TryGetValue(name, out resolver))
            {
                return false;
            }

            instance = resolver.New();
            proxy    = GetSetter(instance);

            return true;
        }

        private void HandleProperty(PropertySetter proxy,
            object targetInstance,
            ClassResolver parentResolver,
            XAttribute attribute,
            ICollection<ErrorArgs> errors)
        {
            var lineInfo = (IXmlLineInfo)attribute;
            var name = attribute.Name;

            if (parentResolver.GetResolver(name.LocalName, out var resolver))
            {
                resolver.Assign(proxy, targetInstance, attribute.Value);
            }
            else
            {
                errors.Add(new ErrorArgs
                {
                    LineNumber   = lineInfo.LineNumber,
                    LinePosition = lineInfo.LinePosition,
                    Message      = $"无法识别指定的属性{name}"
                });
            }
        }

        private void AssignToParent(
            PropertySetter proxy,
            IXmlLineInfo lineInfo,
            string name,
            object parent,
            object instance,
            ClassResolver parentResolver,
            ICollection<ErrorArgs> errors)
        {
            if (parent is IList list)
            {
                list.Add(instance);
            }
            else if (parent is IAddChild addChild)
            {
                addChild.Accept(instance);
            }
            else if (parentResolver.GetResolver(name, out var resolver))
            {
                resolver.Assign(proxy, parent, instance);
            }
            else
            {
                errors.Add(new ErrorArgs
                {
                    LineNumber   = lineInfo.LineNumber,
                    LinePosition = lineInfo.LinePosition,
                    Message      = $"无法识别赋值{name}"
                });
            }
        }

        #endregion

        #region Mapping

        private PropertySetter GetSetter(object instance)
        {
            var type = instance.GetType();
            if (!ByTypeSetter.TryGetValue(type, out var setter))
            {
                setter = OldVerClasses.GetSetter(instance);
                ByTypeSetter.Add(type, setter);
            }

            return setter;
        }

        //
        // ClassResolver 
        //
        // Top-level that means you should process
        // content-level -> nested property
        //
        /// <summary>
        /// 初始化解析器是一个非常耗时的操作，需要使用异步来实现。
        /// </summary>
        /// <param name="importMany"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static XmlParser GetParser(IEnumerable<Type> importMany)
        {
            if (importMany is null)
            {
                throw new ArgumentNullException(nameof(importMany));
            }

            var btr = new GlobalTypeClassResolvers(17);
            var bsr = new GlobalStringClassResolvers(StringComparer.OrdinalIgnoreCase);
            var counter = new Stopwatch();

            //
            //
            counter.Start();

            //
            //
            foreach (var importType in importMany)
            {
                if (importType is null)
                {
                    continue;
                }

                //
                //
                CompileTopLevelClass(importType, btr, bsr);
            }

            return new XmlParser(counter.ElapsedTicks, counter.ElapsedMilliseconds, bsr, btr);
        }


        internal static Func<object> CreateInstance(Type type)
        {
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
            return () => constructor.Invoke(null);
        }

        private static string GetAlias(MemberInfo member, string value)
        {
            if (member.IsDefined(typeof(AliasAttribute)) &&
                member.GetCustomAttribute<AliasAttribute>() is { Alias: { } elementName })
            {
                return string.IsNullOrEmpty(elementName) ? value : elementName;
            }

            return value;
        }


        private static IEnumerable<PropertyInfo> GetProperties(IReflect classType)
        {
            return classType.GetProperties(BindingFlags.Instance |
                                           BindingFlags.Public |
                                           BindingFlags.SetProperty)
                .Where(x => !x.IsDefined(typeof(IgnoreAttribute)))
                .ToArray();
        }

        private static void CompileTopLevelClass(Type classType, GlobalTypeClassResolvers btr,
            GlobalStringClassResolvers bsr)
        {
            //
            // 所有属性
            var members = GetProperties(classType);
            var lsr = new LocalPropertyResolvers(StringComparer.OrdinalIgnoreCase);

            //
            //
            foreach (var member in members)
            {
                var pt = member.PropertyType;
                var pn = member.Name;
                var indeedName = GetAlias(member, pn);

                if (pt == typeof(string))
                {
                    lsr.TryAdd(indeedName, PropertyResolver.GetString(pn));
                    continue;
                }

                CompileProperty(
                    pt,
                    pn,
                    indeedName,
                    bsr,
                    btr,
                    lsr,
                    0);
            }

            var cr = new ClassResolver(lsr, CreateInstance(classType));
            var cn = classType.Name;
            var indeedName2 = GetAlias(classType, cn);
            bsr.TryAdd(cn, cr);
            bsr.TryAdd(indeedName2, cr);
            btr.TryAdd(classType, cr);
        }

        private static ClassResolver CompileContentLevelClass(Type classType, GlobalTypeClassResolvers btr,
            GlobalStringClassResolvers bsr, int depth)
        {
            //
            // 所有属性
            var members = GetProperties(classType);
            var lsr = new LocalPropertyResolvers(StringComparer.OrdinalIgnoreCase);

            //
            //
            foreach (var member in members)
            {
                var pt = member.PropertyType;
                var pn = member.Name;
                var indeedName = GetAlias(member, pn);

                if (pt == typeof(string))
                {
                    lsr.TryAdd(indeedName, PropertyResolver.GetString(pn));
                }
                else
                {
                    CompileProperty(
                        pt,
                        pn,
                        indeedName,
                        bsr,
                        btr,
                        lsr,
                        depth);
                }
            }

            var cr = new ClassResolver(lsr, CreateInstance(classType));
            var cn = classType.Name;
            var indeedName2 = GetAlias(classType, cn);
            bsr.TryAdd(cn, cr);
            bsr.TryAdd(indeedName2, cr);
            btr.TryAdd(classType, cr);

            return cr;
        }

        private static void CompileProperty(
            Type pt,
            string pn,
            string indeedName,
            GlobalStringClassResolvers bsr,
            GlobalTypeClassResolvers btr,
            LocalPropertyResolvers lsr,
            int depth)
        {
            if (depth > 64)
            {
                return;
            }

            if (pt.IsEnum)
            {
                lsr.TryAdd(indeedName, PropertyResolver.GetEnum(pt, pn));
                return;
            }

            if (pt.IsClass)
            {
                if (pt.IsDefined(typeof(TypeConverterAttribute)))
                {
                    try
                    {
                        var tca = pt.GetCustomAttribute<TypeConverterAttribute>();
                        var tct = tca!.Converter;
                        var expression = CreateInstance(tct);
                        var converter = (CustomConverter)expression();
                        lsr.TryAdd(indeedName, new ConverterPropertyResolver(converter, pn));
                    }
                    catch
                    {
                        //
                    }

                    return;
                }

                if (pt.IsAbstract)
                {
                    return;
                }

                if (!btr.TryGetValue(pt, out var resolver))
                {
                    resolver = CompileContentLevelClass(pt, btr, bsr, depth + 1);
                    btr.TryAdd(pt, resolver);
                }

                lsr.TryAdd(indeedName, new ClassPropertyResolver(resolver, pn));
                return;
            }

            lsr.TryAdd(indeedName, PropertyResolver.GetValue(pt, pn));
        }

        #endregion

        public long ElapsedTicks { get; }
        public long ElapsedMilliseconds { get; }
    }
}