using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using tallerV1.Models.Enums;

namespace tallerV1.Models.Entities
{
    public class OrdenTrabajo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string NumeroOrden { get; set; } = string.Empty;

        [ForeignKey("Equipo")]
        public int EquipoId { get; set; }

        [ForeignKey("UsuarioCreador")]
        public int UsuarioCreadorId { get; set; }

        [ForeignKey("UsuarioAsignado")]
        public int? UsuarioAsignadoId { get; set; }

        [ForeignKey("UsuarioAprobador")]
        public int? UsuarioAprobadorId { get; set; }

        public TipoMantenimientoEnum TipoMantenimiento { get; set; }

        public PrioridadEnum Prioridad { get; set; }

        public EstadoOrdenEnum Estado { get; set; }

        [Required]
        [StringLength(1000)]
        public string DescripcionProblema { get; set; } = string.Empty;

        [StringLength(2000)]
        public string? DiagnosticoTecnico { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? KilometrajeEquipo { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? HorometroEquipo { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? CostoRepuestos { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? CostoManoObra { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? CostoServiciosExternos { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? CostoTotal { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? HorasHombre { get; set; }

        [StringLength(2000)]
        public string? ObservacionesFinales { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public DateTime? FechaAsignacion { get; set; }

        public DateTime? FechaAprobacion { get; set; }

        public DateTime? FechaInicio { get; set; }

        public DateTime? FechaFinalizacion { get; set; }

        public DateTime? FechaEntrega { get; set; }

        // Navegación
        public Equipo Equipo { get; set; } = null!;
        public Usuario UsuarioCreador { get; set; } = null!;
        public Usuario? UsuarioAsignado { get; set; }
        public Usuario? UsuarioAprobador { get; set; }
        public ICollection<OrdenTrabajoRepuesto> Repuestos { get; set; } = new List<OrdenTrabajoRepuesto>();
        public ICollection<OrdenTrabajoImagen> Imagenes { get; set; } = new List<OrdenTrabajoImagen>();
    }
}
