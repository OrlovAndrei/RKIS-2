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
				return Encoding.UTF8.GetBytes(string.Empty);

			byte[] key = SHA256.HashData(Encoding.UTF8.GetBytes(password));
			byte[] iv = MD5.HashData(Encoding.UTF8.GetBytes(password));

			using (Aes aes = Aes.Create())
			{
				aes.Key = key;
				aes.IV = iv;

				using (MemoryStream memoryStream = new MemoryStream())
				{
					using (CryptoStream cryptoStream = new CryptoStream(
						memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
					{
						using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
						{
							streamWriter.Write(plainText);
						}
					}
					return memoryStream.ToArray();
				}
			}
		}

		public static string Decrypt(byte[] cipherText, string password)
		{
			if (cipherText == null || cipherText.Length == 0)
				return string.Empty;

			byte[] key = SHA256.HashData(Encoding.UTF8.GetBytes(password));
			byte[] iv = MD5.HashData(Encoding.UTF8.GetBytes(password));

			using (Aes aes = Aes.Create())
			{
				aes.Key = key;
				aes.IV = iv;

				using (MemoryStream memoryStream = new MemoryStream(cipherText))
				{
					using (CryptoStream cryptoStream = new CryptoStream(
						memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Read))
					{
						using (StreamReader streamReader = new StreamReader(cryptoStream))
						{
							return streamReader.ReadToEnd();
						}
					}
				}
			}
		}
	}
}
