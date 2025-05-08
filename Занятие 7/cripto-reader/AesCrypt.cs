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
            using Aes aes = Aes.Create();
            using (SHA256 sha256 = SHA256.Create())
            {
                aes.Key = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            }

            aes.GenerateIV();

            using MemoryStream ms = new MemoryStream();

            ms.Write(aes.IV, 0, aes.IV.Length);

            using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
            {
                using StreamWriter sw = new StreamWriter(cs);
                sw.Write(plainText);
            }

            return ms.ToArray();
        }

        public static string Decrypt(byte[] cipherText, string password)
        {
            if (cipherText == null || cipherText.Length < 16)
            {
                throw new CryptographicException("Invalid cipher text.");
            }

            byte[] iv = new byte[16];
            Array.Copy(cipherText, 0, iv, 0, iv.Length);

            byte[] cipher = new byte[cipherText.Length - iv.Length];
            Array.Copy(cipherText, iv.Length, cipher, 0, cipher.Length);

            using Aes aes = Aes.Create();

            using (SHA256 sha256 = SHA256.Create())
            {
                aes.Key = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            }

            aes.IV = iv;

            using MemoryStream ms = new MemoryStream(cipher);
            using CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read);
            using StreamReader sr = new StreamReader(cs);
            return sr.ReadToEnd();
        }
    }
}