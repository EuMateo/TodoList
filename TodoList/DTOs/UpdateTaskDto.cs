using System.ComponentModel.DataAnnotations;

namespace TodoList.DTOs
{
    public class UpdateTaskDto
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public required string Title { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [RegularExpression("Alta|Media|Baja")]  
        public required string Priority { get; set; }

        public bool IsCompleted { get; set; }
    }
}