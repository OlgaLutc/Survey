using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Encryption.BL
{
    public class SEncription
    {
        public byte[] EncryptStringToBytes(string plainText, string key)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            des.Key = Encoding.ASCII.GetBytes(key.PadRight(8, '0').Substring(0, 8)); // Убедимся, что ключ состоит из 8 байтов
            des.IV = Encoding.ASCII.GetBytes(key.PadRight(8, '0').Substring(0, 8)); // Инициализационный вектор также должен быть 8 байтов

            byte[] encrypted;

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    byte[] plaintextBytes = Encoding.UTF8.GetBytes(plainText);
                    //byte[] plaintextBytes = Encoding.ASCII.GetBytes(plainText);

                    cs.Write(plaintextBytes, 0, plaintextBytes.Length);
                    cs.FlushFinalBlock();
                }
                encrypted = ms.ToArray();
            }

            return encrypted;
        }



        public string DecryptStringFromBytes(byte[] cipherText, string key)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            des.Key = Encoding.UTF8.GetBytes(key);
            des.IV = Encoding.UTF8.GetBytes(key);

            string plaintext = null;

            using (MemoryStream ms = new MemoryStream(cipherText))
            {
                using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    using (StreamReader sr = new StreamReader(cs))
                    {
                        plaintext = sr.ReadToEnd();
                    }
                }
            }

            return plaintext;
        }
    }


}