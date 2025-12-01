using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tallerV1.Models.Entities
{
    public class OrdenTrabajoImagen
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("OrdenTrabajo")]
        public int OrdenTrabajoId { get; set; }

        [Required]
        [StringLength(500)]
        public string RutaArchivo { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Descripcion { get; set; }

        [StringLength(50)]
        public string? TipoImagen { get; set; } // Antes, Durante, Después

        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        // Navegación
        public OrdenTrabajo OrdenTrabajo { get; set; } = null!;
    }
}
