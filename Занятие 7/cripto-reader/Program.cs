using Avalonia;
using cripto_reader.UI;

namespace cripto_reader
{
	internal class Program
	{
		[STAThread]
		public static void Main(string[] args) => BuildAvaloniaApp()
			.StartWithClassicDesktopLifetime(args);

		public static AppBuilder BuildAvaloniaApp()
			=> AppBuilder.Configure<App>()
				.UsePlatformDetect();
	}
}
}