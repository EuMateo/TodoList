using System;
using System.ComponentModel.DataAnnotations;

namespace TodoList.Models
{
    public class TodoTask
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El título es requerido")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El título debe tener entre 3 y 100 caracteres")]
        public required string Title { get; set; }

        [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
        public string? Description { get; set; }

        public bool IsCompleted { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime? CompletedAt { get; set; }

        [Required]
        [RegularExpression("Alta|Media|Baja", ErrorMessage = "La prioridad debe ser: Alta, Media o Baja")]
        public string Priority { get; set; }

        public TodoTask()
        {
            CreatedAt = DateTime.Now;
            IsCompleted = false;
            Priority = "Media";
        }
    }
}