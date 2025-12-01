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
    public class MantenimientosPreventivosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MantenimientosPreventivosController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<MantenimientoPreventivoDTO>>>> GetAll([FromQuery] int? equipoId)
        {
            var query = _context.MantenimientosPreventivos
                .Include(mp => mp.Equipo)
                .Include(mp => mp.Tareas)
                .AsQueryable();

            if (equipoId.HasValue)
                query = query.Where(mp => mp.EquipoId == equipoId.Value);

            var mantenimientos = await query
                .OrderBy(mp => mp.Nombre)
                .Select(mp => new MantenimientoPreventivoDTO
                {
                    Id = mp.Id,
                    Nombre = mp.Nombre,
                    Descripcion = mp.Descripcion,
                    EquipoId = mp.EquipoId,
                    EquipoNombre = mp.Equipo.Nombre,
                    TipoControl = mp.TipoControl,
                    TipoControlNombre = mp.TipoControl.ToString(),
                    IntervaloKilometros = mp.IntervaloKilometros,
                    IntervaloHoras = mp.IntervaloHoras,
                    IntervaloDias = mp.IntervaloDias,
                    UltimoKilometraje = mp.UltimoKilometraje,
                    UltimasHoras = mp.UltimasHoras,
                    UltimaFecha = mp.UltimaFecha,
                    ProximoKilometraje = mp.ProximoKilometraje,
                    ProximasHoras = mp.ProximasHoras,
                    ProximaFecha = mp.ProximaFecha,
                    NotificacionEnviada = mp.NotificacionEnviada,
                    Activo = mp.Activo,
                    FechaCreacion = mp.FechaCreacion,
                    Tareas = mp.Tareas.Select(t => new TareaMantenimientoDTO
                    {
                        Id = t.Id,
                        Descripcion = t.Descripcion,
                        Orden = t.Orden
                    }).ToList()
                })
                .ToListAsync();

            return Ok(ApiResponse<List<MantenimientoPreventivoDTO>>.SuccessResponse(mantenimientos));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<MantenimientoPreventivoDTO>>> GetById(int id)
        {
            var mantenimiento = await _context.MantenimientosPreventivos
                .Include(mp => mp.Equipo)
                .Include(mp => mp.Tareas)
                .Where(mp => mp.Id == id)
                .Select(mp => new MantenimientoPreventivoDTO
                {
                    Id = mp.Id,
                    Nombre = mp.Nombre,
                    Descripcion = mp.Descripcion,
                    EquipoId = mp.EquipoId,
                    EquipoNombre = mp.Equipo.Nombre,
                    TipoControl = mp.TipoControl,
                    TipoControlNombre = mp.TipoControl.ToString(),
                    IntervaloKilometros = mp.IntervaloKilometros,
                    IntervaloHoras = mp.IntervaloHoras,
                    IntervaloDias = mp.IntervaloDias,
                    UltimoKilometraje = mp.UltimoKilometraje,
                    UltimasHoras = mp.UltimasHoras,
                    UltimaFecha = mp.UltimaFecha,
                    ProximoKilometraje = mp.ProximoKilometraje,
                    ProximasHoras = mp.ProximasHoras,
                    ProximaFecha = mp.ProximaFecha,
                    NotificacionEnviada = mp.NotificacionEnviada,
                    Activo = mp.Activo,
                    FechaCreacion = mp.FechaCreacion,
                    Tareas = mp.Tareas.Select(t => new TareaMantenimientoDTO
                    {
                        Id = t.Id,
                        Descripcion = t.Descripcion,
                        Orden = t.Orden
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (mantenimiento == null)
                return NotFound(ApiResponse<MantenimientoPreventivoDTO>.ErrorResponse("Mantenimiento preventivo no encontrado"));

            return Ok(ApiResponse<MantenimientoPreventivoDTO>.SuccessResponse(mantenimiento));
        }

        [HttpPost]
        [Authorize(Roles = "Administrador,Coordinador")]
        public async Task<ActionResult<ApiResponse<MantenimientoPreventivoDTO>>> Create([FromBody] CreateMantenimientoPreventivoDTO dto)
        {
            var mantenimiento = new MantenimientoPreventivo
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                EquipoId = dto.EquipoId,
                TipoControl = dto.TipoControl,
                IntervaloKilometros = dto.IntervaloKilometros,
                IntervaloHoras = dto.IntervaloHoras,
                IntervaloDias = dto.IntervaloDias,
                Activo = true,
                FechaCreacion = DateTime.Now
            };

            foreach (var tareaDTO in dto.Tareas)
            {
                mantenimiento.Tareas.Add(new MantenimientoPreventivoTarea
                {
                    Descripcion = tareaDTO.Descripcion,
                    Orden = tareaDTO.Orden
                });
            }

            _context.MantenimientosPreventivos.Add(mantenimiento);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = mantenimiento.Id },
                ApiResponse<MantenimientoPreventivoDTO>.SuccessResponse(null, "Mantenimiento preventivo creado exitosamente"));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador,Coordinador")]
        public async Task<ActionResult<ApiResponse<object>>> Update(int id, [FromBody] UpdateMantenimientoPreventivoDTO dto)
        {
            var mantenimiento = await _context.MantenimientosPreventivos.FindAsync(id);
            if (mantenimiento == null)
                return NotFound(ApiResponse<object>.ErrorResponse("Mantenimiento preventivo no encontrado"));

            mantenimiento.Nombre = dto.Nombre;
            mantenimiento.Descripcion = dto.Descripcion;
            mantenimiento.IntervaloKilometros = dto.IntervaloKilometros;
            mantenimiento.IntervaloHoras = dto.IntervaloHoras;
            mantenimiento.IntervaloDias = dto.IntervaloDias;
            mantenimiento.Activo = dto.Activo;

            await _context.SaveChangesAsync();

            return Ok(ApiResponse<object>.SuccessResponse(null, "Mantenimiento preventivo actualizado exitosamente"));
        }
    }
}
