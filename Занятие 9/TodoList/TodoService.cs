using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoList
{
    public class TodoService
    {
        private readonly List<TodoItem> _todos = new();

        public Task<TodoItem> AddTodo(TodoItem item)
        {
            _todos.Add(item);
            return Task.FromResult(item);
        }

        public Task<TodoItem?> UpdateTodo(TodoItem item) // Изменили возвращаемый тип на TodoItem?
        {
            var existing = _todos.FirstOrDefault(t => t.Id == item.Id);
            if (existing != null)
            {
                existing.Text = item.Text;
                existing.IsCompleted = item.IsCompleted;
                existing.EndTime = item.EndTime;
                return Task.FromResult<TodoItem?>(existing);
            }
            return Task.FromResult<TodoItem?>(null); // Явно указываем тип возвращаемого null
        }

        public Task DeleteTodo(Guid id)
        {
            _todos.RemoveAll(t => t.Id == id);
            return Task.CompletedTask;
        }

        public Task<List<TodoItem>> GetAllTodos()
        {
            return Task.FromResult(_todos.ToList());
        }

        public Task<TodoItem?> GetByIdTodos(Guid id) // Изменили возвращаемый тип на TodoItem?
        {
            return Task.FromResult(_todos.FirstOrDefault(t => t.Id == id));
        }
    }
}