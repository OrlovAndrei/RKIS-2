using Microsoft.EntityFrameworkCore;

namespace TodoList
{
	public class TodoService
{
    private readonly TodoContext _context;

    public TodoService(TodoContext context)
    {
        _context = context;
    }

    public async Task<List<TodoItem>> GetAllAsync()
    {
        return await _context.TodoItems.ToListAsync();
    }

    public async Task<TodoItem?> GetByIdAsync(int id)
    {
        return await _context.TodoItems.FindAsync(id);
    }

    public async Task AddAsync(string text)
    {
        var item = new TodoItem { Text = text };
        _context.TodoItems.Add(item);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateTextAsync(int id, string newText)
    {
        var item = await _context.TodoItems.FindAsync(id);
        if (item == null) return;

        item.Text = newText;
        await _context.SaveChangesAsync();
    }

    public async Task ToggleCompletedAsync(int id)
    {
        var item = await _context.TodoItems.FindAsync(id);
        if (item == null) return;

        item.IsCompleted = !item.IsCompleted;
        item.CompletedAt = item.IsCompleted ? DateTime.UtcNow : null;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var item = await _context.TodoItems.FindAsync(id);
        if (item == null) return;

        _context.TodoItems.Remove(item);
        await _context.SaveChangesAsync();
    }
}
