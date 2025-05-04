namespace cripto_reader
{
	static class AesCrypt
	{
		public static byte[] Encrypt(string plainText, string password)
		{
			if (string.IsNullOrEmpty(plainText))
                return Array.Empty<byte>();

            using (Aes aesAlg = Aes.Create())
            {
                // Генерация Key и IV на основе пароля
                using (var sha256 = SHA256.Create())
                {
                    aesAlg.Key = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                }
                
                using (var md5 = MD5.Create())
                {
                    aesAlg.IV = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
                }

                // Создание encryptor
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Шифрование
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt, Encoding.UTF8))
                        {
                            swEncrypt.Write(plainText);
                        }
                        return msEncrypt.ToArray();
                    }
                }
            }
		}

		public static string Decrypt(byte[] cipherText, string password)
		{
			if (cipherText == null || cipherText.Length == 0)
                return string.Empty;

            try
            {
                using (Aes aesAlg = Aes.Create())
                {
                    // Генерация Key и IV на основе пароля (такая же как при шифровании)
                    using (var sha256 = SHA256.Create())
                    {
                        aesAlg.Key = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                    }
                    
                    using (var md5 = MD5.Create())
                    {
                        aesAlg.IV = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
                    }

                    // Создание decryptor
                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                    // Дешифрование
                    using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt, Encoding.UTF8))
                            {
                                return srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch (CryptographicException)
            {
                throw;
            }
            catch (Exception ex) when (ex is ArgumentException || ex is IOException)
            {
                throw new CryptographicException("Ошибка расшифровки", ex);
            }
		}
	}
}
