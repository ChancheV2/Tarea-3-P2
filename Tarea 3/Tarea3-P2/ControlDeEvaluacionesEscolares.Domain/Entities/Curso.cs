using System.Collections.Generic;

namespace ControlDeEvaluacionesEscolares.Domain.Entities
{
    public class Curso
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public virtual ICollection<Evaluacion> Evaluaciones { get; set; }
    }
}