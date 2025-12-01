using System.ComponentModel.DataAnnotations;

namespace tallerV1.Models.Entities
{
    public class Empresa
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(50)]
        public string? RUC { get; set; }

        [StringLength(200)]
        public string? Direccion { get; set; }

        [StringLength(50)]
        public string? Telefono { get; set; }

        [StringLength(100)]
        public string? Email { get; set; }

        public bool Activo { get; set; } = true;

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        // Navegación
        public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
        public ICollection<Equipo> Equipos { get; set; } = new List<Equipo>();
    }
}
