using System;

namespace ControlDeEvaluacionesEscolares.Application.DTOs
{
    public class EstudianteDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Matricula { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Email { get; set; }
    }
}