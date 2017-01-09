using System.Collections.Generic;
using System.IO;

namespace CompressionTechniques.DEFLATE
{
    public static class DEFLATECompressor
    {
        private const string m_CompressorName = "DEFLATE";
        public static byte[] Compress(byte[] bytes)
        {
            MemoryStream ms = new MemoryStream();
            System.IO.Compression.DeflateStream zip = new System.IO.Compression.DeflateStream(ms, System.IO.Compression.CompressionMode.Compress, true);
            zip.Write(bytes, 0, bytes.Length);
            zip.Close();
            byte[] result = new byte[ms.Length];
            ms.Seek(0, SeekOrigin.Begin);
            ms.Read(result, 0, (int)ms.Length);
            ms.Close();
            return result;
        }
        public static byte[] Decompress(byte[] bytes)
        {
            MemoryStream ms = new MemoryStream(bytes);
            Stream zip = new System.IO.Compression.DeflateStream(ms, System.IO.Compression.CompressionMode.Decompress);
            List<byte> buffer = new List<byte>();
            int temp = -1;
            while ((temp = zip.ReadByte()) != -1)
                buffer.Add((byte)temp);
            ms.Close();
            zip.Close();
            return buffer.ToArray();
        }

        public static Stream Compress(Stream stream)
        {
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, (int)buffer.Length);
            return new MemoryStream(Compress(buffer));
        }
        public static Stream Decompress(Stream stream)
        {
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, (int)buffer.Length);
            return new MemoryStream(Decompress(buffer));
        }
        public static string CompressorName { get { return m_CompressorName; } }
    }
}
