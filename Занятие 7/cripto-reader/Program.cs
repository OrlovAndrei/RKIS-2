using Avalonia;
using cripto_reader.UI;

namespace cripto_reader
{
	public class Program
	internal class Program
	{
		[STAThread]
		public static void Main(string[] args) => BuildAvaloniaApp()
@@ -13,4 +13,4 @@ public static AppBuilder BuildAvaloniaApp()
			=> AppBuilder.Configure<App>()
				.UsePlatformDetect();
	}
}
}
