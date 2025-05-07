using Microsoft.EntityFrameworkCore;

namespace TodoList
{
    public class TodoService
    {
        private readonly AppDbContext _context;

        public TodoService()
        {
            _context = new AppDbContext();
            _context.Database.EnsureCreated(); // создаёт БД, если её нет
        }

        public async Task<TodoItem> AddTodo(TodoItem item)
        {
            _context.TodoItems.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<TodoItem>? UpdateTodo(TodoItem item)
        {
            var existing = await _context.TodoItems.FindAsync(item.Id);
            if (existing == null) return null;

            existing.Text = item.Text;
            existing.IsCompleted = item.IsCompleted;
            existing.EndTime = item.IsCompleted ? DateTime.Now : null;

            await _context.SaveChangesAsync();
            return existing;
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

        public async Task<TodoItem>? GetByIdTodos(Guid id)
        {
            return await _context.TodoItems.FindAsync(id);
        }
    }
}
