using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using TodoList.Data;
using TodoList.DTOs;
using TodoList.Models;

using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace TodoListApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TasksController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene todas las tareas con filtros opcionales
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TodoTask>>> GetAllTasks(
            [FromQuery] bool? isCompleted = null,
            [FromQuery] string? priority = null,
            [FromQuery] string? searchTerm = null)
        {
            var query = _context.TodoTasks.AsQueryable();

            // Filtrar por estado
            if (isCompleted.HasValue)
            {
                query = query.Where(t => t.IsCompleted == isCompleted.Value);
            }

            // Filtrar por prioridad
            if (!string.IsNullOrEmpty(priority))
            {
                query = query.Where(t => t.Priority == priority);
            }

            // Buscar en título o descripción
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(t =>
                    t.Title.Contains(searchTerm) ||
                    (t.Description != null && t.Description.Contains(searchTerm)));
            }

            var tasks = await query.OrderByDescending(t => t.CreatedAt).ToListAsync();

            return Ok(new
            {
                count = tasks.Count,
                data = tasks
            });
        }

        /// <summary>
        /// Obtiene estadísticas de las tareas
        /// </summary>
        [HttpGet("stats")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetTaskStats()
        {
            var totalTasks = await _context.TodoTasks.CountAsync();
            var completedTasks = await _context.TodoTasks.CountAsync(t => t.IsCompleted);
            var pendingTasks = totalTasks - completedTasks;
            var highPriorityTasks = await _context.TodoTasks.CountAsync(t => t.Priority == "Alta" && !t.IsCompleted);

            return Ok(new
            {
                total = totalTasks,
                completed = completedTasks,
                pending = pendingTasks,
                highPriority = highPriorityTasks,
                completionRate = totalTasks > 0 ? Math.Round((double)completedTasks / totalTasks * 100, 2) : 0
            });
        }
        /// <summary>
        /// Crea una nueva tarea
        /// </summary>
        /// <param name="createTaskDto">Datos de la tarea a crear</param>
        /// <returns>La tarea creada</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TodoTask>> CreateTask([FromBody] CreateTaskDto createTaskDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var task = new TodoTask
            {
                Title = createTaskDto.Title,
                Description = createTaskDto.Description,
                Priority = createTaskDto.Priority,
                CreatedAt = DateTime.Now,
                IsCompleted = false
            };

            _context.TodoTasks.Add(task);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
        }

        /// <summary>
        /// Obtiene una tarea por ID (placeholder)
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TodoTask>> GetTask(int id)
        {
            var task = await _context.TodoTasks.FindAsync(id);

            if (task == null)
            {
                return NotFound(new
                {
                    message = $"Tarea con ID {id} no encontrada",
                    id = id
                });
            }

            return Ok(task);
        }
    }
}