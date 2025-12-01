using System.ComponentModel.DataAnnotations;

namespace tallerV1.Models.DTOs
{
    public class LoginRequestDTO
    {
        [Required(ErrorMessage = "El username es requerido")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "El password es requerido")]
        public string Password { get; set; } = string.Empty;
    }
}
