using todo_api.Models;

namespace todo_api.Repositories
{
    /// <summary>
    /// Data access layer for ToDo items.
    /// </summary>
    public interface ITodoRepository
    {
        /// <summary>
        /// Inserts a new <see cref="TodoItem"/> into the database.
        /// </summary>
        Task AddAsync(TodoItem item);

        /// <summary>
        /// Removes the specified <see cref="TodoItem"/>.
        /// </summary>
        Task DeleteAsync(TodoItem item);

        /// <summary>
        /// Retrieves all <see cref="TodoItem"/> records.
        /// </summary>
        Task<List<TodoItem>> GetAllItemsAsync();

        /// <summary>
        /// Finds a <see cref="TodoItem"/> by its identifier.
        /// </summary>
        Task<TodoItem?> GetByIdAsync(int id);

        /// <summary>
        /// Retrieves all <see cref="TodoItem"/> with due date within the next 7 days.
        /// </summary>
        Task<List<TodoItem>> GetIncomingAsync();

        /// <summary>
        /// Persists all pending changes to the database.
        /// </summary>
        Task SaveChangesAsync();

        /// <summary>
        /// Updates an existing <see cref="TodoItem"/>.
        /// </summary>
        Task UpdateAsync(TodoItem item);
    }
}
