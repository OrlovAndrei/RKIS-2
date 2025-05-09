using Microsoft.EntityFrameworkCore;

namespace TodoList
{
    public class AppDbContext : DbContext
    {
        public DbSet<TodoItem> Todos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=todolist.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TodoItem>()
                .HasKey(t => t.Id);

            modelBuilder.Entity<TodoItem>()
                .Property(t => t.Text)
                .IsRequired();
        }
    }
}