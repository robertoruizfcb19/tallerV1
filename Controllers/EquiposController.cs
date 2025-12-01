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
    public class EquiposController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EquiposController> _logger;

        public EquiposController(ApplicationDbContext context, ILogger<EquiposController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<EquipoDTO>>>> GetAll([FromQuery] int? empresaId, [FromQuery] TipoEquipoEnum? tipoEquipo)
        {
            try
            {
                var query = _context.Equipos.Include(e => e.Empresa).AsQueryable();

                if (empresaId.HasValue)
                    query = query.Where(e => e.EmpresaId == empresaId.Value);

                if (tipoEquipo.HasValue)
                    query = query.Where(e => e.TipoEquipo == tipoEquipo.Value);

                var equipos = await query
                    .OrderBy(e => e.Codigo)
                    .Select(e => new EquipoDTO
                    {
                        Id = e.Id,
                        Codigo = e.Codigo,
                        Nombre = e.Nombre,
                        TipoEquipo = e.TipoEquipo,
                        TipoEquipoNombre = e.TipoEquipo.ToString(),
                        Marca = e.Marca,
                        Modelo = e.Modelo,
                        NumeroSerie = e.NumeroSerie,
                        Placa = e.Placa,
                        AnioFabricacion = e.AnioFabricacion,
                        EmpresaId = e.EmpresaId,
                        EmpresaNombre = e.Empresa.Nombre,
                        TipoControl = e.TipoControl,
                        TipoControlNombre = e.TipoControl.HasValue ? e.TipoControl.Value.ToString() : null,
                        KilometrajeActual = e.KilometrajeActual,
                        HorasActuales = e.HorasActuales,
                        GalonesPorKm = e.GalonesPorKm,
                        GalonesPorHora = e.GalonesPorHora,
                        Observaciones = e.Observaciones,
                        Activo = e.Activo,
                        FechaCreacion = e.FechaCreacion,
                        FechaUltimoMantenimiento = e.FechaUltimoMantenimiento
                    })
                    .ToListAsync();

                return Ok(ApiResponse<List<EquipoDTO>>.SuccessResponse(equipos));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener equipos");
                return StatusCode(500, ApiResponse<List<EquipoDTO>>.ErrorResponse("Error interno del servidor"));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<EquipoDTO>>> GetById(int id)
        {
            try
            {
                var equipo = await _context.Equipos
                    .Include(e => e.Empresa)
                    .Where(e => e.Id == id)
                    .Select(e => new EquipoDTO
                    {
                        Id = e.Id,
                        Codigo = e.Codigo,
                        Nombre = e.Nombre,
                        TipoEquipo = e.TipoEquipo,
                        TipoEquipoNombre = e.TipoEquipo.ToString(),
                        Marca = e.Marca,
                        Modelo = e.Modelo,
                        NumeroSerie = e.NumeroSerie,
                        Placa = e.Placa,
                        AnioFabricacion = e.AnioFabricacion,
                        EmpresaId = e.EmpresaId,
                        EmpresaNombre = e.Empresa.Nombre,
                        TipoControl = e.TipoControl,
                        TipoControlNombre = e.TipoControl.HasValue ? e.TipoControl.Value.ToString() : null,
                        KilometrajeActual = e.KilometrajeActual,
                        HorasActuales = e.HorasActuales,
                        GalonesPorKm = e.GalonesPorKm,
                        GalonesPorHora = e.GalonesPorHora,
                        Observaciones = e.Observaciones,
                        Activo = e.Activo,
                        FechaCreacion = e.FechaCreacion,
                        FechaUltimoMantenimiento = e.FechaUltimoMantenimiento
                    })
                    .FirstOrDefaultAsync();

                if (equipo == null)
                    return NotFound(ApiResponse<EquipoDTO>.ErrorResponse("Equipo no encontrado"));

                return Ok(ApiResponse<EquipoDTO>.SuccessResponse(equipo));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener equipo {EquipoId}", id);
                return StatusCode(500, ApiResponse<EquipoDTO>.ErrorResponse("Error interno del servidor"));
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrador,Coordinador")]
        public async Task<ActionResult<ApiResponse<EquipoDTO>>> Create([FromBody] CreateEquipoDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ApiResponse<EquipoDTO>.ErrorResponse("Datos inválidos"));

                // Verificar si el código ya existe
                if (await _context.Equipos.AnyAsync(e => e.Codigo == dto.Codigo))
                    return BadRequest(ApiResponse<EquipoDTO>.ErrorResponse("El código de equipo ya existe"));

                var equipo = new Equipo
                {
                    Codigo = dto.Codigo,
                    Nombre = dto.Nombre,
                    TipoEquipo = dto.TipoEquipo,
                    Marca = dto.Marca,
                    Modelo = dto.Modelo,
                    NumeroSerie = dto.NumeroSerie,
                    Placa = dto.Placa,
                    AnioFabricacion = dto.AnioFabricacion,
                    EmpresaId = dto.EmpresaId,
                    TipoControl = dto.TipoControl,
                    KilometrajeActual = dto.KilometrajeActual,
                    HorasActuales = dto.HorasActuales,
                    GalonesPorKm = dto.GalonesPorKm,
                    GalonesPorHora = dto.GalonesPorHora,
                    Observaciones = dto.Observaciones,
                    Activo = true,
                    FechaCreacion = DateTime.Now
                };

                _context.Equipos.Add(equipo);
                await _context.SaveChangesAsync();

                // Cargar la empresa
                await _context.Entry(equipo).Reference(e => e.Empresa).LoadAsync();

                var equipoDTO = new EquipoDTO
                {
                    Id = equipo.Id,
                    Codigo = equipo.Codigo,
                    Nombre = equipo.Nombre,
                    TipoEquipo = equipo.TipoEquipo,
                    TipoEquipoNombre = equipo.TipoEquipo.ToString(),
                    Marca = equipo.Marca,
                    Modelo = equipo.Modelo,
                    NumeroSerie = equipo.NumeroSerie,
                    Placa = equipo.Placa,
                    AnioFabricacion = equipo.AnioFabricacion,
                    EmpresaId = equipo.EmpresaId,
                    EmpresaNombre = equipo.Empresa.Nombre,
                    TipoControl = equipo.TipoControl,
                    TipoControlNombre = equipo.TipoControl?.ToString(),
                    KilometrajeActual = equipo.KilometrajeActual,
                    HorasActuales = equipo.HorasActuales,
                    GalonesPorKm = equipo.GalonesPorKm,
                    GalonesPorHora = equipo.GalonesPorHora,
                    Observaciones = equipo.Observaciones,
                    Activo = equipo.Activo,
                    FechaCreacion = equipo.FechaCreacion
                };

                _logger.LogInformation("Equipo {EquipoCodigo} creado exitosamente", equipo.Codigo);

                return CreatedAtAction(nameof(GetById), new { id = equipo.Id },
                    ApiResponse<EquipoDTO>.SuccessResponse(equipoDTO, "Equipo creado exitosamente"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear equipo");
                return StatusCode(500, ApiResponse<EquipoDTO>.ErrorResponse("Error interno del servidor"));
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador,Coordinador")]
        public async Task<ActionResult<ApiResponse<EquipoDTO>>> Update(int id, [FromBody] UpdateEquipoDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ApiResponse<EquipoDTO>.ErrorResponse("Datos inválidos"));

                var equipo = await _context.Equipos.Include(e => e.Empresa).FirstOrDefaultAsync(e => e.Id == id);
                if (equipo == null)
                    return NotFound(ApiResponse<EquipoDTO>.ErrorResponse("Equipo no encontrado"));

                equipo.Nombre = dto.Nombre;
                equipo.TipoEquipo = dto.TipoEquipo;
                equipo.Marca = dto.Marca;
                equipo.Modelo = dto.Modelo;
                equipo.NumeroSerie = dto.NumeroSerie;
                equipo.Placa = dto.Placa;
                equipo.AnioFabricacion = dto.AnioFabricacion;
                equipo.TipoControl = dto.TipoControl;
                equipo.KilometrajeActual = dto.KilometrajeActual;
                equipo.HorasActuales = dto.HorasActuales;
                equipo.GalonesPorKm = dto.GalonesPorKm;
                equipo.GalonesPorHora = dto.GalonesPorHora;
                equipo.Observaciones = dto.Observaciones;
                equipo.Activo = dto.Activo;

                await _context.SaveChangesAsync();

                var equipoDTO = new EquipoDTO
                {
                    Id = equipo.Id,
                    Codigo = equipo.Codigo,
                    Nombre = equipo.Nombre,
                    TipoEquipo = equipo.TipoEquipo,
                    TipoEquipoNombre = equipo.TipoEquipo.ToString(),
                    Marca = equipo.Marca,
                    Modelo = equipo.Modelo,
                    NumeroSerie = equipo.NumeroSerie,
                    Placa = equipo.Placa,
                    AnioFabricacion = equipo.AnioFabricacion,
                    EmpresaId = equipo.EmpresaId,
                    EmpresaNombre = equipo.Empresa.Nombre,
                    TipoControl = equipo.TipoControl,
                    TipoControlNombre = equipo.TipoControl?.ToString(),
                    KilometrajeActual = equipo.KilometrajeActual,
                    HorasActuales = equipo.HorasActuales,
                    GalonesPorKm = equipo.GalonesPorKm,
                    GalonesPorHora = equipo.GalonesPorHora,
                    Observaciones = equipo.Observaciones,
                    Activo = equipo.Activo,
                    FechaCreacion = equipo.FechaCreacion,
                    FechaUltimoMantenimiento = equipo.FechaUltimoMantenimiento
                };

                return Ok(ApiResponse<EquipoDTO>.SuccessResponse(equipoDTO, "Equipo actualizado exitosamente"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar equipo {EquipoId}", id);
                return StatusCode(500, ApiResponse<EquipoDTO>.ErrorResponse("Error interno del servidor"));
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
        {
            try
            {
                var equipo = await _context.Equipos.FindAsync(id);
                if (equipo == null)
                    return NotFound(ApiResponse<object>.ErrorResponse("Equipo no encontrado"));

                equipo.Activo = false;
                await _context.SaveChangesAsync();

                return Ok(ApiResponse<object>.SuccessResponse(null, "Equipo desactivado exitosamente"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar equipo {EquipoId}", id);
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Error interno del servidor"));
            }
        }

        [HttpPut("{id}/actualizar-kilometraje")]
        [Authorize(Roles = "Administrador,Coordinador,Mecanico")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateKilometraje(int id, [FromBody] decimal kilometraje)
        {
            try
            {
                var equipo = await _context.Equipos.FindAsync(id);
                if (equipo == null)
                    return NotFound(ApiResponse<object>.ErrorResponse("Equipo no encontrado"));

                equipo.KilometrajeActual = kilometraje;
                await _context.SaveChangesAsync();

                return Ok(ApiResponse<object>.SuccessResponse(null, "Kilometraje actualizado exitosamente"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar kilometraje del equipo {EquipoId}", id);
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Error interno del servidor"));
            }
        }

        [HttpPut("{id}/actualizar-horas")]
        [Authorize(Roles = "Administrador,Coordinador,Mecanico")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateHoras(int id, [FromBody] decimal horas)
        {
            try
            {
                var equipo = await _context.Equipos.FindAsync(id);
                if (equipo == null)
                    return NotFound(ApiResponse<object>.ErrorResponse("Equipo no encontrado"));

                equipo.HorasActuales = horas;
                await _context.SaveChangesAsync();

                return Ok(ApiResponse<object>.SuccessResponse(null, "Horas actualizadas exitosamente"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar horas del equipo {EquipoId}", id);
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Error interno del servidor"));
            }
        }
    }
}
