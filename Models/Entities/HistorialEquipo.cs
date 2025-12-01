using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tallerV1.Models.Entities
{
    public class HistorialEquipo
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Equipo")]
        public int EquipoId { get; set; }

        [Required]
        [StringLength(100)]
        public string TipoEvento { get; set; } = string.Empty; // Mantenimiento, Reparación, Cambio Estado, etc.

        [Required]
        [StringLength(1000)]
        public string Descripcion { get; set; } = string.Empty;

        [ForeignKey("Usuario")]
        public int UsuarioId { get; set; }

        [ForeignKey("OrdenTrabajo")]
        public int? OrdenTrabajoId { get; set; }

        public DateTime FechaEvento { get; set; } = DateTime.Now;

        // Navegación
        public Equipo Equipo { get; set; } = null!;
        public Usuario Usuario { get; set; } = null!;
        public OrdenTrabajo? OrdenTrabajo { get; set; }
    }
}
