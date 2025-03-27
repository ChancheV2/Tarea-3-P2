using System;

namespace ControlDeEvaluacionesEscolares.Application.DTOs
{
    public class EvaluacionDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Calificacion { get; set; }
        public int EstudianteId { get; set; }
        public int CursoId { get; set; }

        public string NombreEstudiante { get; set; }
        public string NombreCurso { get; set; }
    }
}