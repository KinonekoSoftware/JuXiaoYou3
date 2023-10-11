using System.Runtime.InteropServices;
using System.Text;
using Koyashiro.PngChunkUtil;

namespace Acorisoft.FutureGL.MigaDB.Utils
{
    public static class PNG
    {
        public static async Task Write(string fileName, string payload, byte[] png)
        {
            var payloadBuffer = Encoding.UTF8.GetBytes(payload);
            await Write(fileName, payloadBuffer, png);
        }

        public static async Task Write(string fileName, byte[] payload, byte[] png)
        {
            var chunks = PngReader.ReadBytes(png);
            var chunks1 = new Chunk[chunks.Length];
            var IEND = Chunk.Create("IEND", payload);
            for (var i = 0; i < chunks.Length - 1; i++)
            {
                chunks1[i] = chunks[i];
            }

            chunks1[^1] = IEND;
            var buffer = PngWriter.WriteBytes(CollectionsMarshal.AsSpan<Chunk>(chunks1.ToList()));
            await File.WriteAllBytesAsync(fileName, buffer);
        }

        public static async Task<string> ReadDataAsync(string fileName)
        {
            var dataPackets = await File.ReadAllBytesAsync(fileName);
            var chunks = PngReader.ReadBytes(dataPackets);
            var IEND = chunks.Last();
            var buffer = IEND.Bytes.Slice(8, IEND.Length ?? 0).ToArray();

            try
            {
                var payload = Encoding.UTF8.GetString(buffer);
                return payload;
            }
            catch
            {
                return "{}";
            }
        }

        public static string ReadData(byte[] dataPackets)
        {
            var chunks = PngReader.ReadBytes(dataPackets);
            var IEND = chunks.Last();
            var buffer = IEND.Bytes.Slice(8, IEND.Length ?? 0).ToArray();

            try
            {
                var payload = Encoding.UTF8.GetString(buffer);
                return payload;
            }
            catch
            {
                return "{}";
            }
        }

        public static async Task<byte[]> ReadImage(string fileName)
        {
            var dataPackets = await File.ReadAllBytesAsync(fileName);
            return ReadImage(dataPackets);
        }

        public static byte[] ReadImage(byte[] dataPackets)
        {
            var chunks = PngReader.ReadBytes(dataPackets);
            var IEND = Chunk.Create("IEND", Array.Empty<byte>());
            chunks[^1] = IEND;
            return PngWriter.WriteBytes(chunks);
        }
    }
}