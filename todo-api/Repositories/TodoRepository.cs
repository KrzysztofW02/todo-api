using Microsoft.EntityFrameworkCore;
using todo_api.Models;

namespace todo_api.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly TodoDbContext _context;

        public TodoRepository(TodoDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task<List<TodoItem>> GetAllItemsAsync()
        {
            return await _context.TodoItems.ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<TodoItem?> GetByIdAsync(int id)
        {
            return await _context.TodoItems.FindAsync(id);
        }

        /// <inheritdoc/>
        public async Task<List<TodoItem>> GetIncomingAsync()
        {
            var today = DateTime.UtcNow.Date;
            var endOfWeek = today.AddDays(7);

            return await _context.TodoItems.Where(t => t.DueDate.Date >= today && t.DueDate.Date <= endOfWeek).ToListAsync();
        }

        /// <inheritdoc/>
        public async Task AddAsync(TodoItem item)
        {
            await _context.TodoItems.AddAsync(item);
        }

        /// <inheritdoc/>
        public Task UpdateAsync(TodoItem item)
        {
            _context.TodoItems.Update(item);
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task DeleteAsync(TodoItem item)
        {
            _context.TodoItems.Remove(item);
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
