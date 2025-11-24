using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using TodoList.Models;

namespace TodoList.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<TodoTask> TodoTasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración adicional
            modelBuilder.Entity<TodoTask>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.Priority).IsRequired().HasMaxLength(10);
            });

            // Datos de prueba (seed data)
            modelBuilder.Entity<TodoTask>().HasData(
                new TodoTask
                {
                    Id = 1,
                    Title = "Completar tarea de Git Flow",
                    Description = "Implementar todas las features con pull requests correctamente",
                    Priority = "Alta",
                    CreatedAt = DateTime.Now,
                    IsCompleted = false
                },
                new TodoTask
                {
                    Id = 2,
                    Title = "Estudiar para examen",
                    Description = "Repasar conceptos de programación y bases de datos",
                    Priority = "Media",
                    CreatedAt = DateTime.Now,
                    IsCompleted = false
                },
                new TodoTask
                {
                    Id = 3,
                    Title = "Hacer ejercicio",
                    Description = "Rutina de 30 minutos",
                    Priority = "Baja",
                    CreatedAt = DateTime.Now.AddDays(-1),
                    IsCompleted = true,
                    CompletedAt = DateTime.Now.AddHours(-2)
                }
            );
        }
    }
}