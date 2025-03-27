using System.Collections.Generic;
using System.Reflection.Emit;
using ControlDeEvaluacionesEscolares.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ControlDeEvaluacionesEscolares.Domain.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Estudiante> Estudiantes { get; set; }
        public DbSet<Curso> Cursos { get; set; }
        public DbSet<Evaluacion> Evaluaciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Evaluacion>()
                .HasOne(e => e.Estudiante)
                .WithMany(s => s.Evaluaciones)
                .HasForeignKey(e => e.EstudianteId);

            modelBuilder.Entity<Evaluacion>()
                .HasOne(e => e.Curso)
                .WithMany(c => c.Evaluaciones)
                .HasForeignKey(e => e.CursoId);
        }
    }
}