using System.ComponentModel.DataAnnotations;

namespace tallerV1.Models.DTOs
{
    public class BodegaDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Ubicacion { get; set; }
        public string? Responsable { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int TotalRepuestos { get; set; }
    }

    public class CreateBodegaDTO
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Ubicacion { get; set; }

        [StringLength(100)]
        public string? Responsable { get; set; }
    }

    public class UpdateBodegaDTO
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Ubicacion { get; set; }

        [StringLength(100)]
        public string? Responsable { get; set; }

        public bool Activo { get; set; }
    }
}
