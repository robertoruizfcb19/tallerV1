using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tallerV1.Models.Entities
{
    public class MovimientoInventario
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Repuesto")]
        public int RepuestoId { get; set; }

        [Required]
        [StringLength(50)]
        public string TipoMovimiento { get; set; } = string.Empty; // Entrada, Salida, Ajuste

        public int Cantidad { get; set; }

        public int StockAnterior { get; set; }

        public int StockNuevo { get; set; }

        [StringLength(500)]
        public string? Observaciones { get; set; }

        [ForeignKey("Usuario")]
        public int UsuarioId { get; set; }

        public DateTime FechaMovimiento { get; set; } = DateTime.Now;

        // Navegación
        public Repuesto Repuesto { get; set; } = null!;
        public Usuario Usuario { get; set; } = null!;
    }
}
