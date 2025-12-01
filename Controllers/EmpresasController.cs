using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tallerV1.Data;
using tallerV1.Models.DTOs;
using tallerV1.Models.Entities;

namespace tallerV1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EmpresasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EmpresasController> _logger;

        public EmpresasController(ApplicationDbContext context, ILogger<EmpresasController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Obtener todas las empresas
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<EmpresaDTO>>>> GetAll()
        {
            try
            {
                var empresas = await _context.Empresas
                    .OrderBy(e => e.Nombre)
                    .Select(e => new EmpresaDTO
                    {
                        Id = e.Id,
                        Nombre = e.Nombre,
                        RUC = e.RUC,
                        Direccion = e.Direccion,
                        Telefono = e.Telefono,
                        Email = e.Email,
                        Activo = e.Activo,
                        FechaCreacion = e.FechaCreacion
                    })
                    .ToListAsync();

                return Ok(ApiResponse<List<EmpresaDTO>>.SuccessResponse(empresas));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener empresas");
                return StatusCode(500, ApiResponse<List<EmpresaDTO>>.ErrorResponse("Error interno del servidor"));
            }
        }

        /// <summary>
        /// Obtener empresa por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<EmpresaDTO>>> GetById(int id)
        {
            try
            {
                var empresa = await _context.Empresas
                    .Where(e => e.Id == id)
                    .Select(e => new EmpresaDTO
                    {
                        Id = e.Id,
                        Nombre = e.Nombre,
                        RUC = e.RUC,
                        Direccion = e.Direccion,
                        Telefono = e.Telefono,
                        Email = e.Email,
                        Activo = e.Activo,
                        FechaCreacion = e.FechaCreacion
                    })
                    .FirstOrDefaultAsync();

                if (empresa == null)
                    return NotFound(ApiResponse<EmpresaDTO>.ErrorResponse("Empresa no encontrada"));

                return Ok(ApiResponse<EmpresaDTO>.SuccessResponse(empresa));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener empresa {EmpresaId}", id);
                return StatusCode(500, ApiResponse<EmpresaDTO>.ErrorResponse("Error interno del servidor"));
            }
        }

        /// <summary>
        /// Crear nueva empresa
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<ApiResponse<EmpresaDTO>>> Create([FromBody] CreateEmpresaDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ApiResponse<EmpresaDTO>.ErrorResponse("Datos inválidos"));

                var empresa = new Empresa
                {
                    Nombre = dto.Nombre,
                    RUC = dto.RUC,
                    Direccion = dto.Direccion,
                    Telefono = dto.Telefono,
                    Email = dto.Email,
                    Activo = true,
                    FechaCreacion = DateTime.Now
                };

                _context.Empresas.Add(empresa);
                await _context.SaveChangesAsync();

                var empresaDTO = new EmpresaDTO
                {
                    Id = empresa.Id,
                    Nombre = empresa.Nombre,
                    RUC = empresa.RUC,
                    Direccion = empresa.Direccion,
                    Telefono = empresa.Telefono,
                    Email = empresa.Email,
                    Activo = empresa.Activo,
                    FechaCreacion = empresa.FechaCreacion
                };

                return CreatedAtAction(nameof(GetById), new { id = empresa.Id },
                    ApiResponse<EmpresaDTO>.SuccessResponse(empresaDTO, "Empresa creada exitosamente"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear empresa");
                return StatusCode(500, ApiResponse<EmpresaDTO>.ErrorResponse("Error interno del servidor"));
            }
        }

        /// <summary>
        /// Actualizar empresa
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<ApiResponse<EmpresaDTO>>> Update(int id, [FromBody] UpdateEmpresaDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ApiResponse<EmpresaDTO>.ErrorResponse("Datos inválidos"));

                var empresa = await _context.Empresas.FindAsync(id);
                if (empresa == null)
                    return NotFound(ApiResponse<EmpresaDTO>.ErrorResponse("Empresa no encontrada"));

                empresa.Nombre = dto.Nombre;
                empresa.RUC = dto.RUC;
                empresa.Direccion = dto.Direccion;
                empresa.Telefono = dto.Telefono;
                empresa.Email = dto.Email;
                empresa.Activo = dto.Activo;

                await _context.SaveChangesAsync();

                var empresaDTO = new EmpresaDTO
                {
                    Id = empresa.Id,
                    Nombre = empresa.Nombre,
                    RUC = empresa.RUC,
                    Direccion = empresa.Direccion,
                    Telefono = empresa.Telefono,
                    Email = empresa.Email,
                    Activo = empresa.Activo,
                    FechaCreacion = empresa.FechaCreacion
                };

                return Ok(ApiResponse<EmpresaDTO>.SuccessResponse(empresaDTO, "Empresa actualizada exitosamente"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar empresa {EmpresaId}", id);
                return StatusCode(500, ApiResponse<EmpresaDTO>.ErrorResponse("Error interno del servidor"));
            }
        }

        /// <summary>
        /// Eliminar empresa (soft delete)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
        {
            try
            {
                var empresa = await _context.Empresas.FindAsync(id);
                if (empresa == null)
                    return NotFound(ApiResponse<object>.ErrorResponse("Empresa no encontrada"));

                empresa.Activo = false;
                await _context.SaveChangesAsync();

                return Ok(ApiResponse<object>.SuccessResponse(null, "Empresa desactivada exitosamente"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar empresa {EmpresaId}", id);
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Error interno del servidor"));
            }
        }
    }
}
