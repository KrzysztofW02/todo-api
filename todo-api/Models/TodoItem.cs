namespace todo_api.Models
{
    public class TodoItem
    {
        public int Id { get; set; }                            
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public int PercentComplete { get; set; } = 0;
        public bool IsCompleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;    
        public DateTime? UpdatedAt { get; set; }                   
    }
}
