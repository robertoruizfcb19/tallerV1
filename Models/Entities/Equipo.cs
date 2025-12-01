using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using tallerV1.Models.Enums;

namespace tallerV1.Models.Entities
{
    public class Equipo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Codigo { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Nombre { get; set; } = string.Empty;

        public TipoEquipoEnum TipoEquipo { get; set; }

        [StringLength(100)]
        public string? Marca { get; set; }

        [StringLength(100)]
        public string? Modelo { get; set; }

        [StringLength(50)]
        public string? NumeroSerie { get; set; }

        [StringLength(50)]
        public string? Placa { get; set; }

        public int? AnioFabricacion { get; set; }

        [ForeignKey("Empresa")]
        public int EmpresaId { get; set; }

        // Control de mantenimiento
        public TipoControlEnum? TipoControl { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? KilometrajeActual { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? HorasActuales { get; set; }

        // Control de combustible
        [Column(TypeName = "decimal(18,4)")]
        public decimal? GalonesPorKm { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal? GalonesPorHora { get; set; }

        [StringLength(500)]
        public string? Observaciones { get; set; }

        public bool Activo { get; set; } = true;

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public DateTime? FechaUltimoMantenimiento { get; set; }

        // Navegación
        public Empresa Empresa { get; set; } = null!;
        public ICollection<OrdenTrabajo> OrdenesTrabajo { get; set; } = new List<OrdenTrabajo>();
        public ICollection<MantenimientoPreventivo> MantenimientosPreventivos { get; set; } = new List<MantenimientoPreventivo>();
        public ICollection<HistorialEquipo> HistorialEquipos { get; set; } = new List<HistorialEquipo>();
    }
}
