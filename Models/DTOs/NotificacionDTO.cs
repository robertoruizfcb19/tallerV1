namespace tallerV1.Models.DTOs
{
    public class NotificacionDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
        public string? TipoNotificacion { get; set; }
        public bool Leida { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaLeida { get; set; }
        public int? OrdenTrabajoId { get; set; }
        public string? NumeroOrden { get; set; }
    }
}
