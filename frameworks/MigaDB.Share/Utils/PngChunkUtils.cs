using System.Buffers.Binary;
using System.Globalization;
using System.IO.Hashing;
using System.Text;
using System.Diagnostics.CodeAnalysis;

namespace Koyashiro.PngChunkUtil
{
    public static class _Png
    {
        public static ReadOnlySpan<byte> Signature => new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };
    }
    
    public static class AncillaryChunk
    {
        /// <summary>
        /// cHRM
        /// </summary>
        public static ReadOnlySpan<byte> CHRM => new byte[] { 0x67, 0x41, 0x4D, 0x41 };

        /// <summary>
        /// gAMA
        /// </summary>
        public static ReadOnlySpan<byte> GAMA => new byte[] { 0x67, 0x41, 0x4D, 0x41 };

        /// <summary>
        /// iCCP
        /// </summary>
        public static ReadOnlySpan<byte> ICCP => new byte[] { 0x69, 0x43, 0x43, 0x50 };

        /// <summary>
        /// sBIT
        /// </summary>
        public static ReadOnlySpan<byte> SBIT => new byte[] { 0x73, 0x42, 0x49, 0x54 };

        /// <summary>
        /// sRGB
        /// </summary>
        public static ReadOnlySpan<byte> SRGB => new byte[] { 0x73, 0x52, 0x47, 0x42 };

        /// <summary>
        /// bKGD
        /// </summary>
        public static ReadOnlySpan<byte> BKGD => new byte[] { 0x62, 0x4B, 0x47, 0x44 };

        /// <summary>
        /// hIST
        /// </summary>
        public static ReadOnlySpan<byte> HIST => new byte[] { 0x68, 0x49, 0x53, 0x54 };

        /// <summary>
        /// tRNS
        /// </summary>
        public static ReadOnlySpan<byte> TRNS => new byte[] { 0x74, 0x52, 0x4E, 0x53 };

        /// <summary>
        /// pHYs
        /// </summary>
        public static ReadOnlySpan<byte> PHYS => new byte[] { 0x70, 0x48, 0x59, 0x73 };

        /// <summary>
        /// sPLT
        /// </summary>
        public static ReadOnlySpan<byte> SPLT => new byte[] { 0x73, 0x50, 0x4C, 0x54 };

        /// <summary>
        /// tIME
        /// </summary>
        public static ReadOnlySpan<byte> TIME => new byte[] { 0x74, 0x49, 0x4D, 0x45 };

        /// <summary>
        /// iTXt
        /// </summary>
        public static ReadOnlySpan<byte> ITXT => new byte[] { 0x69, 0x54, 0x58, 0x74 };

        /// <summary>
        /// tEXt
        /// </summary>
        public static ReadOnlySpan<byte> TEXT => new byte[] { 0x69, 0x45, 0x58, 0x74 };

        /// <summary>
        /// zTXt
        /// </summary>
        public static ReadOnlySpan<byte> ZTXT => new byte[] { 0x7A, 0x54, 0x58, 0x74 };
    }
    
    public static class CriticalChunk
    {
        /// <summary>
        /// IHDR
        /// </summary>
        public static ReadOnlySpan<byte> IHDR => new byte[] { 0x49, 0x48, 0x44, 0x52 };

        /// <summary>
        /// PLTE
        /// </summary>
        public static ReadOnlySpan<byte> PLTE => new byte[] { 0x50, 0x4C, 0x54, 0x45 };

        /// <summary>
        /// IDAT
        /// </summary>
        public static ReadOnlySpan<byte> IDAT => new byte[] { 0x49, 0x44, 0x41, 0x54 };

        /// <summary>
        /// IEND
        /// </summary>
        public static ReadOnlySpan<byte> IEND => new byte[] { 0x49, 0x45, 0x4E, 0x44 };
    }
    
    public static class PngWriter
    {
        public static byte[] WriteBytes(ReadOnlySpan<Chunk> chunks)
        {
            if (chunks.Length == 0)
            {
                throw new ArgumentException("invalid chunks");
            }

            using var memoryStream = new MemoryStream();
            using var binaryWriter = new BinaryWriter(memoryStream);

            binaryWriter.Write(_Png.Signature);

            foreach (var chunk in chunks)
            {
                if (!chunk.IsValid())
                {
                    throw new ArgumentException("broken chunk");
                }

                binaryWriter.Write(chunk.Bytes);
            }

            return memoryStream.ToArray();
        }

        public static bool TryWriteBytes(ReadOnlySpan<Chunk> chunks, [MaybeNullWhen(false)] out byte[] output)
        {
            if (chunks.Length == 0)
            {
                output = default;
                return false;
            }

            using var memoryStream = new MemoryStream();
            using var binaryWriter = new BinaryWriter(memoryStream);

            binaryWriter.Write(_Png.Signature);

            foreach (var chunk in chunks)
            {
                if (!chunk.IsValid())
                {
                    output = default;
                    return false;
                }

                binaryWriter.Write(chunk.Bytes);
            }

            output = memoryStream.ToArray();
            return true;
        }
    }
    
    public static class PngReader
    {
        private static bool HasSignature(ReadOnlySpan<byte> source)
        {
            if (source.Length < 8)
            {
                return false;
            }

            return source[0..8].SequenceEqual(_Png.Signature);
        }

        public static Chunk[] ReadBytes(ReadOnlyMemory<byte> input)
        {
            if (!HasSignature(input.Span))
            {
                throw new ArgumentException("invalid signature", nameof(input));
            }

            var chunks = new List<Chunk>();

            var index = 8;
            while (index < input.Length)
            {
                if (index + 8 > input.Length)
                {
                    throw new ArgumentException("invalid png", nameof(input));
                }

                var length = BinaryPrimitives.ReadInt32BigEndian(input.Span[index..(index + 4)]) + 12;

                if (index + length > input.Length)
                {
                    throw new ArgumentException("invalid png", nameof(input));
                }

                var chunk = Chunk.Parse(input[index..(index + length)]);
                if (!chunk.IsValid())
                {
                    throw new ArgumentException("invalid png", nameof(input));
                }

                chunks.Add(chunk);

                if (CriticalChunk.IEND.SequenceEqual(chunk.ChunkTypeBytes))
                {
                    break;
                }

                index += length;
            }

            return chunks.ToArray();
        }

        public static bool TryReadBytes(ReadOnlyMemory<byte> input, [MaybeNullWhen(false)] out Chunk[] chunks)
        {
            if (!HasSignature(input.Span))
            {
                chunks = default;
                return false;
            }

            var list = new List<Chunk>();

            var index = 8;
            while (index < input.Length)
            {
                if (index + 8 > input.Length)
                {
                    chunks = default;
                    return false;
                }

                var length = BinaryPrimitives.ReadInt32BigEndian(input.Span[index..(index + 4)]) + 12;

                if (index + length > input.Length)
                {
                    chunks = default;
                    return false;
                }

                var chunk = Chunk.Parse(input[index..(index + length)]);
                if (!chunk.IsValid())
                {
                    chunks = default;
                    return false;
                }

                list.Add(chunk);

                if (CriticalChunk.IEND.SequenceEqual(chunk.ChunkTypeBytes))
                {
                    break;
                }

                index += length;
            }


            chunks = list.ToArray();
            return true;
        }
    }
    
    /// <summary>
    /// Chunk
    /// </summary>
    public readonly struct Chunk : IEquatable<Chunk>
    {
        private static readonly CultureInfo CULTURE_INFO = new CultureInfo("en-US");
        private static readonly Range LENGTH_RANGE = 0..4;
        private static readonly Range CHUNK_TYPE_RANGE = 4..8;
        private static readonly Range CHUNK_DATA_RANGE = 8..^4;
        private static readonly Range CRC_RANGE = ^4..;

        private static readonly Range CRC_TARGET_RANGE = 4..^4;

        private readonly ReadOnlyMemory<byte> _buffer;

        private Chunk(ReadOnlyMemory<byte> buffer)
        {
            _buffer = buffer;
        }

        public ReadOnlySpan<byte> Bytes => _buffer.Span;
        public ReadOnlySpan<byte> LengthBytes => _buffer.IsEmpty ? Span<byte>.Empty : _buffer.Span[LENGTH_RANGE];
        public ReadOnlySpan<byte> ChunkTypeBytes => _buffer.IsEmpty ? Span<byte>.Empty : _buffer.Span[CHUNK_TYPE_RANGE];
        public ReadOnlySpan<byte> ChunkDataBytes => _buffer.IsEmpty ? Span<byte>.Empty : _buffer.Span[CHUNK_DATA_RANGE];
        public ReadOnlySpan<byte> CrcBytes => _buffer.IsEmpty ? Span<byte>.Empty : _buffer.Span[CRC_RANGE];

        public int? Length => IsValid() ? BinaryPrimitives.ReadInt32BigEndian(_buffer.Span[LENGTH_RANGE]) : default(int?);
        public string ChunkType => IsValid() ? Encoding.UTF8.GetString(_buffer.Span[CHUNK_TYPE_RANGE]) : default(string);
        public uint? Crc => IsValid() ? BinaryPrimitives.ReadUInt32BigEndian(_buffer.Span[CRC_RANGE]) : default(uint?);

        public static Chunk Parse(ReadOnlyMemory<byte> input)
        {
            if (input.Length < 4)
            {
                throw new ArgumentException("`input.Length` must be grater than or equal to 4", nameof(input));
            }

            var length = BinaryPrimitives.ReadInt32BigEndian(input.Span[LENGTH_RANGE]);
            if (input.Length != length + 12)
            {
                throw new ArgumentException("`Length` and `input.Length` do not match", nameof(input));
            }

            return new Chunk(input);
        }

        public static bool TryParse(ReadOnlyMemory<byte> input, out Chunk chunk)
        {
            if (input.Length < 4)
            {
                chunk = default;
                return false;
            }

            var length = BinaryPrimitives.ReadInt32BigEndian(input.Span[LENGTH_RANGE]);
            if (input.Length != length + 12)
            {
                chunk = default;
                return false;
            }

            chunk = new Chunk(input);
            return true;
        }

        public static Chunk Create(ReadOnlySpan<byte> chunkType, ReadOnlySpan<byte> chunkData)
        {
            if (chunkType.Length != 4)
            {
                throw new ArgumentException("`chunkType.Length` must be 4", nameof(chunkType));
            }

            var buffer = new byte[12 + chunkData.Length];
            var span = buffer.AsSpan();
            BinaryPrimitives.WriteInt32BigEndian(span[LENGTH_RANGE], chunkData.Length);
            chunkType.CopyTo(span[CHUNK_TYPE_RANGE]);
            chunkData.CopyTo(span[CHUNK_DATA_RANGE]);

            Span<byte> hash = Crc32.Hash(span[CRC_TARGET_RANGE]);
            hash.Reverse();
            hash.CopyTo(span[CRC_RANGE]);

            return new Chunk(buffer);
        }

        public static bool TryCreate(ReadOnlySpan<byte> chunkType, ReadOnlySpan<byte> chunkData, out Chunk chunk)
        {
            if (chunkType.Length != 4)
            {
                chunk = default;
                return false;
            }

            var buffer = new byte[12 + chunkData.Length];
            var span = buffer.AsSpan();
            BinaryPrimitives.WriteInt32BigEndian(span[LENGTH_RANGE], chunkData.Length);
            chunkType.CopyTo(span[CHUNK_TYPE_RANGE]);
            chunkData.CopyTo(span[CHUNK_DATA_RANGE]);

            Span<byte> hash = Crc32.Hash(span[CRC_TARGET_RANGE]);
            hash.Reverse();
            hash.CopyTo(span[CRC_RANGE]);

            chunk = new Chunk(buffer);
            return true;
        }

        public static Chunk Create(string chunkType, ReadOnlySpan<byte> chunkData)
        {
            var chunkTypeBytes = Encoding.UTF8.GetBytes(chunkType);

            return Create(chunkTypeBytes, chunkData);
        }

        public static bool TryCreate(string chunkType, ReadOnlySpan<byte> chunkData, out Chunk chunk)
        {
            var chunkTypeBytes = Encoding.UTF8.GetBytes(chunkType);

            return TryCreate(chunkTypeBytes, chunkData, out chunk);
        }

        public static Chunk Create(string chunkType, string chunkData)
        {
            var chunkTypeBytes = Encoding.UTF8.GetBytes(chunkType);
            var chunkDataBytes = string.IsNullOrEmpty(chunkData) ? Array.Empty<byte>() : Encoding.UTF8.GetBytes(chunkData);

            return Create(chunkTypeBytes, chunkDataBytes);
        }

        public static bool TryCreate(string chunkType, string chunkData, out Chunk chunk)
        {
            var chunkTypeBytes = Encoding.UTF8.GetBytes(chunkType);
            var chunkDataBytes = string.IsNullOrEmpty(chunkData) ? Array.Empty<byte>() : Encoding.UTF8.GetBytes(chunkData);

            return TryCreate(chunkTypeBytes, chunkDataBytes, out chunk);
        }

        public bool IsValid()
        {
            if (_buffer.Length < 4)
            {
                return false;
            }

            if (_buffer.Length != 12 + BinaryPrimitives.ReadInt32BigEndian(_buffer.Span[LENGTH_RANGE]))
            {
                return false;
            }

            Span<byte> hash = Crc32.Hash(_buffer.Span[CRC_TARGET_RANGE]);
            hash.Reverse();
            if (!hash.SequenceEqual(_buffer.Span[CRC_RANGE]))
            {
                return false;
            }

            return true;
        }

        public override string ToString()
        {
            if (!IsValid())
            {
                return "Invalid Chunk";
            }

            var sb = Pool.GetStringBuilder();

            sb.Append("Length: ");
            sb.AppendFormat(CULTURE_INFO, "{0,5}", Length);
            sb.Append(",   ");

            sb.Append("ChunkType: ");
            sb.Append(ChunkType);
            sb.Append(",   ");

            sb.Append("ChunkData: ");
            sb.Append("[ ");
            for (var i = 0; i < 8; i++)
            {
                if (ChunkDataBytes.Length == i)
                {
                    break;
                }

                if (i != 0)
                {
                    sb.Append(", ");
                }
                sb.AppendFormat(CULTURE_INFO, "0x{0:x2}", ChunkDataBytes[i]);
            }

            if (ChunkDataBytes.Length > 8)
            {
                sb.Append(", ...");
            }
            sb.Append(" ]");

            var s = sb.ToString();
            Pool.ReturnStringBuilder(sb);
            return s;
        }

        public override bool Equals(object obj)
        {
            if (obj is Chunk other)
            {
                return Equals(other);
            }

            return false;
        }

        public bool Equals(Chunk other)
        {
            return Bytes.SequenceEqual(other.Bytes);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_buffer);
        }

        public static bool operator ==(Chunk left, Chunk right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Chunk left, Chunk right)
        {
            return !(left == right);
        }
    }
}