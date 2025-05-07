using Avalonia;
using Microsoft.EntityFrameworkCore;
using TodoList.UI;

namespace TodoList
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            using (var db = new AppDbContext())
            {
                db.Database.Migrate(); 
            }

            // Запуск Avalonia-приложния.
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }

        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                         .UsePlatformDetect();
    }
}

