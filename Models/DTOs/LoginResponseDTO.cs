using tallerV1.Models.Enums;

namespace tallerV1.Models.DTOs
{
    public class LoginResponseDTO
    {
        public int UsuarioId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public RolEnum Rol { get; set; }
        public int EmpresaId { get; set; }
        public string EmpresaNombre { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public DateTime FechaExpiracion { get; set; }
    }
}
