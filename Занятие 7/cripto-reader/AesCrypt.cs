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
            using (SHA256 sha256 = SHA256.Create())
            using (MD5 md5 = MD5.Create())
            {
                byte[] key = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                byte[] iv = md5.ComputeHash(Encoding.UTF8.GetBytes(password));

                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = key;
                    aesAlg.IV = iv;

                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, aesAlg.CreateEncryptor(), CryptoStreamMode.Write))
                        using (StreamWriter writer = new StreamWriter(cs))
                        {
                            writer.Write(plainText);
                        }
                        return ms.ToArray();
                    }
                }
            }
        }
        public static string Decrypt(byte[] cipherText, string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            using (MD5 md5 = MD5.Create())
            {
                byte[] key = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                byte[] iv = md5.ComputeHash(Encoding.UTF8.GetBytes(password));

                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = key;
                    aesAlg.IV = iv;

                    using (MemoryStream ms = new MemoryStream(cipherText))
                    {
                        using (CryptoStream cs = new CryptoStream(ms, aesAlg.CreateDecryptor(), CryptoStreamMode.Read))
                        using (StreamReader reader = new StreamReader(cs))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
