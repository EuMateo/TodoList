using System.ComponentModel.DataAnnotations;

namespace TodoList.DTOs
{
    public class CreateTaskDto
    {
        [Required(ErrorMessage = "El título es requerido")]
        [StringLength(100, MinimumLength = 3)]
        public required string Title { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [RegularExpression("Alta|Media|Baja")]
        public string Priority { get; set; } = "Media";
    }
}