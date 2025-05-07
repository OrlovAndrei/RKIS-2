using Microsoft.EntityFrameworkCore;

namespace TodoList
{
    public class TodoService
    {
        private readonly AppDbContext _context;

        public TodoService()
        {
            _context = new AppDbContext();
            _context.Database.EnsureCreated();
        }

        public async Task<TodoItem> AddTodo(TodoItem newItem)
        {
            _context.TodoItems.Add(newItem);
            await _context.SaveChangesAsync();
            return newItem;
        }

        public async Task<bool> DeleteTodo(Guid todoId)
        {
            var todoToDelete = await _context.TodoItems.FindAsync(todoId);
            if (todoToDelete == null) return false;

            _context.TodoItems.Remove(todoToDelete);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<TodoItem?> UpdateTodo(Guid todoId, string updatedText)
        {
            var existingTodo = await _context.TodoItems.FindAsync(todoId);
            if (existingTodo == null) return null;

            existingTodo.Text = updatedText;
            await _context.SaveChangesAsync();
            return existingTodo;
        }

        public async Task<TodoItem?> ToggleTodoCompletion(TodoItem modifiedItem)
        {
            var currentItem = await _context.TodoItems.FindAsync(modifiedItem.Id);
            if (currentItem == null) return null;

            currentItem.Text = modifiedItem.Text;
            currentItem.IsCompleted = modifiedItem.IsCompleted;

            if (modifiedItem.IsCompleted && currentItem.EndTime == null)
            {
                currentItem.EndTime = DateTime.Now;
            }
            else if (!modifiedItem.IsCompleted)
            {
                currentItem.EndTime = null;
            }

            await _context.SaveChangesAsync();
            return currentItem;
        }

        public async Task<List<TodoItem>> GetAllTodos()
        {
            return await _context.TodoItems.ToListAsync();
        }

        public async Task<TodoItem?> GetByIdTodos(Guid todoId)
        {
            return await _context.TodoItems.FindAsync(todoId);
        }
    }
}
