using todo_api.DTOs;
using todo_api.Mappings;
using todo_api.Repositories;

namespace todo_api.Services
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _repository;

        public TodoService(ITodoRepository repository)
        {
            _repository = repository;
        }

        /// <inheritdoc/>
        public async Task<List<TodoReadDto>> GetAllItemsAsync()
        {
            var items = await _repository.GetAllItemsAsync();
            return items.Select(t => t.AsReadDto()).ToList();
        }

        /// <inheritdoc/>
        public async Task<TodoReadDto?> GetByIdAsync(int id)
        {
            var item = await _repository.GetByIdAsync(id);
            return item?.AsReadDto();
        }

        /// <inheritdoc/>
        public async Task<List<TodoReadDto>> GetIncomingAsync()
        {
            var items = await _repository.GetIncomingAsync();
            return items.Select(t => t.AsReadDto()).ToList();
        }

        /// <inheritdoc/>
        public async Task<TodoReadDto> CreateAsync(TodoCreateDto dto)
        {
            var item = dto.ToEntity();
            await _repository.AddAsync(item);
            await _repository.SaveChangesAsync();
            return item.AsReadDto();
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateAsync(int id, TodoUpdateDto dto)
        {
            var item = await _repository.GetByIdAsync(id);
            if (item is null) return false;

            item.UpdateFromDto(dto);
            await _repository.UpdateAsync(item);
            await _repository.SaveChangesAsync();
            return true;
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteAsync(int id)
        {
            var item = await _repository.GetByIdAsync(id);
            if (item is null) return false;

            await _repository.DeleteAsync(item);
            await _repository.SaveChangesAsync();
            return true;
        }

        /// <inheritdoc/>
        public async Task<bool> SetPercentCompleteAsync(int id, int percent)
        {
            var item = await _repository.GetByIdAsync(id);
            if (item is null) return false;

            item.PercentComplete = percent;
            item.UpdatedAt = DateTime.UtcNow;
            await _repository.UpdateAsync(item);
            await _repository.SaveChangesAsync();
            return true;
        }

        /// <inheritdoc/>
        public async Task<bool> MarkAsCompleted(int id)
        {
            var item = await _repository.GetByIdAsync(id);
            if (item == null) return false;

            item.IsCompleted = true;
            item.PercentComplete = 100;
            item.UpdatedAt = DateTime.UtcNow;
            await _repository.UpdateAsync(item);
            await _repository.SaveChangesAsync();
            return true;
        }
    }
}
