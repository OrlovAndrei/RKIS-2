using Microsoft.EntityFrameworkCore;

namespace TodoList
{
    public class TodoService
    {
        private readonly AppDbContext _db;

        public TodoService()
        {
            _db = new AppDbContext();
            _db.Database.EnsureCreated();
        }

        // Добавление новой задачи
        public async Task<TodoItem> AddTodo(TodoItem item)
        {
            _db.TodoItems.Add(item);
            await _db.SaveChangesAsync();
            return item;
        }

        // Удаление задачи по ID
        public async Task<bool> DeleteTodo(Guid id)
        {
            var item = await _db.TodoItems.FindAsync(id);
            if (item == null) return false;

            _db.TodoItems.Remove(item);
            await _db.SaveChangesAsync();
            return true;
        }

        // Изменение текста задачи
        public async Task<TodoItem?> UpdateTodoText(Guid id, string newText)
        {
            var item = await _db.TodoItems.FindAsync(id);
            if (item == null) return null;

            item.Text = newText;
            await _db.SaveChangesAsync();
            return item;
        }

        // Отметка задачи как выполненной/невыполненной
        public async Task<TodoItem?> ToggleTodoCompletion(TodoItem item)
        {
            var existingItem = await _db.TodoItems.FindAsync(item.Id);
            if (existingItem == null) return null;
            existingItem.Text = item.Text;
            existingItem.IsCompleted = item.IsCompleted;
            if (item == null) return null;

            // Если задача завершена, устанавливаем дату окончания
            if (item.IsCompleted && existingItem.EndTime == null)
            {
                existingItem.EndTime = DateTime.Now;
            }
            // Если задача снова активна, сбрасываем дату окончания
            else if (!item.IsCompleted)
            {
                existingItem.EndTime = null;
            }

            await _db.SaveChangesAsync();
            return existingItem;
        }

        // Получение всех задач
        public async Task<List<TodoItem>> GetAllTodos()
        {
            return await _db.TodoItems.ToListAsync();
        }

        // Получение задачи по ID
        public async Task<TodoItem?> GetByIdTodos(Guid id)
        {
            return await _db.TodoItems.FindAsync(id);
        }

    }
}
