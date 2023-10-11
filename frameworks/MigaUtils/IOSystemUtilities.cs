using Acorisoft.FutureGL.MigaUtils.Collections;

namespace Acorisoft.FutureGL.MigaUtils
{
    public static class IOSystemUtilities
    {
        private const double TBSize         = 1099511627776;
        private const double MaxGBSize      = TBSize - 1;
        private const double MinGBSize      = 1073741824;
        private const double MaxMBSize      = MinGBSize - 1;
        private const double MinMBSize      = 1048576;
        private const double MaxKBSize      = MinMBSize - 1;
        private const double MinKBSize      = 1024;
        private const string ConcatPattern  = "{0} {1}";
        private const string UnitOfByte     = "B";
        private const string UnitOfKByte    = "KB";
        private const string UnitOfMByte    = "MB";
        private const string UnitOfGigaByte = "GB";
        private const string UnitOfTeraByte = "TB";

        private static bool InRange(int value, double min, double max)
        {
            return value >= min && value <= max;
        }

        private static bool InRange(long value, double min, double max)
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

        /// <summary>
        /// 获得目录大小
        /// </summary>
        /// <param name="directory">要计算的目录</param>
        /// <param name="recursive">是否递归计算</param>
        /// <returns>返回指定目录的大小</returns>
        public static Task<long> GetFolderSize(string directory, bool recursive)
        {
            return Task.Run(() =>
            {
                var queue  = new Queue<string>();
                var list   = new List<FileInfo>(128);

                if (recursive)
                {
                    queue.Enqueue(directory);

                    while (queue.Count > 0)
                    {
                        var dir     = queue.Dequeue();
                        var subDirs = Directory.GetDirectories(dir);
                        list.AddMany(Directory.GetFiles(dir)
                                               .Select(x => new FileInfo(x))
                                               .Where(x => x.Exists));
                        queue.AddMany(subDirs);
                    }
                }
                else
                {
                    list.AddMany(Directory.GetFiles(directory)
                                           .Select(x => new FileInfo(x))
                                           .Where(x => x.Exists));
                }

                return list.Sum(x => x.Length);
            });
        }
    }
}