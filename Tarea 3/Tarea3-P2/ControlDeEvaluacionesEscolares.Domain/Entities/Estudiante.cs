using System;
using System.Collections.Generic;

namespace ControlDeEvaluacionesEscolares.Domain.Entities
{
    public class Estudiante
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Matricula { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Email { get; set; }
        public virtual ICollection<Evaluacion> Evaluaciones { get; set; }
    }
}