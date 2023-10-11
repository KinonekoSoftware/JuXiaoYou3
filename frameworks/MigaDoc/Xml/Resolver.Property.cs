using System.Globalization;
using System.Linq;
using PropertySetter = Acorisoft.Miga.Utils.PropertySetter;

namespace Acorisoft.Miga.Xml
{
    public abstract class PropertyResolver : Resolver
    {
        #region HelperMethods

        private static readonly object True = true;
        private static readonly object False = false;

        private static readonly NumberFormatInfo nfi = new NumberFormatInfo();

        internal static object ToInt8(string value)
        {
            return byte.TryParse(value, NumberStyles.None, nfi, out var val) ? val : 0;
        }

        internal static object ToSInt8(string value)
        {
            return sbyte.TryParse(value, NumberStyles.None, nfi, out var val) ? val : 0;
        }

        internal static object ToInt16(string value)
        {
            return short.TryParse(value, NumberStyles.None, nfi, out var val) ? val : 0;
        }

        internal static object ToUInt16(string value)
        {
            return ushort.TryParse(value, NumberStyles.None, nfi, out var val) ? val : 0;
        }

        internal static object ToInt32(string value)
        {
            return int.TryParse(value, NumberStyles.None, nfi, out var val) ? val : 0;
        }

        internal static object ToUInt32(string value)
        {
            return uint.TryParse(value, NumberStyles.None, nfi, out var val) ? val : 0;
        }

        internal static object ToBoolean(string value)
        {
            return (bool.TryParse(value, out var val) ? val : false)? True : False;
        }

        internal static object ToInt64(string value)
        {
            return long.TryParse(value, NumberStyles.None, nfi, out var val) ? val : 0L;
        }

        internal static object ToUInt64(string value)
        {
            return ulong.TryParse(value, NumberStyles.None, nfi, out var val) ? val : 0L;
        }

        internal static object ToFP32(string value)
        {
            return float.TryParse(value, NumberStyles.None, nfi, out var val) ? val : 0f;
        }

        internal static object ToFP64(string value)
        {
            return double.TryParse(value, NumberStyles.None, nfi, out var val) ? val : 0d;
        }

        internal static object ToDecimal(string value)
        {
            return decimal.TryParse(value, NumberStyles.None, nfi, out var val) ? val : 0;
        }

        internal static object ToString(string value)
        {
            return value;
        }

        internal static object ToGuid(string value)
        {
            return Guid.TryParse(value, out var val) ? val : Guid.NewGuid();
        }

        internal static object ToTimeSpan(string value)
        {
            return TimeSpan.TryParse(value, out var val) ? val : TimeSpan.Zero;
        }

        internal static object ToDateTime(string value)
        {
            return DateTime.TryParse(value, out var val) ? val : new DateTime();
        }

        internal static object ToIntPtr(string value)
        {
            return IntPtr.TryParse(value, NumberStyles.None, nfi, out var val) ? val : 0;
        }

        internal static object ToUIntPtr(string value)
        {
            return UIntPtr.TryParse(value, NumberStyles.None, nfi, out var val) ? val : 0;
        }

        internal static object ToByteArray(string value)
        {
            try
            {
                return string.IsNullOrEmpty(value) ? null : Convert.FromBase64String(value);
            }
            catch
            {
                return Array.Empty<byte>();
            }
        }

        #endregion
        
