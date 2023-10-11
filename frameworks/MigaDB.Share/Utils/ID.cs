using System.Text.RegularExpressions;
// ReSharper disable PossibleInvalidCastException
// ReSharper disable ConvertIfStatementToSwitchStatement
// ReSharper disable MergeCastWithTypeCheck
// ReSharper disable SuggestVarOrType_SimpleTypes

namespace Acorisoft.FutureGL.MigaDB.Utils
{
    public static class ID
    {
        public readonly struct ShortGuid
        {
            private static readonly Regex GuidRegEx = new Regex("^[0-9a-fA-F]{32}$");

            private readonly Guid _guid;

            private readonly string _value;

            public static readonly ShortGuid Empty = new ShortGuid(Guid.Empty);

            public Guid Guid => _guid;

            public ShortGuid(string value)
            {
                _value = value;
                _guid  = Decode(value);
            }

            public ShortGuid(Guid guid)
            {
                _value = Encode(guid);
                _guid  = guid;
            }

            public override string ToString()
            {
                return _value;
            }

            public override bool Equals(object obj)
            {
                if (obj == null)
                {
                    throw new ArgumentNullException(nameof(obj));
                }

                if (obj is ShortGuid)
                {
                    ShortGuid shortGuid = (ShortGuid)obj;
                    Guid guid = _guid;
                    return guid.Equals(shortGuid._guid);
                }

                if (obj is Guid)
                {
                    Guid g = (Guid)obj;
                    Guid guid = _guid;
                    return guid.Equals(g);
                }

                if (obj is string)
                {
                    Guid guid = _guid;
                    return guid.Equals(((ShortGuid)obj)._guid);
                }

                return false;
            }

            public override int GetHashCode()
            {
                Guid guid = _guid;
                return guid.GetHashCode();
            }

            public static ShortGuid Parse(string value)
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException(nameof(value));
                }

                if (value.Trim() == "")
                {
                    throw new ArgumentNullException(nameof(value));
                }

                value = value.Trim();
                ShortGuid result;
                if (value.Length >= 22 && value.Length <= 24)
                {
                    value  = value.Replace("=", "");
                    result = new ShortGuid(value);
                }
                else
                {
                    if (value.Length < 32 || value.Length > 38)
                    {
                        throw new FormatException("String was not in a valid format.");
                    }

                    value = value.Replace("{", "").Replace("}", "").Replace("-", "");
                    if (!GuidRegEx.IsMatch(value))
                    {
                        throw new FormatException("Guid string contained invalid characters.");
                    }

                    result = new ShortGuid(new Guid(value));
                }

                return result;
            }

            public static ShortGuid NewGuid()
            {
                return new ShortGuid(Guid.NewGuid());
            }

            public static string Encode(string value)
            {
                return Encode(new Guid(value));
            }

            public static string Encode(Guid guid)
            {
                return Convert.ToBase64String(guid.ToByteArray())
                              .Replace("/", "_")
                              .Replace("+", "-")
                              .Substring(0, 22);
            }

            public static Guid Decode(string value)
            {
                value = value.Replace("_", "/").Replace("-", "+");
                return new Guid(value.EndsWith("==") ? Convert.FromBase64String(value) : Convert.FromBase64String(value + "=="));
            }

            public static bool operator ==(ShortGuid x, ShortGuid y)
            {
                Guid guid = x._guid;
                return guid.Equals(y._guid);
            }

            public static bool operator !=(ShortGuid x, ShortGuid y)
            {
                return !(x == y);
            }

            public static implicit operator string(ShortGuid shortGuid)
            {
                return shortGuid._value;
            }

            public static implicit operator Guid(ShortGuid shortGuid)
            {
                return shortGuid._guid;
            }

            public static implicit operator ShortGuid(string shortGuid)
            {
                return new ShortGuid(shortGuid);
            }

            public static implicit operator ShortGuid(Guid guid)
            {
                return new ShortGuid(guid);
            }
        }

        public static string Get() => ShortGuid.NewGuid().ToString();
    }
}