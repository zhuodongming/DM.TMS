using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace DM.Infrastructure.Helper
{
    /// <summary>
    /// RSA pkcs8 解密、签名、验签
    /// </summary>
    public sealed class RSAFromPkcs8
    {
        //RSA公钥加密
        public static string PublicEncrypt(string data, string publicKey)
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            var publicKeyParam = PublicKeyFactory.CreateKey(Convert.FromBase64String(publicKey));

            var rsa = new Pkcs1Encoding(new RsaEngine());
            rsa.Init(true, publicKeyParam);//参数true表示加密/false表示解密。
            dataBytes = rsa.ProcessBlock(dataBytes, 0, dataBytes.Length);
            return BitConverter.ToString(dataBytes).Replace("-", "");
        }

        //RSA私钥解密
        public static string PrivateDecrypt(string data, string privateKey)
        {
            byte[] dataBytes = GetBytesFromHex(data);
            var privateKeyParam = PrivateKeyFactory.CreateKey(Convert.FromBase64String(privateKey));

            var rsa = new Pkcs1Encoding(new RsaEngine());
            rsa.Init(false, privateKeyParam);//参数true表示加密/false表示解密。  
            dataBytes = rsa.ProcessBlock(dataBytes, 0, dataBytes.Length);
            return Encoding.UTF8.GetString(dataBytes);
        }

        //RSA私钥加密
        public static string PrivateEncrypt(string data, string privateKey)
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            var privateKeyParam = PrivateKeyFactory.CreateKey(Convert.FromBase64String(privateKey));

            var rsa = new Pkcs1Encoding(new RsaEngine());
            rsa.Init(true, privateKeyParam);//参数true表示加密/false表示解密。  
            dataBytes = rsa.ProcessBlock(dataBytes, 0, dataBytes.Length);
            return BitConverter.ToString(dataBytes).Replace("-", "");
        }

        //RSA公钥解密
        public static string PublicDecrypt(string data, string publicKey)
        {
            byte[] dataBytes = GetBytesFromHex(data);
            var publicKeyParam = PublicKeyFactory.CreateKey(Convert.FromBase64String(publicKey));

            var rsa = new Pkcs1Encoding(new RsaEngine());
            rsa.Init(false, publicKeyParam);//参数true表示加密/false表示解密。  
            dataBytes = rsa.ProcessBlock(dataBytes, 0, dataBytes.Length);
            return Encoding.UTF8.GetString(dataBytes);
        }

        //RSA签名
        public static string Sign(string data, string privateKey, string hashAlgorithm = "MD5withRSA")
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            var privateKeyParam = PrivateKeyFactory.CreateKey(Convert.FromBase64String(privateKey));

            ISigner signer = SignerUtilities.GetSigner(hashAlgorithm);
            signer.Init(true, privateKeyParam);//参数为true验签，参数为false加签  
            signer.BlockUpdate(dataBytes, 0, dataBytes.Length);
            return Convert.ToBase64String(signer.GenerateSignature());
        }

        //RSA验签
        public static bool Verify(string data, string sign, string publicKey, string hashAlgorithm = "MD5withRSA")
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            byte[] signBytes = Convert.FromBase64String(sign);
            var publicKeyParam = PublicKeyFactory.CreateKey(Convert.FromBase64String(publicKey));

            ISigner signer = SignerUtilities.GetSigner(hashAlgorithm);
            signer.Init(false, publicKeyParam);
            signer.BlockUpdate(dataBytes, 0, dataBytes.Length);
            return signer.VerifySignature(signBytes);
        }

        //16进制字符串数据转换为字节数组
        private static byte[] GetBytesFromHex(string strData)
        {
            byte[] data = new byte[strData.Length / 2];
            for (int i = 0; i < data.Length; i++)
            {
                string strByte = strData.Substring(2 * i, 2);
                byte b = Convert.ToByte(strByte, 16);
                data[i] = b;
            }
            return data;
        }
    }
}
