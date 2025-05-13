using todo_api.DTOs;
using todo_api.Models;

namespace todo_api.Mappings
{
    public static class TodoMapper
    {
        public static TodoReadDto AsReadDto(this TodoItem entity)
        {
            return new TodoReadDto
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                DueDate = entity.DueDate,
                PercentComplete = entity.PercentComplete,
                IsCompleted = entity.IsCompleted,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            };
        }

        public static TodoItem ToEntity(this TodoCreateDto dto)
        {
            return new TodoItem
            {
                Title = dto.Title,
                Description = dto.Description ?? string.Empty,
                DueDate = dto.DueDate,
            };
        }

        public static void UpdateFromDto(this TodoItem entity, TodoUpdateDto dto)
        {
            entity.Title = dto.Title;
            entity.Description = dto.Description ?? string.Empty;
            entity.DueDate = dto.DueDate;
            entity.PercentComplete = dto.PercentComplete;
            entity.IsCompleted = dto.IsCompleted;
            entity.UpdatedAt = DateTime.UtcNow;
        }
    }
}
