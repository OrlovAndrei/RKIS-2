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
			if (string.IsNullOrEmpty(plainText))
				return new byte[0];

			byte[] key = SHA256.HashData(Encoding.UTF8.GetBytes(password));
			byte[] iv = MD5.HashData(Encoding.UTF8.GetBytes(password));

			using var aes = Aes.Create();
			aes.Key = key;
			aes.IV = iv;

			using var memoryStream = new MemoryStream();
			using var cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write);
			using var streamWriter = new StreamWriter(cryptoStream);
			
			streamWriter.Write(plainText);
			streamWriter.Flush();
			cryptoStream.FlushFinalBlock();
			
			return memoryStream.ToArray();
		}

		public static string Decrypt(byte[] cipherText, string password)
		{
			if (cipherText == null || cipherText.Length == 0)
				return string.Empty;

			byte[] key = SHA256.HashData(Encoding.UTF8.GetBytes(password));
			byte[] iv = MD5.HashData(Encoding.UTF8.GetBytes(password));

			using var aes = Aes.Create();
			aes.Key = key;
			aes.IV = iv;

			using var memoryStream = new MemoryStream(cipherText);
			using var cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Read);
			using var streamReader = new StreamReader(cryptoStream);
			
			return streamReader.ReadToEnd();
		}
	}
}
