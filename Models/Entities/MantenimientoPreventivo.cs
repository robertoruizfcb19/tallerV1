using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using tallerV1.Models.Enums;

namespace tallerV1.Models.Entities
{
    public class MantenimientoPreventivo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Descripcion { get; set; }

        [ForeignKey("Equipo")]
        public int EquipoId { get; set; }

        public TipoControlEnum TipoControl { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? IntervaloKilometros { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? IntervaloHoras { get; set; }

        public int? IntervaloDias { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? UltimoKilometraje { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? UltimasHoras { get; set; }

        public DateTime? UltimaFecha { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? ProximoKilometraje { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? ProximasHoras { get; set; }

        public DateTime? ProximaFecha { get; set; }

        public bool NotificacionEnviada { get; set; } = false;

        public bool Activo { get; set; } = true;

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        // Navegación
        public Equipo Equipo { get; set; } = null!;
        public ICollection<MantenimientoPreventivoTarea> Tareas { get; set; } = new List<MantenimientoPreventivoTarea>();
    }
}
