using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace cripto_reader
{
    static class AesCrypt
    {
        private static void GetKeyAndIV(string password, out byte[] key, out byte[] iv)
        {
            using (SHA256 sha256 = SHA256.Create())
            using (MD5 md5 = MD5.Create())
            {
                key = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                iv = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        public static byte[] Encrypt(string plainText, string password)
        {
            if (plainText == null)
                throw new ArgumentNullException(nameof(plainText));
            if (password == null)
                throw new ArgumentNullException(nameof(password));

            GetKeyAndIV(password, out byte[] key, out byte[] iv);

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                using (MemoryStream memoryStream = new MemoryStream())
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                using (StreamWriter writer = new StreamWriter(cryptoStream, Encoding.UTF8))
                {
                    writer.Write(plainText);
                    writer.Flush();
                    cryptoStream.FlushFinalBlock();
                    return memoryStream.ToArray();
                }
            }
        }

        public static string Decrypt(byte[] cipherText, string password)
        {
            if (cipherText == null)
                throw new ArgumentNullException(nameof(cipherText));
            if (password == null)
                throw new ArgumentNullException(nameof(password));

            GetKeyAndIV(password, out byte[] key, out byte[] iv);

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                using (MemoryStream memoryStream = new MemoryStream(cipherText))
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Read))
                using (StreamReader reader = new StreamReader(cryptoStream, Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
