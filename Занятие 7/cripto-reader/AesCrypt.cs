using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace cripto_reader
{
    static class AesCrypt
    {
        public static byte[] Encrypt(string plainText, string password)
        {
            // Генерация криптографических ключей из пароля
            byte[] key, iv;
            GenerateKeysFromPassword(password, out key, out iv);

            // Настройка AES алгоритма
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;  // Устанавливает ключ шифрования
                aesAlg.IV = iv;   // Устанавливает вектор инициализации

                // 3. Шифрование данных
                using (var memoryStream = new MemoryStream())
                {
                    // Создает криптопоток для шифрования
                    using (var cryptoStream = new CryptoStream(memoryStream, aesAlg.CreateEncryptor(), CryptoStreamMode.Write))
                    // Создает потоковый писатель для записи текста
                    using (var streamWriter = new StreamWriter(cryptoStream))
                    {
                        streamWriter.Write(plainText); // Записывает текст для шифрования
                    }

                    // Возвращает зашифрованные байты
                    return memoryStream.ToArray();
                }
            }
        }

        public static string Decrypt(byte[] cipherText, string password)
        {
            // Генерация ключей из пароля, которые должны совпадать с теми, что при шифровании
            byte[] key, iv;
            GenerateKeysFromPassword(password, out key, out iv);

            // Настройка AES алгоритма
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;  // Устанавливает тот же ключ
                aesAlg.IV = iv;    // Устанавливает тот же вектор инициализации

                // Дешифрование данных
                using (var memoryStream = new MemoryStream(cipherText))
                // Создает криптопоток для дешифрования
                using (var cryptoStream = new CryptoStream(memoryStream, aesAlg.CreateDecryptor(), CryptoStreamMode.Read))
                // Создает потоковый читатель для чтения расшифрованного текста
                using (var streamReader = new StreamReader(cryptoStream))
                {
                    return streamReader.ReadToEnd(); // Возвращает расшифрованный текст
                }
            }
        }
        // Описание для меня дебила, который не может все сразу понять
        /// <summary>
        /// Генерирует ключ и вектор инициализации из пароля
        /// </summary>
        /// <param name="password">Пароль для генерации ключей</param>
        /// <param name="key">Выходной параметр: 256-битный ключ (32 байта)</param>
        /// <param name="iv">Выходной параметр: 128-битный вектор инициализации (16 байт)</param>
        private static void GenerateKeysFromPassword(string password, out byte[] key, out byte[] iv)
        {
            // Для генерации ключа использует SHA256 (256 бит = 32 байта)
            using (SHA256 sha256 = SHA256.Create())
                key = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

            // Для генерации вектора инициализации использует MD5 (128 бит = 16 байт)
            using (MD5 md5 = MD5.Create())
                iv = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }
}