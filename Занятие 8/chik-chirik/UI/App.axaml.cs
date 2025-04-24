using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

namespace chik_chirik.UI
{
	public partial class App : Application
	{
		public override void Initialize()
		{
			AvaloniaXamlLoader.Load(this);
		}

		public override void OnFrameworkInitializationCompleted()
		{
			// Инициализация DI-контейнера
			var httpClient = new HttpClient();
			var apiService = new ApiService(httpClient);

			if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
			{
				desktop.MainWindow = new MainWindow
				{
					DataContext = new MainWindowViewModel(apiService)
				};
			}

			base.OnFrameworkInitializationCompleted();
		}
	}
}