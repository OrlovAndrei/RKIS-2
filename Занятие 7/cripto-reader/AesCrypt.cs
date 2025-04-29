using System.Security.Cryptography;
using System.IO;
using System.Text;

namespace cripto_reader
{
    static class AesCrypt
    {
        public static byte[] Encrypt(string plainText, string password)
        {
            using var algorithm = Aes.Create();
            algorithm.Key = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            algorithm.IV = MD5.HashData(Encoding.UTF8.GetBytes(password));

            using var memoryStream = new MemoryStream();
            using (var cryptoStream = new CryptoStream(memoryStream, algorithm.CreateEncryptor(), CryptoStreamMode.Write))
            using (var streamWriter = new StreamWriter(cryptoStream))
                streamWriter.Write(plainText);

            return memoryStream.ToArray();
        }

        public static string Decrypt(byte[] cipherText, string password)
        {
            using var algorithm = Aes.Create();
            algorithm.Key = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            algorithm.IV = MD5.HashData(Encoding.UTF8.GetBytes(password));

            using var memoryStream = new MemoryStream(cipherText);
            using var cryptoStream = new CryptoStream(memoryStream, algorithm.CreateDecryptor(), CryptoStreamMode.Read);
            using var streamReader = new StreamReader(cryptoStream);
            return streamReader.ReadToEnd();

        }
    }
}
