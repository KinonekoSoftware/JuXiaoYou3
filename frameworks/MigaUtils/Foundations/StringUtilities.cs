namespace Acorisoft.FutureGL.MigaUtils.Foundation
{
    public static class StringUtilities
    {
        public static string ReplaceCommonPrefix(this string pattern, string value)
        {
            if (string.IsNullOrEmpty(pattern) || string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }
            
            var diffIndex = 0;
            var minLength = Math.Min(pattern.Length, value.Length);
            
            for(var i =0; i< minLength;i++)
            {
                diffIndex = i;
                if (pattern[i] != value[i])
                {
                    break;
                }
            }

            var span = value.AsSpan()[diffIndex..];
            return new string(span);
        }
        
        public static string ReplaceCommonPrefix(this string pattern, string value, params string[] concat)
        {
            if (string.IsNullOrEmpty(pattern) || string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }
            
            var diffIndex = 0;
            var minLength = Math.Min(pattern.Length, value.Length);
            var sb        = Pool.GetStringBuilder();
            
            for(var i =0; i< minLength;i++)
            {
                diffIndex = i;
                if (pattern[i] != value[i])
                {
                    break;
                }
            }

            var span = value.AsSpan()[diffIndex..];
            sb.Append(span);
            foreach (var i in concat)
            {
                sb.Append(i);
            }

            var s = sb.ToString();
            Pool.ReturnStringBuilder(sb);
            return s;
        }
        
        public static string SubString(this string source, int maxLength = 100)
        {
            if (string.IsNullOrEmpty(source))
            {
                return string.Empty;
            }

            var length = Math.Min(maxLength, source.Length);
            return source[..length];
        }
        
        public static bool EqualsWithIgnoreCase(this string src, string dst)
        {
            return !string.IsNullOrEmpty(dst) && string.Equals(src, dst, StringComparison.OrdinalIgnoreCase);
        }

        public static string RemoveCharacters(this string src, params char[] removedCharacters)
        {
            if (string.IsNullOrEmpty(src)) return src;
            var length = src.Length;
            var hash = new HashSet<char>(removedCharacters);
            var sb = Pool.GetStringBuilder();

            for (var i = 0; i < length; i++)
            {
                if (hash.Contains(src[i])) continue;
                sb.Append(src[i]);
            }

            var s = sb.ToString();
            Pool.ReturnStringBuilder(sb);
            return s;
        }

        public static bool StartWithEx(this string src, char character)
        {
            if (string.IsNullOrEmpty(src)) return false;
            var length = src.Length;

            for (var i = 0; i < length; i++)
            {
                switch (src[i])
                {
                    case '\r':
                    case '\n':
                    case '\t':
                    case '\x20':
                        continue;
                }

                return src[i] == character;
            }

            return false;
        }

        public static int GetRepeatCount(this string src, char character, out int startIndex)
        {
            startIndex = -1;

            if (string.IsNullOrEmpty(src))
            {
                return 0;
            }

            var length = src.Length;
            var count = 0;

            for (var i = 0; i < length; i++)
            {
                switch (src[i])
                {
                    case '\r':
                    case '\n':
                    case '\t':
                    case '\x20':
                        continue;
                }

                if (src[i] == character)
                {
                    if (startIndex == -1)
                    {
                        startIndex = i;
                    }

                    count++;
                }
                else
                {
                    break;
                }
            }

            return count;
        }

        public static string Repeat(this string src, int count)
        {
            count = count == 0 ? 1 : count;
            var sb = Pool.GetStringBuilder();
            for (var i = 0; i < count; i++)
            {
                sb.Append(src);
            }

            var s = sb.ToString();
            Pool.ReturnStringBuilder(sb);
            return s;
        }

        public static bool ToBoolean(this string text, bool defaultValue)
        {
            return string.IsNullOrEmpty(text) ? 
                defaultValue :
                bool.TryParse(text, out var state) ?
                    state :
                    defaultValue;
        }
        
        public static double ToDouble(this string text, double defaultValue)
        {
            return string.IsNullOrEmpty(text) ? 
                defaultValue :
                double.TryParse(text, out var state) ?
                    state :
                    defaultValue;
        }
        
        public static float ToFloat(this string text, float defaultValue)
        {
            return string.IsNullOrEmpty(text) ? 
                defaultValue :
                float.TryParse(text, out var state) ?
                    state :
                    defaultValue;
        }
        
        public static int ToInt(this string text, int defaultValue)
        {
            return string.IsNullOrEmpty(text) ? 
                defaultValue :
                int.TryParse(text, out var state) ?
                    state :
                    defaultValue;
        }
    }
}