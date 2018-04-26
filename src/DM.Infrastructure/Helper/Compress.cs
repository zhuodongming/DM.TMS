using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace DM.Infrastructure.Helper
{
    /// <summary>
    /// 压缩解压 Helper
    /// </summary>
    public sealed class Compress
    {
        /// <summary>
        /// GZip压缩
        /// </summary>
        /// <param name="sourceStream"></param>
        /// <returns></returns>
        public static MemoryStream GZip(Stream sourceStream)
        {
            try
            {
                sourceStream.Position = 0;
                MemoryStream responseStream = new MemoryStream();
                using (GZipStream gzipStream = new GZipStream(responseStream, CompressionMode.Compress, true))
                {
                    sourceStream.CopyTo(gzipStream);
                }
                responseStream.Position = 0;
                return responseStream;
            }
            catch
            {
                Log.Error("GZip压缩出错");
                throw;
            }
        }

        /// <summary>
        /// GZip解压缩
        /// </summary>
        /// <param name="sourceStream"></param>
        /// <returns></returns>
        public static MemoryStream UnGZip(Stream sourceStream)
        {
            try
            {
                sourceStream.Position = 0;
                MemoryStream responseStream = new MemoryStream();
                using (GZipStream gzipStream = new GZipStream(sourceStream, CompressionMode.Decompress, true))
                {
                    gzipStream.CopyTo(responseStream);
                }
                responseStream.Position = 0;
                return responseStream;
            }
            catch
            {
                Log.Error("GZip解压缩出错");
                throw;
            }
        }

        /// <summary>
        /// Zip压缩
        /// </summary>
        /// <param name="sourceStream"></param>
        /// <returns></returns>
        public static MemoryStream Zip(Stream sourceStream)
        {
            try
            {
                sourceStream.Position = 0;
                MemoryStream responseStream = new MemoryStream();
                using (ZipArchive archive = new ZipArchive(responseStream, ZipArchiveMode.Update, true))
                {
                    ZipArchiveEntry readmeEntry = archive.CreateEntry("file");
                    using (var zipStream = readmeEntry.Open())
                    {
                        sourceStream.CopyTo(zipStream);
                    }
                }
                responseStream.Position = 0;
                return responseStream;
            }
            catch
            {
                Log.Error("Zip压缩出错");
                throw;
            }
        }


        /// <summary>
        /// Zip解压缩
        /// </summary>
        /// <param name="sourceStream"></param>
        /// <returns></returns>
        public static MemoryStream UnZip(Stream sourceStream)
        {
            try
            {
                sourceStream.Position = 0;
                MemoryStream responseStream = new MemoryStream();
                using (ZipArchive archive = new ZipArchive(sourceStream, ZipArchiveMode.Update))
                {
                    var entry = archive.Entries[0];
                    using (var zipStream = entry.Open())
                    {
                        zipStream.CopyTo(responseStream);
                    }
                }
                responseStream.Position = 0;
                return responseStream;
            }
            catch (Exception e)
            {
                Log.Error("Zip解压缩出错");
                throw e;
            }
        }
    }
}
