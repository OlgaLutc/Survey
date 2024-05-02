using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Encryption.BL
{
    public class Signature
    {

        public static string CreateSignature(string data)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Хешируем данные
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(data));

                // Преобразуем байты в строку шестнадцатеричного формата
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        // Метод для проверки электронной подписи
        public static bool VerifySignature(string data, string signature)
        {
            // Создаем новую подпись для данных и сравниваем ее с предоставленной подписью
            string newSignature = CreateSignature(data);
            return newSignature.Equals(signature, StringComparison.OrdinalIgnoreCase);
        }
    }
}