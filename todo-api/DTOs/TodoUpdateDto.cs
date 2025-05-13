using System.ComponentModel.DataAnnotations;

namespace todo_api.DTOs
{
    public class TodoUpdateDto
    {
        [Required, StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        public int PercentComplete { get; set; }
        public bool IsCompleted { get; set; }
    }
}
