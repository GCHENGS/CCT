﻿using System.IO;
using System.Security.Cryptography;

namespace CCT.Resource.Helpers
{
    /// <summary>
    /// 加密类 
    /// </summary>
    public class CryptInfo
    {
        /// <summary>
        /// 私有构造方法禁止实例化该类型
        /// </summary>
        public CryptInfo() { }

        /// <summary>
        /// 对输入的字符串进行加密，并获取加密后的字符串
        /// </summary>
        /// <param name="text">输入的字符串</param>
        /// <returns></returns>
        public static string GetEncrypt(string text)
        {
            return Encrypt(text);
        }

        /// <summary>
        /// 加密 对用户名和密码进行加密的方法
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static string Encrypt(string text)
        {
            Rijndael crypt = Rijndael.Create();
            byte[] key = new byte[32] { 0XA6, 0X7D, 0XE1, 0X3F, 0X35, 0X0E, 0XE1, 0XA9, 0X83, 0XA5, 0X62, 0XAA, 0X7A, 0XAE, 0X79, 0X98, 0XA7, 0X33, 0X49, 0XFF, 0XE6, 0XAE, 0XBF, 0X8D, 0X8D, 0X20, 0X8A, 0X49, 0X31, 0X3A, 0X12, 0X40 };

            byte[] iv = new byte[16] { 0XF8, 0X8B, 0X01, 0XFB, 0X08, 0X85, 0X9A, 0XA4, 0XBE, 0X45, 0X28, 0X56, 0X03, 0X42, 0XF6, 0X19 };
            crypt.Key = key;
            crypt.IV = iv;

            MemoryStream ms = new MemoryStream();

            ICryptoTransform transtormEncode = new ToBase64Transform();
            //Base64编码
            CryptoStream csEncode = new CryptoStream(ms, transtormEncode, CryptoStreamMode.Write);

            CryptoStream csEncrypt = new CryptoStream(csEncode, crypt.CreateEncryptor(), CryptoStreamMode.Write);

            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
            byte[] rawData = enc.GetBytes(text);

            csEncrypt.Write(rawData, 0, rawData.Length);
            csEncrypt.FlushFinalBlock();

            byte[] encryptedData = new byte[ms.Length];
            ms.Position = 0;
            ms.Read(encryptedData, 0, (int)ms.Length);

            return enc.GetString(encryptedData);
        }

    }
}
