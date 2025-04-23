using System.Security.Cryptography;
using System.Text;

namespace cripto_reader
{
    static class AesCrypt
    {
        public static byte[] Encrypt(string plainText, string password)
        {
            if (plainText is null)
                throw new ArgumentNullException(nameof(plainText));
            if (password is null)
                throw new ArgumentNullException(nameof(password));

            using var aes = Aes.Create();
            aes.Key = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            aes.IV = MD5.HashData(Encoding.UTF8.GetBytes(password));

            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                cs.Write(Encoding.UTF8.GetBytes(plainText));
            
            return ms.ToArray();
        }

        public static string Decrypt(byte[] cipherText, string password)
        {
            if (cipherText is null)
                throw new ArgumentNullException(nameof(cipherText));
            if (password is null)
                throw new ArgumentNullException(nameof(password));

            using var aes = Aes.Create();
            aes.Key = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            aes.IV = MD5.HashData(Encoding.UTF8.GetBytes(password));

            using var ms = new MemoryStream(cipherText);
            using var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);
            
            return sr.ReadToEnd();
        }
    }
}