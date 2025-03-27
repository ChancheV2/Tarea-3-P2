namespace ControlDeEvaluacionesEscolares.Domain.Common
{
    public class StatusDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

        public StatusDto(bool success, string message, object data = null)
        {
            Success = success;
            Message = message;
            Data = data;
        }

        public static StatusDto Ok(string message = "Operación exitosa", object data = null)
        {
            return new StatusDto(true, message, data);
        }

        public static StatusDto Error(string message = "Ha ocurrido un error", object data = null)
        {
            return new StatusDto(false, message, data);
        }
    }
}