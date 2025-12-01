using System.ComponentModel.DataAnnotations;
using tallerV1.Models.Enums;

namespace tallerV1.Models.DTOs
{
    public class OrdenTrabajoDTO
    {
        public int Id { get; set; }
        public string NumeroOrden { get; set; } = string.Empty;
        public int EquipoId { get; set; }
        public string EquipoNombre { get; set; } = string.Empty;
        public string EquipoCodigo { get; set; } = string.Empty;
        public int UsuarioCreadorId { get; set; }
        public string UsuarioCreadorNombre { get; set; } = string.Empty;
        public int? UsuarioAsignadoId { get; set; }
        public string? UsuarioAsignadoNombre { get; set; }
        public int? UsuarioAprobadorId { get; set; }
        public string? UsuarioAprobadorNombre { get; set; }
        public TipoMantenimientoEnum TipoMantenimiento { get; set; }
        public string TipoMantenimientoNombre { get; set; } = string.Empty;
        public PrioridadEnum Prioridad { get; set; }
        public string PrioridadNombre { get; set; } = string.Empty;
        public EstadoOrdenEnum Estado { get; set; }
        public string EstadoNombre { get; set; } = string.Empty;
        public string DescripcionProblema { get; set; } = string.Empty;
        public string? DiagnosticoTecnico { get; set; }
        public decimal? KilometrajeEquipo { get; set; }
        public decimal? HorometroEquipo { get; set; }
        public decimal? CostoRepuestos { get; set; }
        public decimal? CostoManoObra { get; set; }
        public decimal? CostoServiciosExternos { get; set; }
        public decimal? CostoTotal { get; set; }
        public decimal? HorasHombre { get; set; }
        public string? ObservacionesFinales { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaAsignacion { get; set; }
        public DateTime? FechaAprobacion { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFinalizacion { get; set; }
        public DateTime? FechaEntrega { get; set; }
    }

    public class CreateOrdenTrabajoDTO
    {
        [Required(ErrorMessage = "El equipo es requerido")]
        public int EquipoId { get; set; }

        [Required(ErrorMessage = "El tipo de mantenimiento es requerido")]
        public TipoMantenimientoEnum TipoMantenimiento { get; set; }

        [Required(ErrorMessage = "La prioridad es requerida")]
        public PrioridadEnum Prioridad { get; set; }

        [Required(ErrorMessage = "La descripción del problema es requerida")]
        [StringLength(1000)]
        public string DescripcionProblema { get; set; } = string.Empty;

        public decimal? KilometrajeEquipo { get; set; }

        public decimal? HorometroEquipo { get; set; }
    }

    public class UpdateOrdenTrabajoDTO
    {
        public int? UsuarioAsignadoId { get; set; }

        public PrioridadEnum Prioridad { get; set; }

        public EstadoOrdenEnum Estado { get; set; }

        [StringLength(2000)]
        public string? DiagnosticoTecnico { get; set; }

        public decimal? CostoRepuestos { get; set; }

        public decimal? CostoManoObra { get; set; }

        public decimal? CostoServiciosExternos { get; set; }

        public decimal? HorasHombre { get; set; }

        [StringLength(2000)]
        public string? ObservacionesFinales { get; set; }
    }

    public class AsignarOrdenDTO
    {
        [Required(ErrorMessage = "El usuario asignado es requerido")]
        public int UsuarioAsignadoId { get; set; }
    }

    public class AprobarOrdenDTO
    {
        [Required(ErrorMessage = "La aprobación es requerida")]
        public bool Aprobada { get; set; }

        [StringLength(500)]
        public string? Observaciones { get; set; }
    }
}
