using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TodoList
{
    public class TodoService
    {
        public async Task<TodoItem> AddTodo(TodoItem item)
        {
            using var db = new AppDbContext();
            db.Todos.Add(item);
            await db.SaveChangesAsync();
            return item;
        }

        public async Task<TodoItem?> UpdateTodo(TodoItem item)
        {
            using var db = new AppDbContext();
            var existing = await db.Todos.FindAsync(item.Id);
            if (existing == null) return null;

            existing.Text = item.Text;
            existing.IsCompleted = item.IsCompleted;
            existing.EndTime = item.IsCompleted ? DateTime.Now : null;

            await db.SaveChangesAsync();
            return existing;
        }

        public async Task DeleteTodo(Guid id)
        {
            using var db = new AppDbContext();
            var item = await db.Todos.FindAsync(id);
            if (item != null)
            {
                db.Todos.Remove(item);
                await db.SaveChangesAsync();
            }
        }

        public async Task<List<TodoItem>> GetAllTodos()
        {
            using var db = new AppDbContext();
            return await db.Todos.ToListAsync();
        }

        public async Task<TodoItem?> GetByIdTodos(Guid id)
        {
            using var db = new AppDbContext();
            return await db.Todos.FindAsync(id);
        }
    }
}