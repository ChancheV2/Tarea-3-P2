using System;

namespace ControlDeEvaluacionesEscolares.Domain.Entities
{
    public class Evaluacion
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Calificacion { get; set; }
        public int EstudianteId { get; set; }
        public int CursoId { get; set; }

        public virtual Estudiante Estudiante { get; set; }
        public virtual Curso Curso { get; set; }
    }
}