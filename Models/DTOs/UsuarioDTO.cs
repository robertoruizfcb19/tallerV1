using System.ComponentModel.DataAnnotations;
using tallerV1.Models.Enums;

namespace tallerV1.Models.DTOs
{
    public class UsuarioDTO
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public RolEnum Rol { get; set; }
        public string RolNombre { get; set; } = string.Empty;
        public int EmpresaId { get; set; }
        public string EmpresaNombre { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? UltimoAcceso { get; set; }
    }

    public class CreateUsuarioDTO
    {
        [Required(ErrorMessage = "El nombre completo es requerido")]
        [StringLength(100)]
        public string NombreCompleto { get; set; } = string.Empty;

        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "El username es requerido")]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "El password es requerido")]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "El rol es requerido")]
        public RolEnum Rol { get; set; }

        [Required(ErrorMessage = "La empresa es requerida")]
        public int EmpresaId { get; set; }

        [StringLength(50)]
        public string? Telefono { get; set; }
    }

    public class UpdateUsuarioDTO
    {
        [Required(ErrorMessage = "El nombre completo es requerido")]
        [StringLength(100)]
        public string NombreCompleto { get; set; } = string.Empty;

        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "El rol es requerido")]
        public RolEnum Rol { get; set; }

        [Required(ErrorMessage = "La empresa es requerida")]
        public int EmpresaId { get; set; }

        [StringLength(50)]
        public string? Telefono { get; set; }

        public bool Activo { get; set; }
    }

    public class ChangePasswordDTO
    {
        [Required(ErrorMessage = "El password actual es requerido")]
        public string PasswordActual { get; set; } = string.Empty;

        [Required(ErrorMessage = "El password nuevo es requerido")]
        [StringLength(100, MinimumLength = 6)]
        public string PasswordNuevo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La confirmación de password es requerida")]
        [Compare("PasswordNuevo", ErrorMessage = "Los passwords no coinciden")]
        public string ConfirmarPassword { get; set; } = string.Empty;
    }
}