        private static readonly Dictionary<Type, Func<string, ExpressionPropertyResolver>> _dictionary =
            new Dictionary<Type, Func<string, ExpressionPropertyResolver>>
            {
                { typeof(byte), x => new ExpressionPropertyResolver(ToInt8) { IndeedName = x } },
                { typeof(sbyte), x => new ExpressionPropertyResolver(ToSInt8) { IndeedName = x } },
                { typeof(short), x => new ExpressionPropertyResolver(ToInt16) { IndeedName = x } },
                { typeof(ushort), x => new ExpressionPropertyResolver(ToUInt16) { IndeedName = x } },
                { typeof(bool), x => new ExpressionPropertyResolver(ToBoolean) { IndeedName = x } },
                { typeof(int), x => new ExpressionPropertyResolver(ToInt32) { IndeedName = x } },
                { typeof(uint), x => new ExpressionPropertyResolver(ToUInt32) { IndeedName = x } },
                { typeof(long), x => new ExpressionPropertyResolver(ToInt64) { IndeedName = x } },
                { typeof(ulong), x => new ExpressionPropertyResolver(ToInt64) { IndeedName = x } },
                { typeof(float), x => new ExpressionPropertyResolver(ToFP32) { IndeedName = x } },
                { typeof(double), x => new ExpressionPropertyResolver(ToFP64) { IndeedName = x } },
                { typeof(decimal), x => new ExpressionPropertyResolver(ToDecimal) { IndeedName = x } },
                { typeof(Guid), x => new ExpressionPropertyResolver(ToGuid) { IndeedName = x } },
                { typeof(byte[]), x => new ExpressionPropertyResolver(ToByteArray) { IndeedName = x } },
                { typeof(string), x => new ExpressionPropertyResolver(ToString) { IndeedName = x } },
                { typeof(TimeSpan), x => new ExpressionPropertyResolver(ToTimeSpan) { IndeedName = x } },
                { typeof(DateTime), x => new ExpressionPropertyResolver(ToDateTime) { IndeedName = x } },
                { typeof(IntPtr), x => new ExpressionPropertyResolver(ToIntPtr) { IndeedName = x } },
                { typeof(UIntPtr), x => new ExpressionPropertyResolver(ToUIntPtr) { IndeedName = x } },
            };

        #region Nested Classes

        
        protected sealed class ExpressionPropertyResolver : PropertyResolver
        {
            private readonly Func<string, object> _expression;

            public ExpressionPropertyResolver(Func<string, object> expression) =>
                _expression = expression ?? throw new ArgumentNullException(nameof(expression));

            public override void Assign(PropertySetter proxy, object targetInstance, string value)
            {
                proxy[IndeedName, targetInstance] = _expression(value);
            }

            public override void Assign(PropertySetter proxy, object targetInstance, object value)
            {
                proxy[IndeedName, targetInstance] = value;
            }
        }

        protected sealed class EnumerationPropertyResolver : PropertyResolver
        {
            public readonly Dictionary<string, object> CacheValues;
        
            public EnumerationPropertyResolver(Type propertyType, Array values)
            {
                CacheValues = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            
                foreach (var value in values)
                {
                    CacheValues.Add(value.ToString()!, value);
                }

                ValueType = propertyType;
            }

            public override void Assign(PropertySetter proxy, object targetInstance, string value)
            {
                if (!CacheValues.TryGetValue(value, out var val))
                {
                    if (!Enum.TryParse(ValueType, value, out val))
                    {
                        val = CacheValues.Values.First();
                    }
                }
            
                proxy[IndeedName, targetInstance] = val;
            }

            public override void Assign(PropertySetter proxy, object targetInstance, object value)
            {
                proxy[IndeedName, targetInstance] = value;
            }
            
            public Type ValueType { get; }
        }
        
        #endregion

        #region Override

        
        public virtual void Assign(PropertySetter proxy, object targetInstance, string value)
        {
            throw new NotSupportedException();
        }

        public virtual void Assign(PropertySetter proxy, object targetInstance, object value)
        {
            throw new NotSupportedException();
        }


        #endregion

        #region Static Methods

        

        public static PropertyResolver GetString(string pn)
        {
            return GetValue(typeof(string), pn);
        }

        public static PropertyResolver GetValue(Type pt, string pn)
        {
            return !_dictionary.TryGetValue(pt, out var expression) ? null : expression(pn);
        }

        public static PropertyResolver GetEnum(Type pt, string pn)
        {
            var values = Enum.GetValues(pt);
            return new EnumerationPropertyResolver(pt, values)
            {
                IndeedName = pn
            };
        }

        #endregion
        
        /// <summary>
        /// 真实属性名
        /// </summary>
        public string IndeedName { get; init; }
    }
}   