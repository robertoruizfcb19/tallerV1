using System.ComponentModel.DataAnnotations;

namespace tallerV1.Models.DTOs
{
    public class RepuestoDTO
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string? CodigoBarras { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public string? Marca { get; set; }
        public string? UnidadMedida { get; set; }
        public int BodegaId { get; set; }
        public string BodegaNombre { get; set; } = string.Empty;
        public int StockActual { get; set; }
        public int StockMinimo { get; set; }
        public int StockMaximo { get; set; }
        public decimal PrecioUnitario { get; set; }
        public int? MesesGarantia { get; set; }
        public string? Proveedor { get; set; }
        public bool Activo { get; set; }
        public bool StockBajo { get; set; }
        public DateTime FechaCreacion { get; set; }
    }

    public class CreateRepuestoDTO
    {
        [Required(ErrorMessage = "El código es requerido")]
        [StringLength(50)]
        public string Codigo { get; set; } = string.Empty;

        [StringLength(50)]
        public string? CodigoBarras { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(200)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Descripcion { get; set; }

        [StringLength(100)]
        public string? Marca { get; set; }

        [StringLength(50)]
        public string? UnidadMedida { get; set; }

        [Required(ErrorMessage = "La bodega es requerida")]
        public int BodegaId { get; set; }

        public int StockActual { get; set; } = 0;

        public int StockMinimo { get; set; } = 0;

        public int StockMaximo { get; set; } = 0;

        [Required(ErrorMessage = "El precio es requerido")]
        public decimal PrecioUnitario { get; set; }

        public int? MesesGarantia { get; set; }

        [StringLength(100)]
        public string? Proveedor { get; set; }
    }

    public class UpdateRepuestoDTO
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(200)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Descripcion { get; set; }

        [StringLength(100)]
        public string? Marca { get; set; }

        [StringLength(50)]
        public string? UnidadMedida { get; set; }

        public int StockMinimo { get; set; }

        public int StockMaximo { get; set; }

        [Required(ErrorMessage = "El precio es requerido")]
        public decimal PrecioUnitario { get; set; }

        public int? MesesGarantia { get; set; }

        [StringLength(100)]
        public string? Proveedor { get; set; }

        public bool Activo { get; set; }
    }

    public class MovimientoInventarioDTO
    {
        public int Id { get; set; }
        public int RepuestoId { get; set; }
        public string RepuestoNombre { get; set; } = string.Empty;
        public string TipoMovimiento { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public int StockAnterior { get; set; }
        public int StockNuevo { get; set; }
        public string? Observaciones { get; set; }
        public int UsuarioId { get; set; }
        public string UsuarioNombre { get; set; } = string.Empty;
        public DateTime FechaMovimiento { get; set; }
    }

    public class CreateMovimientoInventarioDTO
    {
        [Required(ErrorMessage = "El repuesto es requerido")]
        public int RepuestoId { get; set; }

        [Required(ErrorMessage = "El tipo de movimiento es requerido")]
        public string TipoMovimiento { get; set; } = string.Empty; // Entrada, Salida, Ajuste

        [Required(ErrorMessage = "La cantidad es requerida")]
        public int Cantidad { get; set; }

        [StringLength(500)]
        public string? Observaciones { get; set; }
    }
}
