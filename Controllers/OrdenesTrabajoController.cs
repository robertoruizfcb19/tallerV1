using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tallerV1.Data;
using tallerV1.Models.DTOs;
using tallerV1.Models.Entities;
using tallerV1.Models.Enums;
using System.Security.Claims;

namespace tallerV1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrdenesTrabajoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<OrdenesTrabajoController> _logger;

        public OrdenesTrabajoController(ApplicationDbContext context, ILogger<OrdenesTrabajoController> logger)
        {
            _context = context;
            _logger = logger;
        }

        private int GetCurrentUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<OrdenTrabajoDTO>>>> GetAll([FromQuery] EstadoOrdenEnum? estado, [FromQuery] int? equipoId)
        {
            try
            {
                var query = _context.OrdenesTrabajo
                    .Include(ot => ot.Equipo)
                    .Include(ot => ot.UsuarioCreador)
                    .Include(ot => ot.UsuarioAsignado)
                    .Include(ot => ot.UsuarioAprobador)
                    .AsQueryable();

                if (estado.HasValue)
                    query = query.Where(ot => ot.Estado == estado.Value);

                if (equipoId.HasValue)
                    query = query.Where(ot => ot.EquipoId == equipoId.Value);

                var ordenes = await query
                    .OrderByDescending(ot => ot.FechaCreacion)
                    .Select(ot => new OrdenTrabajoDTO
                    {
                        Id = ot.Id,
                        NumeroOrden = ot.NumeroOrden,
                        EquipoId = ot.EquipoId,
                        EquipoNombre = ot.Equipo.Nombre,
                        EquipoCodigo = ot.Equipo.Codigo,
                        UsuarioCreadorId = ot.UsuarioCreadorId,
                        UsuarioCreadorNombre = ot.UsuarioCreador.NombreCompleto,
                        UsuarioAsignadoId = ot.UsuarioAsignadoId,
                        UsuarioAsignadoNombre = ot.UsuarioAsignado != null ? ot.UsuarioAsignado.NombreCompleto : null,
                        UsuarioAprobadorId = ot.UsuarioAprobadorId,
                        UsuarioAprobadorNombre = ot.UsuarioAprobador != null ? ot.UsuarioAprobador.NombreCompleto : null,
                        TipoMantenimiento = ot.TipoMantenimiento,
                        TipoMantenimientoNombre = ot.TipoMantenimiento.ToString(),
                        Prioridad = ot.Prioridad,
                        PrioridadNombre = ot.Prioridad.ToString(),
                        Estado = ot.Estado,
                        EstadoNombre = ot.Estado.ToString(),
                        DescripcionProblema = ot.DescripcionProblema,
                        DiagnosticoTecnico = ot.DiagnosticoTecnico,
                        KilometrajeEquipo = ot.KilometrajeEquipo,
                        HorometroEquipo = ot.HorometroEquipo,
                        CostoRepuestos = ot.CostoRepuestos,
                        CostoManoObra = ot.CostoManoObra,
                        CostoServiciosExternos = ot.CostoServiciosExternos,
                        CostoTotal = ot.CostoTotal,
                        HorasHombre = ot.HorasHombre,
                        ObservacionesFinales = ot.ObservacionesFinales,
                        FechaCreacion = ot.FechaCreacion,
                        FechaAsignacion = ot.FechaAsignacion,
                        FechaAprobacion = ot.FechaAprobacion,
                        FechaInicio = ot.FechaInicio,
                        FechaFinalizacion = ot.FechaFinalizacion,
                        FechaEntrega = ot.FechaEntrega
                    })
                    .ToListAsync();

                return Ok(ApiResponse<List<OrdenTrabajoDTO>>.SuccessResponse(ordenes));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener órdenes de trabajo");
                return StatusCode(500, ApiResponse<List<OrdenTrabajoDTO>>.ErrorResponse("Error interno del servidor"));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<OrdenTrabajoDTO>>> GetById(int id)
        {
            try
            {
                var orden = await _context.OrdenesTrabajo
                    .Include(ot => ot.Equipo)
                    .Include(ot => ot.UsuarioCreador)
                    .Include(ot => ot.UsuarioAsignado)
                    .Include(ot => ot.UsuarioAprobador)
                    .Where(ot => ot.Id == id)
                    .Select(ot => new OrdenTrabajoDTO
                    {
                        Id = ot.Id,
                        NumeroOrden = ot.NumeroOrden,
                        EquipoId = ot.EquipoId,
                        EquipoNombre = ot.Equipo.Nombre,
                        EquipoCodigo = ot.Equipo.Codigo,
                        UsuarioCreadorId = ot.UsuarioCreadorId,
                        UsuarioCreadorNombre = ot.UsuarioCreador.NombreCompleto,
                        UsuarioAsignadoId = ot.UsuarioAsignadoId,
                        UsuarioAsignadoNombre = ot.UsuarioAsignado != null ? ot.UsuarioAsignado.NombreCompleto : null,
                        UsuarioAprobadorId = ot.UsuarioAprobadorId,
                        UsuarioAprobadorNombre = ot.UsuarioAprobador != null ? ot.UsuarioAprobador.NombreCompleto : null,
                        TipoMantenimiento = ot.TipoMantenimiento,
                        TipoMantenimientoNombre = ot.TipoMantenimiento.ToString(),
                        Prioridad = ot.Prioridad,
                        PrioridadNombre = ot.Prioridad.ToString(),
                        Estado = ot.Estado,
                        EstadoNombre = ot.Estado.ToString(),
                        DescripcionProblema = ot.DescripcionProblema,
                        DiagnosticoTecnico = ot.DiagnosticoTecnico,
                        KilometrajeEquipo = ot.KilometrajeEquipo,
                        HorometroEquipo = ot.HorometroEquipo,
                        CostoRepuestos = ot.CostoRepuestos,
                        CostoManoObra = ot.CostoManoObra,
                        CostoServiciosExternos = ot.CostoServiciosExternos,
                        CostoTotal = ot.CostoTotal,
                        HorasHombre = ot.HorasHombre,
                        ObservacionesFinales = ot.ObservacionesFinales,
                        FechaCreacion = ot.FechaCreacion,
                        FechaAsignacion = ot.FechaAsignacion,
                        FechaAprobacion = ot.FechaAprobacion,
                        FechaInicio = ot.FechaInicio,
                        FechaFinalizacion = ot.FechaFinalizacion,
                        FechaEntrega = ot.FechaEntrega
                    })
                    .FirstOrDefaultAsync();

                if (orden == null)
                    return NotFound(ApiResponse<OrdenTrabajoDTO>.ErrorResponse("Orden de trabajo no encontrada"));

                return Ok(ApiResponse<OrdenTrabajoDTO>.SuccessResponse(orden));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener orden de trabajo {OrdenId}", id);
                return StatusCode(500, ApiResponse<OrdenTrabajoDTO>.ErrorResponse("Error interno del servidor"));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<OrdenTrabajoDTO>>> Create([FromBody] CreateOrdenTrabajoDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ApiResponse<OrdenTrabajoDTO>.ErrorResponse("Datos inválidos"));

                var userId = GetCurrentUserId();

                // Generar número de orden
                var ultimaOrden = await _context.OrdenesTrabajo
                    .OrderByDescending(ot => ot.Id)
                    .FirstOrDefaultAsync();

                var numeroOrden = $"OT-{DateTime.Now:yyyyMMdd}-{(ultimaOrden?.Id ?? 0) + 1:D4}";

                var orden = new OrdenTrabajo
                {
                    NumeroOrden = numeroOrden,
                    EquipoId = dto.EquipoId,
                    UsuarioCreadorId = userId,
                    TipoMantenimiento = dto.TipoMantenimiento,
                    Prioridad = dto.Prioridad,
                    Estado = EstadoOrdenEnum.Recepcionada,
                    DescripcionProblema = dto.DescripcionProblema,
                    KilometrajeEquipo = dto.KilometrajeEquipo,
                    HorometroEquipo = dto.HorometroEquipo,
                    FechaCreacion = DateTime.Now
                };

                _context.OrdenesTrabajo.Add(orden);
                await _context.SaveChangesAsync();

                // Cargar relaciones
                await _context.Entry(orden)
                    .Reference(ot => ot.Equipo)
                    .LoadAsync();
                await _context.Entry(orden)
                    .Reference(ot => ot.UsuarioCreador)
                    .LoadAsync();

                var ordenDTO = new OrdenTrabajoDTO
                {
                    Id = orden.Id,
                    NumeroOrden = orden.NumeroOrden,
                    EquipoId = orden.EquipoId,
                    EquipoNombre = orden.Equipo.Nombre,
                    EquipoCodigo = orden.Equipo.Codigo,
                    UsuarioCreadorId = orden.UsuarioCreadorId,
                    UsuarioCreadorNombre = orden.UsuarioCreador.NombreCompleto,
                    TipoMantenimiento = orden.TipoMantenimiento,
                    TipoMantenimientoNombre = orden.TipoMantenimiento.ToString(),
                    Prioridad = orden.Prioridad,
                    PrioridadNombre = orden.Prioridad.ToString(),
                    Estado = orden.Estado,
                    EstadoNombre = orden.Estado.ToString(),
                    DescripcionProblema = orden.DescripcionProblema,
                    KilometrajeEquipo = orden.KilometrajeEquipo,
                    HorometroEquipo = orden.HorometroEquipo,
                    FechaCreacion = orden.FechaCreacion
                };

                _logger.LogInformation("Orden de trabajo {NumeroOrden} creada exitosamente", orden.NumeroOrden);

                return CreatedAtAction(nameof(GetById), new { id = orden.Id },
                    ApiResponse<OrdenTrabajoDTO>.SuccessResponse(ordenDTO, "Orden de trabajo creada exitosamente"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear orden de trabajo");
                return StatusCode(500, ApiResponse<OrdenTrabajoDTO>.ErrorResponse("Error interno del servidor"));
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador,Coordinador,Mecanico")]
        public async Task<ActionResult<ApiResponse<OrdenTrabajoDTO>>> Update(int id, [FromBody] UpdateOrdenTrabajoDTO dto)
        {
            try
            {
                var orden = await _context.OrdenesTrabajo
                    .Include(ot => ot.Equipo)
                    .Include(ot => ot.UsuarioCreador)
                    .Include(ot => ot.UsuarioAsignado)
                    .Include(ot => ot.UsuarioAprobador)
                    .FirstOrDefaultAsync(ot => ot.Id == id);

                if (orden == null)
                    return NotFound(ApiResponse<OrdenTrabajoDTO>.ErrorResponse("Orden de trabajo no encontrada"));

                orden.UsuarioAsignadoId = dto.UsuarioAsignadoId;
                orden.Prioridad = dto.Prioridad;
                orden.Estado = dto.Estado;
                orden.DiagnosticoTecnico = dto.DiagnosticoTecnico;
                orden.CostoRepuestos = dto.CostoRepuestos;
                orden.CostoManoObra = dto.CostoManoObra;
                orden.CostoServiciosExternos = dto.CostoServiciosExternos;
                orden.HorasHombre = dto.HorasHombre;
                orden.ObservacionesFinales = dto.ObservacionesFinales;

                // Calcular costo total
                orden.CostoTotal = (dto.CostoRepuestos ?? 0) + (dto.CostoManoObra ?? 0) + (dto.CostoServiciosExternos ?? 0);

                // Actualizar fechas según estado
                if (dto.Estado == EstadoOrdenEnum.EnEjecucion && !orden.FechaInicio.HasValue)
                    orden.FechaInicio = DateTime.Now;

                if (dto.Estado == EstadoOrdenEnum.Completada && !orden.FechaFinalizacion.HasValue)
                    orden.FechaFinalizacion = DateTime.Now;

                await _context.SaveChangesAsync();

                var ordenDTO = MapToDTO(orden);

                return Ok(ApiResponse<OrdenTrabajoDTO>.SuccessResponse(ordenDTO, "Orden de trabajo actualizada exitosamente"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar orden de trabajo {OrdenId}", id);
                return StatusCode(500, ApiResponse<OrdenTrabajoDTO>.ErrorResponse("Error interno del servidor"));
            }
        }

        [HttpPost("{id}/asignar")]
        [Authorize(Roles = "Administrador,Coordinador")]
        public async Task<ActionResult<ApiResponse<object>>> Asignar(int id, [FromBody] AsignarOrdenDTO dto)
        {
            try
            {
                var orden = await _context.OrdenesTrabajo.FindAsync(id);
                if (orden == null)
                    return NotFound(ApiResponse<object>.ErrorResponse("Orden de trabajo no encontrada"));

                orden.UsuarioAsignadoId = dto.UsuarioAsignadoId;
                orden.FechaAsignacion = DateTime.Now;
                orden.Estado = EstadoOrdenEnum.EnDiagnostico;

                await _context.SaveChangesAsync();

                return Ok(ApiResponse<object>.SuccessResponse(null, "Orden asignada exitosamente"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al asignar orden de trabajo {OrdenId}", id);
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Error interno del servidor"));
            }
        }

        [HttpPost("{id}/aprobar")]
        [Authorize(Roles = "Administrador,Coordinador")]
        public async Task<ActionResult<ApiResponse<object>>> Aprobar(int id, [FromBody] AprobarOrdenDTO dto)
        {
            try
            {
                var orden = await _context.OrdenesTrabajo.FindAsync(id);
                if (orden == null)
                    return NotFound(ApiResponse<object>.ErrorResponse("Orden de trabajo no encontrada"));

                var userId = GetCurrentUserId();

                orden.UsuarioAprobadorId = userId;
                orden.FechaAprobacion = DateTime.Now;
                orden.Estado = dto.Aprobada ? EstadoOrdenEnum.Aprobada : EstadoOrdenEnum.Cancelada;

                if (!string.IsNullOrEmpty(dto.Observaciones))
                    orden.ObservacionesFinales = dto.Observaciones;

                await _context.SaveChangesAsync();

                return Ok(ApiResponse<object>.SuccessResponse(null, $"Orden {(dto.Aprobada ? "aprobada" : "rechazada")} exitosamente"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al aprobar/rechazar orden de trabajo {OrdenId}", id);
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Error interno del servidor"));
            }
        }

        private OrdenTrabajoDTO MapToDTO(OrdenTrabajo orden)
        {
            return new OrdenTrabajoDTO
            {
                Id = orden.Id,
                NumeroOrden = orden.NumeroOrden,
                EquipoId = orden.EquipoId,
                EquipoNombre = orden.Equipo.Nombre,
                EquipoCodigo = orden.Equipo.Codigo,
                UsuarioCreadorId = orden.UsuarioCreadorId,
                UsuarioCreadorNombre = orden.UsuarioCreador.NombreCompleto,
                UsuarioAsignadoId = orden.UsuarioAsignadoId,
                UsuarioAsignadoNombre = orden.UsuarioAsignado?.NombreCompleto,
                UsuarioAprobadorId = orden.UsuarioAprobadorId,
                UsuarioAprobadorNombre = orden.UsuarioAprobador?.NombreCompleto,
                TipoMantenimiento = orden.TipoMantenimiento,
                TipoMantenimientoNombre = orden.TipoMantenimiento.ToString(),
                Prioridad = orden.Prioridad,
                PrioridadNombre = orden.Prioridad.ToString(),
                Estado = orden.Estado,
                EstadoNombre = orden.Estado.ToString(),
                DescripcionProblema = orden.DescripcionProblema,
                DiagnosticoTecnico = orden.DiagnosticoTecnico,
                KilometrajeEquipo = orden.KilometrajeEquipo,
                HorometroEquipo = orden.HorometroEquipo,
                CostoRepuestos = orden.CostoRepuestos,
                CostoManoObra = orden.CostoManoObra,
                CostoServiciosExternos = orden.CostoServiciosExternos,
                CostoTotal = orden.CostoTotal,
                HorasHombre = orden.HorasHombre,
                ObservacionesFinales = orden.ObservacionesFinales,
                FechaCreacion = orden.FechaCreacion,
                FechaAsignacion = orden.FechaAsignacion,
                FechaAprobacion = orden.FechaAprobacion,
                FechaInicio = orden.FechaInicio,
                FechaFinalizacion = orden.FechaFinalizacion,
                FechaEntrega = orden.FechaEntrega
            };
        }
    }
}
