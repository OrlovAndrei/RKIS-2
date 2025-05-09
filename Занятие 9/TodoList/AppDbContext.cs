using Microsoft.EntityFrameworkCore;

namespace TodoList
{
	public class AppDbContext : DbContext
	{
		public DbSet<TodoItem> TodoItems { get; set; }

		public string DbPath { get; }

		public TodoContext()
		{
			var folder = Environment.SpecialFolder.LocalApplicationData;
			var path = Environment.GetFolderPath(folder);
			DbPath = System.IO.Path.Join(path, "todo.db");
		}

		public TodoContext(DbContextOptions<TodoContext> options) : base(options) {}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured) 
			{
				optionsBuilder.UseSqlite($"Data Source={DbPath}");
			}
		}
	}
}