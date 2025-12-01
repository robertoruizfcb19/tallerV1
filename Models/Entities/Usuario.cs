using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using tallerV1.Models.Enums;

namespace tallerV1.Models.Entities
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string NombreCompleto { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public RolEnum Rol { get; set; }

        [ForeignKey("Empresa")]
        public int EmpresaId { get; set; }

        [StringLength(50)]
        public string? Telefono { get; set; }

        public bool Activo { get; set; } = true;

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public DateTime? UltimoAcceso { get; set; }

        // Navegación
        public Empresa Empresa { get; set; } = null!;
        public ICollection<OrdenTrabajo> OrdenesCreadas { get; set; } = new List<OrdenTrabajo>();
        public ICollection<OrdenTrabajo> OrdenesAsignadas { get; set; } = new List<OrdenTrabajo>();
        public ICollection<Notificacion> Notificaciones { get; set; } = new List<Notificacion>();
    }
}
