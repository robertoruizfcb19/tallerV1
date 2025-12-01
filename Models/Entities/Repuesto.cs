using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tallerV1.Models.Entities
{
    public class Repuesto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Codigo { get; set; } = string.Empty;

        [StringLength(50)]
        public string? CodigoBarras { get; set; }

        [Required]
        [StringLength(200)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Descripcion { get; set; }

        [StringLength(100)]
        public string? Marca { get; set; }

        [StringLength(50)]
        public string? UnidadMedida { get; set; }

        [ForeignKey("Bodega")]
        public int BodegaId { get; set; }

        public int StockActual { get; set; } = 0;

        public int StockMinimo { get; set; } = 0;

        public int StockMaximo { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioUnitario { get; set; }

        public int? MesesGarantia { get; set; }

        [StringLength(100)]
        public string? Proveedor { get; set; }

        public bool Activo { get; set; } = true;

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        // Navegación
        public Bodega Bodega { get; set; } = null!;
        public ICollection<OrdenTrabajoRepuesto> OrdenesTrabajoRepuestos { get; set; } = new List<OrdenTrabajoRepuesto>();
        public ICollection<MovimientoInventario> MovimientosInventario { get; set; } = new List<MovimientoInventario>();
    }
}
