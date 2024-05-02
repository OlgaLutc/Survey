using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace Encryption.BL
{
    public class AEncription
    {
        public class DiffieHellmanEncryption
        {
            private int p; // Простое число
            private int g; // Первообразный корень по модулю p
            private int privateKey; // Секретный ключ

            public DiffieHellmanEncryption(int p, int g, int privateKey)
            {
                this.p = p;
                this.g = g;
                this.privateKey = privateKey;
            }

            // Вычисление открытого ключа
            public int CalculatePublicKey()
            {
                return ModuloPower(g, privateKey, p);
            }

            // Вычисление общего секретного ключа
            public byte[] CalculateSecretKey(int otherPartyPublicKey)
            {
                int secretKeyInt = ModuloPower(otherPartyPublicKey, privateKey, p);
                // Преобразование числа общего секретного ключа в байтовый массив
                byte[] secretKeyBytes = BitConverter.GetBytes(secretKeyInt);
                // Хэширование байтового массива, чтобы получить ключ нужного размера
                using (SHA256 sha256 = SHA256.Create())
                {
                    return sha256.ComputeHash(secretKeyBytes);
                }
            }

            // Возведение числа в степень по модулю
            private int ModuloPower(int number, int exponent, int modulus)
            {
                if (exponent == 0)
                    return 1;

                long result = 1;
                long baseValue = number % modulus;

                while (exponent > 0)
                {
                    if (exponent % 2 == 1)
                        result = (result * baseValue) % modulus;

                    exponent >>= 1;
                    baseValue = (baseValue * baseValue) % modulus;
                }

                return (int)result;
            }

            // Шифрование данных
            public byte[] Encrypt(byte[] data, byte[] key)
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = key;
                    aes.IV = new byte[16]; // Используем нулевой вектор инициализации для простоты
                    ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                    using (var ms = new System.IO.MemoryStream())
                    {
                        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                        {
                            cs.Write(data, 0, data.Length);
                        }
                        return ms.ToArray();
                    }
                }
            }

            // Дешифрование данных
            public byte[] Decrypt(byte[] encryptedData, byte[] key)
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = key;
                    aes.IV = new byte[16]; // Используем нулевой вектор инициализации для простоты
                    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                    using (var ms = new System.IO.MemoryStream())
                    {
                        using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write))
                        {
                            cs.Write(encryptedData, 0, encryptedData.Length);
                        }
                        return ms.ToArray();
                    }
                }
            }
        }

    }
}