using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace TodoList
{
    public class TodoService
    {
        private readonly TodoDbContext _context;

        public TodoService(TodoDbContext context)
        {
            _context = context;
        }

        public async Task<TodoItem> AddTodo(TodoItem item)
        {
            item.StartDate = DateTime.Now;
            _context.TodoItems.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<TodoItem?> UpdateTodo(TodoItem item)
        {
            var existingItem = await _context.TodoItems.FindAsync(item.Id);
            if (existingItem == null) return null;
            existingItem.Text = item.Text;
            existingItem.IsCompleted = item.IsCompleted;
            if (item.IsCompleted) existingItem.EndDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return existingItem;
        }

        public async Task DeleteTodo(Guid id)
        {
            var item = await _context.TodoItems.FindAsync(id);
            if (item != null)
            {
                _context.TodoItems.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<TodoItem>> GetAllTodos()
        {
            return await _context.TodoItems.ToListAsync();
        }

        public async Task<TodoItem?> GetByIdTodos(Guid id)
        {
            return await _context.TodoItems.FindAsync(id);
        }
    }

    public class TodoItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Text { get; set; } = "";
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsCompleted { get; set; }
    }

    public class TodoDbContext : DbContext
    {
        public DbSet<TodoItem> TodoItems { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("TestDb");
        }
    }
}
