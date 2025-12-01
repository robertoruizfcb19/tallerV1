using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tallerV1.Data;
using tallerV1.Helpers;
using tallerV1.Models.DTOs;

namespace tallerV1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly JwtHelper _jwtHelper;
        private readonly JwtSettings _jwtSettings;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            ApplicationDbContext context,
            JwtHelper jwtHelper,
            JwtSettings jwtSettings,
            ILogger<AuthController> logger)
        {
            _context = context;
            _jwtHelper = jwtHelper;
            _jwtSettings = jwtSettings;
            _logger = logger;
        }

        /// <summary>
        /// Endpoint de login
        /// </summary>
        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<LoginResponseDTO>>> Login([FromBody] LoginRequestDTO request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    return BadRequest(ApiResponse<LoginResponseDTO>.ErrorResponse("Datos inválidos", errors));
                }

                // Buscar usuario
                var usuario = await _context.Usuarios
                    .Include(u => u.Empresa)
                    .FirstOrDefaultAsync(u => u.Username == request.Username && u.Activo);

                if (usuario == null)
                {
                    _logger.LogWarning($"Intento de login fallido para usuario: {request.Username}");
                    return Unauthorized(ApiResponse<LoginResponseDTO>.ErrorResponse("Credenciales inválidas"));
                }

                // Verificar password
                bool passwordValido = BCrypt.Net.BCrypt.Verify(request.Password, usuario.PasswordHash);

                if (!passwordValido)
                {
                    _logger.LogWarning($"Password incorrecto para usuario: {request.Username}");
                    return Unauthorized(ApiResponse<LoginResponseDTO>.ErrorResponse("Credenciales inválidas"));
                }

                // Actualizar último acceso
                usuario.UltimoAcceso = DateTime.Now;
                await _context.SaveChangesAsync();

                // Generar token JWT
                var token = _jwtHelper.GenerateToken(usuario);

                var response = new LoginResponseDTO
                {
                    UsuarioId = usuario.Id,
                    Username = usuario.Username,
                    NombreCompleto = usuario.NombreCompleto,
                    Email = usuario.Email,
                    Rol = usuario.Rol,
                    EmpresaId = usuario.EmpresaId,
                    EmpresaNombre = usuario.Empresa.Nombre,
                    Token = token,
                    FechaExpiracion = DateTime.Now.AddMinutes(_jwtSettings.ExpirationMinutes)
                };

                _logger.LogInformation($"Login exitoso para usuario: {usuario.Username}");

                return Ok(ApiResponse<LoginResponseDTO>.SuccessResponse(response, "Login exitoso"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el proceso de login");
                return StatusCode(500, ApiResponse<LoginResponseDTO>.ErrorResponse("Error interno del servidor"));
            }
        }

        /// <summary>
        /// Endpoint para verificar si el servidor está activo
        /// </summary>
        [HttpGet("health")]
        public ActionResult<ApiResponse<object>> Health()
        {
            return Ok(ApiResponse<object>.SuccessResponse(
                new { status = "OK", timestamp = DateTime.Now },
                "API funcionando correctamente"
            ));
        }
    }
}
