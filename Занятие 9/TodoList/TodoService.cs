using Microsoft.EntityFrameworkCore;

namespace TodoList
{
	public class TodoService
    {
        private readonly AppDbContext _context;

        public TodoService()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("TestDatabase") 
                .Options;

            _context = new AppDbContext(options);
            _context.Database.EnsureCreated();
        }

        public async Task<TodoItem> AddTodo(TodoItem todoItem)
        {
            if (todoItem == null)
                throw new ArgumentNullException(nameof(todoItem));

            if (string.IsNullOrWhiteSpace(todoItem.Text))
                throw new InvalidOperationException("Text property is required.");

            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();
            return todoItem;
        }

        public async Task<TodoItem> UpdateTodo(TodoItem item)
        {
            var existingItem = await _context.TodoItems.FindAsync(item.Id);
            if (existingItem != null)
            {
                existingItem.Text = item.Text;
                existingItem.EndTime = item.EndTime;
                existingItem.IsCompleted = item.IsCompleted;
                await _context.SaveChangesAsync();
                return existingItem;
            }
            return null;
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

        public async Task<TodoItem> GetByIdTodos(Guid id)
        {
            return await _context.TodoItems.FindAsync(id);
        }

        public async Task<List<TodoItem>> GetAllTodos()
        {
            return await _context.TodoItems.ToListAsync();
        }
    }
}
