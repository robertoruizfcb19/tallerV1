using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tallerV1.Models.Entities
{
    public class Notificacion
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Usuario")]
        public int UsuarioId { get; set; }

        [Required]
        [StringLength(200)]
        public string Titulo { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string Mensaje { get; set; } = string.Empty;

        [StringLength(50)]
        public string? TipoNotificacion { get; set; } // MantenimientoProximo, OrdenAprobada, etc.

        public bool Leida { get; set; } = false;

        public bool EnviadaPorEmail { get; set; } = false;

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public DateTime? FechaLeida { get; set; }

        [ForeignKey("OrdenTrabajo")]
        public int? OrdenTrabajoId { get; set; }

        // Navegación
        public Usuario Usuario { get; set; } = null!;
        public OrdenTrabajo? OrdenTrabajo { get; set; }
    }
}
