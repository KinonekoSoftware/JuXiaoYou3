namespace ConsoleApp
{

public static class ByteUtilities
    {
        public const double TBSize         = 1099511627776;
        public const double MaxGBSize      = TBSize - 1;
        public const double MinGBSize      = 1073741824;
        public const double MaxMBSize      = MinGBSize - 1;
        public const double MinMBSize      = 1048576;
        public const double MaxKBSize      = MinMBSize - 1;
        public const double MinKBSize      = 1024;
        public const string ConcatPattern  = "{0}{1}";
        public const string ConcatPattern2  = "{0}.{1}{2}";
        public const string UnitOfByte     = "B";
        public const string UnitOfKByte    = "KB";
        public const string UnitOfMByte    = "MB";
        public const string UnitOfGigaByte = "GB";
        public const string UnitOfTeraByte = "TB";

        public static bool InRange(int value, double min, double max)
        {
            return value >= min && value <= max;
        }
        
        public static bool InRange(long value, double min, double max)
        {
            return value >= min && value <= max;
        }
        
        /// <summary>
        /// 转换成B KB MB GB TB单位
        /// </summary>
        /// <returns></returns>
        public static string GetFileSizeString(int length)
        {
            double d;
            
            if (length < MinKBSize)
            {
                return string.Format(ConcatPattern, length, UnitOfByte);
            }

            if (InRange(length, MinKBSize, MaxKBSize))
            {
                d = Math.Round(length / MinKBSize, 2);
                return string.Format(ConcatPattern, d, UnitOfKByte);
            }
            
            if (InRange(length, MinMBSize, MaxMBSize))
            {
                d = Math.Round(length / MinMBSize, 2);
                return string.Format(ConcatPattern, d, UnitOfMByte);
            }
            
            
            d = Math.Round(length / MinGBSize, 2);
            return string.Format(ConcatPattern, d, UnitOfGigaByte);
        }
        
        /// <summary>
        /// 转换成B KB MB GB TB单位
        /// </summary>
        /// <returns></returns>
        public static string GetFileSizeString(long length)
        {
            double d;
            if (length < MinKBSize)
            {
                return string.Format(ConcatPattern, length, UnitOfByte);
            }

            if (InRange(length, MinKBSize, MaxKBSize))
            {
                d = Math.Round(length / MinKBSize, 2);
                return string.Format(ConcatPattern, d, UnitOfKByte);
            }
            
            if (InRange(length, MinMBSize, MaxMBSize))
            {
                d = Math.Round(length / MinMBSize, 2);
                return string.Format(ConcatPattern, d, UnitOfMByte);
            }
            
            if (InRange(length, MinGBSize, MaxGBSize))
            {
                d = Math.Round(length / MinGBSize, 2);
                return string.Format(ConcatPattern, d, UnitOfGigaByte);
            }
            
            d = Math.Round(length / TBSize, 2);
            return string.Format(ConcatPattern, d, UnitOfTeraByte);
        }
    }
}