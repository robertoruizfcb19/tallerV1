using System.ComponentModel.DataAnnotations;
using tallerV1.Models.Enums;

namespace tallerV1.Models.DTOs
{
    public class EquipoDTO
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public TipoEquipoEnum TipoEquipo { get; set; }
        public string TipoEquipoNombre { get; set; } = string.Empty;
        public string? Marca { get; set; }
        public string? Modelo { get; set; }
        public string? NumeroSerie { get; set; }
        public string? Placa { get; set; }
        public int? AnioFabricacion { get; set; }
        public int EmpresaId { get; set; }
        public string EmpresaNombre { get; set; } = string.Empty;
        public TipoControlEnum? TipoControl { get; set; }
        public string? TipoControlNombre { get; set; }
        public decimal? KilometrajeActual { get; set; }
        public decimal? HorasActuales { get; set; }
        public decimal? GalonesPorKm { get; set; }
        public decimal? GalonesPorHora { get; set; }
        public string? Observaciones { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaUltimoMantenimiento { get; set; }
    }

    public class CreateEquipoDTO
    {
        [Required(ErrorMessage = "El código es requerido")]
        [StringLength(50)]
        public string Codigo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(200)]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El tipo de equipo es requerido")]
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

        [Required(ErrorMessage = "La empresa es requerida")]
        public int EmpresaId { get; set; }

        public TipoControlEnum? TipoControl { get; set; }

        public decimal? KilometrajeActual { get; set; }

        public decimal? HorasActuales { get; set; }

        public decimal? GalonesPorKm { get; set; }

        public decimal? GalonesPorHora { get; set; }

        [StringLength(500)]
        public string? Observaciones { get; set; }
    }

    public class UpdateEquipoDTO
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(200)]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El tipo de equipo es requerido")]
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

        public TipoControlEnum? TipoControl { get; set; }

        public decimal? KilometrajeActual { get; set; }

        public decimal? HorasActuales { get; set; }

        public decimal? GalonesPorKm { get; set; }

        public decimal? GalonesPorHora { get; set; }

        [StringLength(500)]
        public string? Observaciones { get; set; }

        public bool Activo { get; set; }
    }
}
