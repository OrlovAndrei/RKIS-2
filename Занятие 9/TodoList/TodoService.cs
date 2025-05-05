using Microsoft.EntityFrameworkCore;

namespace TodoList
{
    // Сервис для работы с задачами (TodoItems) в базе данных.
    public class TodoService
    {
        private readonly AppDbContext _db;
        //Инициализирует новый экземпляр <see cref="TodoService"/> и гарантирует создание БД.
        public TodoService()
        {
            _db = new AppDbContext();
            _db.Database.EnsureCreated(); // Создание БД, если её нет
        }

        // Добавляет новую задачу в базу данных.
        // </summary>
        // <param name="item">Задача для добавления.</param>
        // <returns>Добавленная задача.</returns>
        public async Task<TodoItem> AddTodo(TodoItem item)
        {
            _db.TodoItems.Add(item);
            await _db.SaveChangesAsync();
            return item;
        }

        // Удаляет задачу по её идентификатору.
        // </summary>
        // <param name="id">Идентификатор задачи.</param>
        // <returns>True, если задача была удалена; False, если задача не найдена.</returns>
        public async Task<bool> DeleteTodo(Guid id)
        {
            var item = await _db.TodoItems.FindAsync(id);
            if (item == null) return false;

            _db.TodoItems.Remove(item);
            await _db.SaveChangesAsync();
            return true;
        }

        // Обновляет текст задачи.
        // </summary>
        // <param name="id">Идентификатор задачи.</param>
        // <param name="newText">Новый текст задачи.</param>
        // <returns>Обновлённая задача или null, если задача не найдена.</returns>
        public async Task<TodoItem?> UpdateTodoText(Guid id, string newText)
        {
            var item = await _db.TodoItems.FindAsync(id);
            if (item == null) return null;

            item.Text = newText;
            await _db.SaveChangesAsync();
            return item;
        }

        // Переключает статус выполнения задачи (выполнена/не выполнена).
        // </summary>
        // <param name="item">Задача с новыми данными.</param>
        // <returns>Обновлённая задача или null, если задача не найдена.</returns>
        public async Task<TodoItem?> ToggleTodoCompletion(TodoItem item)
        {
            var existingItem = await _db.TodoItems.FindAsync(item.Id);
            if (existingItem == null) return null;

            // Обновляет только необходимые поля
            existingItem.Text = item.Text;
            existingItem.IsCompleted = item.IsCompleted;

            if (item.IsCompleted && existingItem.EndTime == null)
            {
                existingItem.EndTime = DateTime.Now; // Устанавливает дату завершения
            }
            else if (!item.IsCompleted)
            {
                existingItem.EndTime = null; // Сбрасывает дату завершения
            }

            await _db.SaveChangesAsync();
            return existingItem;
        }

        /// Получает список всех задач.
        /// </summary>
        /// <returns>Список задач.</returns>
        public async Task<List<TodoItem>> GetAllTodos()
        {
            return await _db.TodoItems.ToListAsync();
        }

        /// Получает задачу по её идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор задачи.</param>
        /// <returns>Найденная задача или null.</returns>
        public async Task<TodoItem?> GetByIdTodos(Guid id)
        {
            return await _db.TodoItems.FindAsync(id);
        }
    }
}