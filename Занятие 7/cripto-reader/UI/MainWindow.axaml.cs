using System.Security.Cryptography;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace cripto_reader.UI
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void EncryptButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				string fileName = FileNameTextBox.Text;
				string content = ContentTextBox.Text;
				string key = KeyTextBox.Text;

				if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(key))
					throw new ArgumentException("Заполните все поля");

				byte[] encrypted = AesCrypt.Encrypt(content, key);
				File.WriteAllBytes(fileName, encrypted);

				ContentTextBox.Text = "Файл успешно зашифрован!";
			}
			catch (Exception ex)
			{
				ContentTextBox.Text = $"Ошибка: {ex.Message}";
			}
		}

		private void DecryptButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				string fileName = FileNameTextBox.Text;
				string key = KeyTextBox.Text;

				if (!File.Exists(fileName))
					throw new FileNotFoundException("Файл не найден");

				byte[] data = File.ReadAllBytes(fileName);
				string decrypted = AesCrypt.Decrypt(data, key);

				ContentTextBox.Text = decrypted;
			}
			catch (CryptographicException)
			{
				ContentTextBox.Text = "Неверный ключ или поврежденный файл";
			}
			catch (Exception ex)
			{
				ContentTextBox.Text = $"Ошибка: {ex.Message}";
			}
		}
	}
}