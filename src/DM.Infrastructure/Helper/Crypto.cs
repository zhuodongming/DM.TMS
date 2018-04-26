using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DM.Infrastructure.Helper
{
    /// <summary>
    /// 加解密 Helper
    /// </summary>
    public sealed class Crypto
    {
        /// <summary>
        /// 获取md5签名
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string GetMD5(string content, Encoding encode = null)
        {
            if (encode == null)
            {
                encode = Encoding.UTF8;
            }

            byte[] bytes_in = encode.GetBytes(content);
            using (MD5 md5 = MD5.Create())
            {
                byte[] bytes_out = md5.ComputeHash(bytes_in);
                return BitConverter.ToString(bytes_out).Replace("-", "").ToLower();
            }
        }

        /// <summary>
        /// 获取md5签名
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string GetMD5(byte[] content)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] bytes_out = md5.ComputeHash(content);
                return BitConverter.ToString(bytes_out).Replace("-", "").ToLower();
            }
        }

        /// <summary>
        /// 获取文件的md5签名
        /// </summary>
        /// <param name="fs"></param>
        /// <returns></returns>
        public static string GetMD5FromFile(string filePath)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (MD5 md5 = MD5.Create())
                {
                    byte[] bytes_out = md5.ComputeHash(fs);
                    return BitConverter.ToString(bytes_out).Replace("-", "").ToLower();
                }
            }
        }

        /// <summary>
        /// 获取sha1签名
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string GetSHA1(string content, Encoding encode = null)
        {
            if (encode == null)
            {
                encode = Encoding.UTF8;
            }

            byte[] bytes_in = encode.GetBytes(content);
            using (SHA1 sha1 = SHA1.Create())
            {
                byte[] bytes_out = sha1.ComputeHash(bytes_in);
                return BitConverter.ToString(bytes_out).Replace("-", "").ToLower();
            }
        }

        /// <summary>
        /// 获取HMACSHA1签名
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string GetHMACSHA1(string content, string key)
        {
            byte[] bytes_in = Encoding.UTF8.GetBytes(content);
            using (HMACSHA1 hmacsha1 = new HMACSHA1(Encoding.UTF8.GetBytes(key)))
            {
                byte[] bytes_out = hmacsha1.ComputeHash(bytes_in);
                return Convert.ToBase64String(bytes_out);
            }
        }

        /// <summary>
        /// 获取sha256签名
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string GetSHA256(string content, Encoding encode = null)
        {
            if (encode == null)
            {
                encode = Encoding.UTF8;
            }

            byte[] bytes_in = encode.GetBytes(content);
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes_out = sha256.ComputeHash(bytes_in);
                return Convert.ToBase64String(bytes_out);
            }
        }

        /// <summary>
        /// 获取sha512签名
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string GetSHA512(string content, Encoding encode = null)
        {
            if (encode == null)
            {
                encode = Encoding.UTF8;
            }

            byte[] bytes_in = encode.GetBytes(content);
            using (SHA512 sha512 = SHA512.Create())
            {
                byte[] bytes_out = sha512.ComputeHash(bytes_in);
                return Convert.ToBase64String(bytes_out);
            }
        }
    }
}
