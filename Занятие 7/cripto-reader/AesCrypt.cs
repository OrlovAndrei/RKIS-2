﻿using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace cripto_reader
{
    static class AesCrypt
    {
        public static byte[] Encrypt(string plainText, string password)
        {
            byte[] key, iv;
            GenerateKeysFromPassword(password, out key, out iv);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, aesAlg.CreateEncryptor(), CryptoStreamMode.Write))
                    using (var streamWriter = new StreamWriter(cryptoStream))
                    {
                        streamWriter.Write(plainText);
                    }

                    return memoryStream.ToArray();
                }
            }
        }

        public static string Decrypt(byte[] cipherText, string password)
        {
            byte[] key, iv;
            GenerateKeysFromPassword(password, out key, out iv);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                using (var memoryStream = new MemoryStream(cipherText))
                using (var cryptoStream = new CryptoStream(memoryStream, aesAlg.CreateDecryptor(), CryptoStreamMode.Read))
                using (var streamReader = new StreamReader(cryptoStream))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }
        private static void GenerateKeysFromPassword(string password, out byte[] key, out byte[] iv)
        {
            using (SHA256 sha256 = SHA256.Create())
                key = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

            using (MD5 md5 = MD5.Create())
                iv = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }
}