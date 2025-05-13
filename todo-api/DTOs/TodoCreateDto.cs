using System.ComponentModel.DataAnnotations;

namespace todo_api.DTOs
{
    public class TodoCreateDto
    {
        [Required, StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        public DateTime DueDate { get; set; }
    }
}
