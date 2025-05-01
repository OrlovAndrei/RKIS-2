using Microsoft.EntityFrameworkCore;

namespace TodoList
{
	public class TodoService
	{
		public async Task<TodoItem> AddTodo(TodoItem item)
		{
			throw new NotImplementedException();
		}

		public async Task<TodoItem>? UpdateTodo(TodoItem item)
		{
			throw new NotImplementedException();
		}

		public async Task DeleteTodo(Guid id)
		{
			throw new NotImplementedException();
		}

		public async Task<List<TodoItem>> GetAllTodos()
		{
			throw new NotImplementedException();
		}

		public async Task<TodoItem>? GetByIdTodos(Guid id)
		{
			throw new NotImplementedException();
		}
	}
}