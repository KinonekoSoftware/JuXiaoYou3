using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.ObjectPool;

namespace Acorisoft.FutureGL.MigaUtils
{
    public class Pool
    {
        private static readonly ObjectPool<StringBuilder> StringBuilderPool;

        static Pool()
        {
            var objectPoolProvider = new DefaultObjectPoolProvider();
            StringBuilderPool  = objectPoolProvider.CreateStringBuilderPool(32, 128);
        }

        public static StringBuilder GetStringBuilder() => StringBuilderPool.Get();
        public static void ReturnStringBuilder(StringBuilder sb) => StringBuilderPool.Return(sb);
        public static readonly MD5  MD5  = MD5.Create();
        public static readonly SHA1 SHA1 = SHA1.Create();
    }
}