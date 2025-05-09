using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<TodoItem> AddTodo(TodoItem item)
        {
            _context.Todos.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<TodoItem?> UpdateTodo(TodoItem item)
        {
            var existing = await _context.Todos.FindAsync(item.Id);
            if (existing != null)
            {
                existing.Text = item.Text;
                existing.IsCompleted = item.IsCompleted;
                existing.EndTime = item.EndTime;
                await _context.SaveChangesAsync();
                return existing;
            }
            return null;
        }

        public async Task DeleteTodo(Guid id)
        {
            var item = await _context.Todos.FindAsync(id);
            if (item != null)
            {
                _context.Todos.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<TodoItem>> GetAllTodos()
        {
            return await _context.Todos.ToListAsync();
        }

        public async Task<TodoItem?> GetByIdTodos(Guid id)
        {
            return await _context.Todos.FindAsync(id);
        }
    }
}