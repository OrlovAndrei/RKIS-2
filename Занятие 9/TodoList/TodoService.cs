using Microsoft.EntityFrameworkCore;

namespace TodoList
{
	public class TodoService
	{
		private readonly AppDbContext _dbContext;

        public TodoService()
        {
            _dbContext = new AppDbContext();
            _dbContext.Database.EnsureCreated();
        }
		public async Task<TodoItem> AddTodo(TodoItem item)
		{
			_dbContext.TodoItems.Add(item);
            await _dbContext.SaveChangesAsync();
            return item;
		}

		public async Task<TodoItem>? UpdateTodo(TodoItem item)
		{
			var existingItem = await _dbContext.TodoItems.FindAsync(item.Id);
            if (existingItem == null)
                return null;

            existingItem.Text = item.Text;
            existingItem.IsCompleted = item.IsCompleted;
            existingItem.EndTime = item.EndTime;

            await _dbContext.SaveChangesAsync();
            return existingItem;
		}

		public async Task DeleteTodo(Guid id)
		{
			var item = await _dbContext.TodoItems.FindAsync(id);
            if (item != null)
            {
                _dbContext.TodoItems.Remove(item);
                await _dbContext.SaveChangesAsync();
            }
		}

		public async Task<List<TodoItem>> GetAllTodos()
		{
			return await _dbContext.TodoItems.ToListAsync();
		}

		public async Task<TodoItem>? GetByIdTodos(Guid id)
		{
			return await _dbContext.TodoItems.FindAsync(id);
		}
	}
}