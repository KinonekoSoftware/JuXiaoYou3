using System.Xml.Linq;
using Acorisoft.Miga.Xml;

namespace Acorisoft.Miga.Doc.Parts
{
    public interface IFallbackSupport
    {
        string Fallback { get; }
    }
    
    
    
    public abstract class InputProperty : ICloneable
    {
        #region ICloneable Interface

        
        
        public object Clone()
        {
            //
            // 创建一个新的实例
            var instance = CreateInstanceOverride();

            ShadowCopy(instance);
            
            return instance;
        }

        /// <summary>
        /// 创建新的实例
        /// </summary>
        /// <returns>返回一个新的实例</returns>
        protected abstract InputProperty CreateInstanceOverride();

        /// <summary>
        /// 复制当前的数据
        /// </summary>
        /// <param name="target">要复制到的目标实例。</param>
        protected virtual void ShadowCopy(InputProperty target)
        {
            target.Name = CopyFrom(Name);
            target.ToolTips = CopyFrom(ToolTips);
            target.Metadata = CopyFrom(Metadata);
        }

        protected static string CopyFrom(string target)
        {
            if(target == null)
            {
                return string.Empty;
            }

            var buffer = target.ToCharArray();
            return new string(buffer);
        }

        #endregion

        #region GetElement

        public static XElement GetElement(InputProperty property)
        {
            return property.GetElementOverride();
        }

        protected internal virtual XElement GetElementOverride()
        {
            return null;
        }

        protected void Write(XElement element)
        {
            element.Add(new XAttribute("name", Name ?? string.Empty));
            element.Add(new XAttribute("meta", Metadata ?? string.Empty));
            element.Add(new XAttribute("tips", ToolTips ?? string.Empty));
        }

        #endregion

        private string _value;
        private string _name;

        public void SetValue(string value) => _value = value;

        /// <summary>
        /// 获取或设置 <see cref="Value"/> 属性。
        /// </summary>
        [Ignore]
        public string Value
        {
            get;
            set;
        }

        public virtual bool IsCompleted() => !string.IsNullOrEmpty(Name);

        // #region Transform
        //
        // protected internal abstract UnifiedProperty Transform();
        //
        // #endregion
        
        /// <summary>
        /// 获取或设置当前属性的名称。
        /// </summary>
        public string Name
        {
            get;
            set;
        }
        
        protected void SetValue<T>(ref T value, T source){}

        /// <summary>
        /// 获取或设置当前属性的提示。
        /// </summary>
        [Alias("tips")]
        public string ToolTips { get; set; }
        
        /// <summary>
        /// 获取或设置当前属性的元数据。
        /// </summary>
        [Alias("meta")]
        public string Metadata { get; set; }
    }
}