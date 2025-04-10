using System.Security.Cryptography;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace cripto_reader
{
	[TestFixture]
	public class AesEncryptorTests
	{
		private const string TestPassword = "securePassword123";
		private const string TestFileName = "test_encrypted.bin";

		[Test]
		public void EncryptDecrypt_ShortText_ReturnsOriginal()
		{
			string original = "Hello, World!";
			byte[] encrypted = AesCrypt.Encrypt(original, TestPassword);
			string decrypted = AesCrypt.Decrypt(encrypted, TestPassword);
			Assert.That(original == decrypted);
		}

		[Test]
		public void Decrypt_WrongKey_ThrowsCryptographicException()
		{
			string original = "Test";
			byte[] encrypted = AesCrypt.Encrypt(original, TestPassword);
			Assert.Throws<CryptographicException>(() =>
				AesCrypt.Decrypt(encrypted, "wrongPassword"));
		}

		[Test]
		public void EncryptDecrypt_File_Success()
		{
			string original = "File content";
			var encrypted = AesCrypt.Encrypt(original, TestPassword);
			File.WriteAllBytes(TestFileName, encrypted);
			byte[] data = File.ReadAllBytes(TestFileName);
			string decrypted = AesCrypt.Decrypt(data, TestPassword);
			File.Delete(TestFileName);
			Assert.That(original == decrypted);
		}

		[Test]
		public void DifferentKeys_ProduceDifferentOutput()
		{
			string original = "Same text";
			byte[] encrypted1 = AesCrypt.Encrypt(original, "key1");
			byte[] encrypted2 = AesCrypt.Encrypt(original, "key2");
			CollectionAssert.AreNotEqual(encrypted1, encrypted2);
		}

		[Test]
		public void InvalidFile_ThrowsFileNotFound()
		{
			Assert.Throws<FileNotFoundException>(() =>
				AesCrypt.Decrypt(File.ReadAllBytes("nonexistent.bin"), TestPassword));
		}

		[TestCase("★♫☃§¶")]
		[TestCase("12345")]
		[TestCase("")]
		public void EncryptDecrypt_VariousInputs_Consistent(string input)
		{
			byte[] encrypted = AesCrypt.Encrypt(input, TestPassword);
			string decrypted = AesCrypt.Decrypt(encrypted, TestPassword);
			Assert.That(input == decrypted);
		
	}
}