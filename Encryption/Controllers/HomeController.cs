using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Encryption.BL;
using Encryption.Models;
using static Encryption.BL.AEncription;


namespace Encryption.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        /*[HttpPost]
        public ActionResult EForm(string word, string encryptionType)
        {
            var model = new EncryptionModel { EncryptedText = word };

            if (encryptionType == "S")
            {
                // Создаем экземпляр класса SEncription
                SEncription encryption = new SEncription();
                

                // Зашифровываем слово
                encryption.EncryptStringToBytes(model.EncryptedText, "OlgaLutc");
            }
            else if (encryptionType == "A")
            {
                // Логика для другого типа шифрования (если нужно)
            }

            return View(model);
        }*/

        public ActionResult EForm(string word, string encryptionType)
        {
            if (!string.IsNullOrEmpty(word) && word is string plainText)
            {
                string original = word;
                string key = "OlgaLutc"; // Ключ для DES

                // Создаем экземпляр класса SEncryption
                SEncription encryption = new SEncription();

                byte[] encryptedBytes;
                string encryptedText = "";
                string decryptedText = ""; // Для расшифрованного текста

                if (encryptionType == "Encrypt")
                {
                    // Шифруем строку, если выбрано значение "S"
                    encryptedBytes = encryption.EncryptStringToBytes(original, key);
                    // Преобразуем зашифрованные байты в строку Base64 для удобства отображения
                    encryptedText = Convert.ToBase64String(encryptedBytes);
                    decryptedText = original; // Для отображения изначального слова
                }
                else if (encryptionType == "Decrypt")
                {
                    byte[] byteArray = Convert.FromBase64String(original);
                    decryptedText = encryption.DecryptStringFromBytes(byteArray, key);
                    encryptedText = original; // Для отображени
                }

                // Создаем объект модели представления и устанавливаем зашифрованный и расшифрованный текст
                EncryptionModel model = new EncryptionModel
                {
                    Word = word,
                    EncryptedText = encryptedText,
                    DecryptedText = decryptedText,
                    ActionType = encryptionType
                };

                // Возвращаем представление с моделью
                return View(model);
            }

            // Если слово не было введено или не выбран тип шифрования, возвращаем пустое представление
            return View(new EncryptionModel());
        }
        public ActionResult AForm(string word, string encryptionType)
        {
            // Параметры для алгоритма Диффи-Хеллмана
            int p = 23;
            int g = 5;
            int privateKey = 6;

            // Создаем экземпляр класса DiffieHellmanEncryption
            DiffieHellmanEncryption dh = new DiffieHellmanEncryption(p, g, privateKey);

            // Вычисление открытого ключа для текущей стороны
            int publicKey = dh.CalculatePublicKey();
            int otherPartyPublicKey = publicKey;

            // Вычисление общего секретного ключа
            byte[] secretKey = dh.CalculateSecretKey(otherPartyPublicKey);

            string encryptedText = "";
            string decryptedText = "";

            // Проверяем, что введено слово и выбран тип шифрования
            if (!string.IsNullOrEmpty(word) && !string.IsNullOrEmpty(encryptionType))
            {
                // Если выбрано шифрование
                if (encryptionType == "Encrypt")
                {
                    


                    byte[] data = Encoding.UTF8.GetBytes(word);
                    // Шифрование данных
                    byte[] encryptedBytes = dh.Encrypt(data, secretKey);
                    // Преобразуем зашифрованные байты в строку Base64
                    encryptedText = Convert.ToBase64String(encryptedBytes);

                    decryptedText = word;
                }
                // Если выбрано дешифрование
                else if (encryptionType == "Decrypt")
                {
                    byte[] encryptedData = Convert.FromBase64String(word);

                    // Дешифрование данных
                    byte[] decryptedData = dh.Decrypt(encryptedData, secretKey);
                    decryptedText = Encoding.UTF8.GetString(decryptedData);
                    encryptedText = word;
                }
            }

            // Создаем объект модели представления и устанавливаем значения
            EncryptionModel model = new EncryptionModel
            {
                Word = word,
                EncryptedText = encryptedText,
                DecryptedText = decryptedText,
                ActionType = encryptionType
            };

            // Возвращаем представление с моделью
            return View(model);
        }

        public ActionResult Sign(string data, string signature, string encryptionType)
        {
            if (encryptionType == "Sign")
            {
                // Создаем подпись
                signature = Signature.CreateSignature(data);
            }
            else if (encryptionType == "Verify")
            {
                // Выполняем проверку подписи
                bool isVerified = Signature.VerifySignature(data, signature);

                // Устанавливаем результат проверки во ViewBag для отображения в представлении
                if (isVerified)
                {
                    ViewBag.VerificationResult = "Подпись верна.";
                }
                else
                {
                    ViewBag.VerificationResult = "Подпись не верна.";
                }
            }

            // Создаем объект модели представления и устанавливаем значения
            EncryptionModel model = new EncryptionModel
            {
                Word = data, // Подставляем в модель введенные данные для отображения в представлении
                EncryptedText = signature, // Подставляем в модель подпись для отображения в представлении
                ActionType = encryptionType
            };

            // Возвращаем представление с моделью
            return View(model);
        }


    }
}