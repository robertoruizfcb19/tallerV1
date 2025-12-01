using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tallerV1.Data;
using tallerV1.Models.DTOs;
using tallerV1.Models.Entities;
using tallerV1.Models.Enums;

namespace tallerV1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsuariosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UsuariosController> _logger;

        public UsuariosController(ApplicationDbContext context, ILogger<UsuariosController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "Administrador,Coordinador")]
        public async Task<ActionResult<ApiResponse<List<UsuarioDTO>>>> GetAll()
        {
            try
            {
                var usuarios = await _context.Usuarios
                    .Include(u => u.Empresa)
                    .OrderBy(u => u.NombreCompleto)
                    .Select(u => new UsuarioDTO
                    {
                        Id = u.Id,
                        NombreCompleto = u.NombreCompleto,
                        Email = u.Email,
                        Username = u.Username,
                        Rol = u.Rol,
                        RolNombre = u.Rol.ToString(),
                        EmpresaId = u.EmpresaId,
                        EmpresaNombre = u.Empresa.Nombre,
                        Telefono = u.Telefono,
                        Activo = u.Activo,
                        FechaCreacion = u.FechaCreacion,
                        UltimoAcceso = u.UltimoAcceso
                    })
                    .ToListAsync();

                return Ok(ApiResponse<List<UsuarioDTO>>.SuccessResponse(usuarios));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener usuarios");
                return StatusCode(500, ApiResponse<List<UsuarioDTO>>.ErrorResponse("Error interno del servidor"));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<UsuarioDTO>>> GetById(int id)
        {
            try
            {
                var usuario = await _context.Usuarios
                    .Include(u => u.Empresa)
                    .Where(u => u.Id == id)
                    .Select(u => new UsuarioDTO
                    {
                        Id = u.Id,
                        NombreCompleto = u.NombreCompleto,
                        Email = u.Email,
                        Username = u.Username,
                        Rol = u.Rol,
                        RolNombre = u.Rol.ToString(),
                        EmpresaId = u.EmpresaId,
                        EmpresaNombre = u.Empresa.Nombre,
                        Telefono = u.Telefono,
                        Activo = u.Activo,
                        FechaCreacion = u.FechaCreacion,
                        UltimoAcceso = u.UltimoAcceso
                    })
                    .FirstOrDefaultAsync();

                if (usuario == null)
                    return NotFound(ApiResponse<UsuarioDTO>.ErrorResponse("Usuario no encontrado"));

                return Ok(ApiResponse<UsuarioDTO>.SuccessResponse(usuario));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener usuario {UsuarioId}", id);
                return StatusCode(500, ApiResponse<UsuarioDTO>.ErrorResponse("Error interno del servidor"));
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrador,Coordinador")]
        public async Task<ActionResult<ApiResponse<UsuarioDTO>>> Create([FromBody] CreateUsuarioDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ApiResponse<UsuarioDTO>.ErrorResponse("Datos inválidos"));

                // Verificar si el email ya existe
                if (await _context.Usuarios.AnyAsync(u => u.Email == dto.Email))
                    return BadRequest(ApiResponse<UsuarioDTO>.ErrorResponse("El email ya está registrado"));

                // Verificar si el username ya existe
                if (await _context.Usuarios.AnyAsync(u => u.Username == dto.Username))
                    return BadRequest(ApiResponse<UsuarioDTO>.ErrorResponse("El username ya está registrado"));

                var usuario = new Usuario
                {
                    NombreCompleto = dto.NombreCompleto,
                    Email = dto.Email,
                    Username = dto.Username,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                    Rol = dto.Rol,
                    EmpresaId = dto.EmpresaId,
                    Telefono = dto.Telefono,
                    Activo = true,
                    FechaCreacion = DateTime.Now
                };

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                // Cargar la empresa para el DTO
                await _context.Entry(usuario).Reference(u => u.Empresa).LoadAsync();

                var usuarioDTO = new UsuarioDTO
                {
                    Id = usuario.Id,
                    NombreCompleto = usuario.NombreCompleto,
                    Email = usuario.Email,
                    Username = usuario.Username,
                    Rol = usuario.Rol,
                    RolNombre = usuario.Rol.ToString(),
                    EmpresaId = usuario.EmpresaId,
                    EmpresaNombre = usuario.Empresa.Nombre,
                    Telefono = usuario.Telefono,
                    Activo = usuario.Activo,
                    FechaCreacion = usuario.FechaCreacion
                };

                return CreatedAtAction(nameof(GetById), new { id = usuario.Id },
                    ApiResponse<UsuarioDTO>.SuccessResponse(usuarioDTO, "Usuario creado exitosamente"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear usuario");
                return StatusCode(500, ApiResponse<UsuarioDTO>.ErrorResponse("Error interno del servidor"));
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador,Coordinador")]
        public async Task<ActionResult<ApiResponse<UsuarioDTO>>> Update(int id, [FromBody] UpdateUsuarioDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ApiResponse<UsuarioDTO>.ErrorResponse("Datos inválidos"));

                var usuario = await _context.Usuarios.Include(u => u.Empresa).FirstOrDefaultAsync(u => u.Id == id);
                if (usuario == null)
                    return NotFound(ApiResponse<UsuarioDTO>.ErrorResponse("Usuario no encontrado"));

                // Verificar si el email ya existe en otro usuario
                if (await _context.Usuarios.AnyAsync(u => u.Email == dto.Email && u.Id != id))
                    return BadRequest(ApiResponse<UsuarioDTO>.ErrorResponse("El email ya está registrado"));

                usuario.NombreCompleto = dto.NombreCompleto;
                usuario.Email = dto.Email;
                usuario.Rol = dto.Rol;
                usuario.EmpresaId = dto.EmpresaId;
                usuario.Telefono = dto.Telefono;
                usuario.Activo = dto.Activo;

                await _context.SaveChangesAsync();

                // Recargar la empresa
                await _context.Entry(usuario).Reference(u => u.Empresa).LoadAsync();

                var usuarioDTO = new UsuarioDTO
                {
                    Id = usuario.Id,
                    NombreCompleto = usuario.NombreCompleto,
                    Email = usuario.Email,
                    Username = usuario.Username,
                    Rol = usuario.Rol,
                    RolNombre = usuario.Rol.ToString(),
                    EmpresaId = usuario.EmpresaId,
                    EmpresaNombre = usuario.Empresa.Nombre,
                    Telefono = usuario.Telefono,
                    Activo = usuario.Activo,
                    FechaCreacion = usuario.FechaCreacion,
                    UltimoAcceso = usuario.UltimoAcceso
                };

                return Ok(ApiResponse<UsuarioDTO>.SuccessResponse(usuarioDTO, "Usuario actualizado exitosamente"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar usuario {UsuarioId}", id);
                return StatusCode(500, ApiResponse<UsuarioDTO>.ErrorResponse("Error interno del servidor"));
            }
        }

        [HttpPost("{id}/cambiar-password")]
        public async Task<ActionResult<ApiResponse<object>>> ChangePassword(int id, [FromBody] ChangePasswordDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ApiResponse<object>.ErrorResponse("Datos inválidos"));

                var usuario = await _context.Usuarios.FindAsync(id);
                if (usuario == null)
                    return NotFound(ApiResponse<object>.ErrorResponse("Usuario no encontrado"));

                // Verificar password actual
                if (!BCrypt.Net.BCrypt.Verify(dto.PasswordActual, usuario.PasswordHash))
                    return BadRequest(ApiResponse<object>.ErrorResponse("Password actual incorrecto"));

                // Actualizar password
                usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.PasswordNuevo);
                await _context.SaveChangesAsync();

                return Ok(ApiResponse<object>.SuccessResponse(null, "Password actualizado exitosamente"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cambiar password del usuario {UsuarioId}", id);
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Error interno del servidor"));
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
        {
            try
            {
                var usuario = await _context.Usuarios.FindAsync(id);
                if (usuario == null)
                    return NotFound(ApiResponse<object>.ErrorResponse("Usuario no encontrado"));

                usuario.Activo = false;
                await _context.SaveChangesAsync();

                return Ok(ApiResponse<object>.SuccessResponse(null, "Usuario desactivado exitosamente"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar usuario {UsuarioId}", id);
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Error interno del servidor"));
            }
        }

        [HttpGet("por-rol/{rol}")]
        [Authorize(Roles = "Administrador,Coordinador")]
        public async Task<ActionResult<ApiResponse<List<UsuarioDTO>>>> GetByRol(RolEnum rol)
        {
            try
            {
                var usuarios = await _context.Usuarios
                    .Include(u => u.Empresa)
                    .Where(u => u.Rol == rol && u.Activo)
                    .OrderBy(u => u.NombreCompleto)
                    .Select(u => new UsuarioDTO
                    {
                        Id = u.Id,
                        NombreCompleto = u.NombreCompleto,
                        Email = u.Email,
                        Username = u.Username,
                        Rol = u.Rol,
                        RolNombre = u.Rol.ToString(),
                        EmpresaId = u.EmpresaId,
                        EmpresaNombre = u.Empresa.Nombre,
                        Telefono = u.Telefono,
                        Activo = u.Activo,
                        FechaCreacion = u.FechaCreacion,
                        UltimoAcceso = u.UltimoAcceso
                    })
                    .ToListAsync();

                return Ok(ApiResponse<List<UsuarioDTO>>.SuccessResponse(usuarios));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener usuarios por rol {Rol}", rol);
                return StatusCode(500, ApiResponse<List<UsuarioDTO>>.ErrorResponse("Error interno del servidor"));
            }
        }
    }
}
