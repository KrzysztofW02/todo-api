using todo_api.DTOs;

namespace todo_api.Services
{
    /// <summary>
    /// Business logic layer for managing ToDo tasks.
    /// </summary>
    public interface ITodoService
    {
        /// <summary>
        /// Creates a new ToDo task.
        /// </summary>
        /// <param name="dto">Data for the new task.</param>
        /// <returns>The created <see cref="TodoReadDto"/>.</returns>
        Task<TodoReadDto> CreateAsync(TodoCreateDto dto);

        /// <summary>
        /// Retrieves all ToDo tasks.
        /// </summary>
        /// <returns>List of <see cref="TodoReadDto"/>.</returns>
        Task<List<TodoReadDto>> GetAllItemsAsync();

        /// <summary>
        /// Retrieves tasks due within the next 7 days.
        /// </summary>
        /// <returns>List of incoming tasks as <see cref="TodoReadDto"/>.</returns>
        Task<List<TodoReadDto>> GetIncomingAsync();

        /// <summary>
        /// Retrieves a single task by its ID.
        /// </summary>
        /// <param name="id">Task identifier.</param>
        /// <returns>The <see cref="TodoReadDto"/> if found; otherwise null.</returns>
        Task<TodoReadDto?> GetByIdAsync(int id);

        /// <summary>
        /// Updates an existing task.
        /// </summary>
        /// <param name="id">Task identifier.</param>
        /// <param name="dto">New data for the task.</param>
        /// <returns>True if update succeeded; false if task not found.</returns>
        Task<bool> UpdateAsync(int id, TodoUpdateDto dto);

        /// <summary>
        /// Deletes a task by its ID.
        /// </summary>
        /// <param name="id">Task identifier.</param>
        /// <returns>True if deletion succeeded; false if task not found.</returns>
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// Sets the percent complete for a task.
        /// </summary>
        /// <param name="id">Task identifier.</param>
        /// <param name="percent">New percent complete (0–100).</param>
        /// <returns>True if update succeeded; false if task not found.</returns>
        Task<bool> SetPercentCompleteAsync(int id, int percent);

        /// <summary>
        /// Marks a task as completed (100%).
        /// </summary>
        /// <param name="id">Task identifier.</param>
        /// <returns>True if operation succeeded; false if task not found.</returns>
        Task<bool> MarkAsCompleted(int id);
    }
}
