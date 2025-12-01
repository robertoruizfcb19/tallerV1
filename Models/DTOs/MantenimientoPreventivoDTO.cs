using System.ComponentModel.DataAnnotations;
using tallerV1.Models.Enums;

namespace tallerV1.Models.DTOs
{
    public class MantenimientoPreventivoDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public int EquipoId { get; set; }
        public string EquipoNombre { get; set; } = string.Empty;
        public TipoControlEnum TipoControl { get; set; }
        public string TipoControlNombre { get; set; } = string.Empty;
        public decimal? IntervaloKilometros { get; set; }
        public decimal? IntervaloHoras { get; set; }
        public int? IntervaloDias { get; set; }
        public decimal? UltimoKilometraje { get; set; }
        public decimal? UltimasHoras { get; set; }
        public DateTime? UltimaFecha { get; set; }
        public decimal? ProximoKilometraje { get; set; }
        public decimal? ProximasHoras { get; set; }
        public DateTime? ProximaFecha { get; set; }
        public bool NotificacionEnviada { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public List<TareaMantenimientoDTO> Tareas { get; set; } = new List<TareaMantenimientoDTO>();
    }

    public class TareaMantenimientoDTO
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public int Orden { get; set; }
    }

    public class CreateMantenimientoPreventivoDTO
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(200)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El equipo es requerido")]
        public int EquipoId { get; set; }

        [Required(ErrorMessage = "El tipo de control es requerido")]
        public TipoControlEnum TipoControl { get; set; }

        public decimal? IntervaloKilometros { get; set; }

        public decimal? IntervaloHoras { get; set; }

        public int? IntervaloDias { get; set; }

        public List<CreateTareaMantenimientoDTO> Tareas { get; set; } = new List<CreateTareaMantenimientoDTO>();
    }

    public class CreateTareaMantenimientoDTO
    {
        [Required(ErrorMessage = "La descripción es requerida")]
        [StringLength(500)]
        public string Descripcion { get; set; } = string.Empty;

        public int Orden { get; set; }
    }

    public class UpdateMantenimientoPreventivoDTO
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(200)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Descripcion { get; set; }

        public decimal? IntervaloKilometros { get; set; }

        public decimal? IntervaloHoras { get; set; }

        public int? IntervaloDias { get; set; }

        public bool Activo { get; set; }
    }
}
