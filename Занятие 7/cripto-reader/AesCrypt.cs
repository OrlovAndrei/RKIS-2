using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace cripto_reader
{
	static class AesCrypt
	{
		// Генерация ключа и IV на основе пароля
		private static void GenerateKeyAndIV(string password, out byte[] key, out byte[] iv)
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
			GenerateKeyAndIV(password, out byte[] key, out byte[] iv);

			using (Aes aes = Aes.Create())
			{
				aes.Key = key;
				aes.IV = iv;

				using (MemoryStream ms = new MemoryStream())
				using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
				using (StreamWriter sw = new StreamWriter(cs, Encoding.UTF8))
				{
					sw.Write(plainText);
					sw.Flush();
					cs.FlushFinalBlock();
					return ms.ToArray();
				}
			}
		}

		public static string Decrypt(byte[] cipherText, string password)
		{
			GenerateKeyAndIV(password, out byte[] key, out byte[] iv);

			using (Aes aes = Aes.Create())
			{
				aes.Key = key;
				aes.IV = iv;

				using (MemoryStream ms = new MemoryStream(cipherText))
				using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read))
				using (StreamReader sr = new StreamReader(cs, Encoding.UTF8))
				{
					return sr.ReadToEnd();
				}
			}
		}
	}
}
