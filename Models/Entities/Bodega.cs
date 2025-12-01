using System.ComponentModel.DataAnnotations;

namespace tallerV1.Models.Entities
{
    public class Bodega
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Ubicacion { get; set; }

        [StringLength(100)]
        public string? Responsable { get; set; }

        public bool Activo { get; set; } = true;

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        // Navegación
        public ICollection<Repuesto> Repuestos { get; set; } = new List<Repuesto>();
    }
}
