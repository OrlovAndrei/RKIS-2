using Microsoft.EntityFrameworkCore;
using System;

namespace TodoList
{
	public class TodoService
	{
		private readonly AppDbContext _context;

		public TodoService(AppDbContext context)
		{
			_context = context;
			_context.Database.EnsureCreated();
		}

		public async Task<TodoItem> AddTodo(TodoItem item)
		{
			item.IsCompleted = false;
			_context.TodoItems.Add(item);
			await _context.SaveChangesAsync();
			return item;
		}

		public async Task<TodoItem>? UpdateTodo(TodoItem item)
		{
			var existingItem = await _context.TodoItems.FindAsync(item.Id);
			if (existingItem == null)
			{
				return null;
			}

			existingItem.Text = item.Text;

			if (existingItem.IsCompleted != item.IsCompleted)
			{
				existingItem.IsCompleted = item.IsCompleted;
				if (existingItem.IsCompleted)
				{
					existingItem.EndTime = DateTime.Now;
				}
				else
				{
					existingItem.EndTime = null;
				}
			}
			
			_context.Entry(existingItem).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				throw; 
			}
			
			return existingItem;
		}

		public async Task DeleteTodo(Guid id)
		{
			var itemToRemove = await _context.TodoItems.FindAsync(id);
			if (itemToRemove != null)
			{
				_context.TodoItems.Remove(itemToRemove);
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