using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tallerV1.Models.Entities
{
    public class OrdenTrabajoRepuesto
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("OrdenTrabajo")]
        public int OrdenTrabajoId { get; set; }

        [ForeignKey("Repuesto")]
        public int RepuestoId { get; set; }

        public int Cantidad { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioUnitario { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Subtotal { get; set; }

        [StringLength(200)]
        public string? NumeroSerie { get; set; }

        public DateTime? FechaInstalacion { get; set; }

        [StringLength(500)]
        public string? Observaciones { get; set; }

        // Navegación
        public OrdenTrabajo OrdenTrabajo { get; set; } = null!;
        public Repuesto Repuesto { get; set; } = null!;
    }
}
