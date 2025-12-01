using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tallerV1.Data;
using tallerV1.Models.DTOs;
using tallerV1.Models.Enums;

namespace tallerV1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrador,Coordinador,Visualizador")]
    public class ReportesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ReportesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("ordenes-por-periodo")]
        public async Task<ActionResult<ApiResponse<object>>> GetOrdenesPorPeriodo([FromQuery] DateTime fechaInicio, [FromQuery] DateTime fechaFin)
        {
            var ordenes = await _context.OrdenesTrabajo
                .Include(ot => ot.Equipo)
                .Include(ot => ot.UsuarioCreador)
                .Where(ot => ot.FechaCreacion >= fechaInicio && ot.FechaCreacion <= fechaFin)
                .Select(ot => new
                {
                    ot.NumeroOrden,
                    EquipoCodigo = ot.Equipo.Codigo,
                    EquipoNombre = ot.Equipo.Nombre,
                    TipoMantenimiento = ot.TipoMantenimiento.ToString(),
                    Estado = ot.Estado.ToString(),
                    ot.CostoTotal,
                    ot.FechaCreacion,
                    ot.FechaFinalizacion
                })
                .ToListAsync();

            return Ok(ApiResponse<object>.SuccessResponse(ordenes));
        }

        [HttpGet("costos-por-equipo")]
        public async Task<ActionResult<ApiResponse<object>>> GetCostosPorEquipo([FromQuery] DateTime? fechaInicio, [FromQuery] DateTime? fechaFin)
        {
            var query = _context.OrdenesTrabajo.Include(ot => ot.Equipo).AsQueryable();

            if (fechaInicio.HasValue)
                query = query.Where(ot => ot.FechaCreacion >= fechaInicio.Value);

            if (fechaFin.HasValue)
                query = query.Where(ot => ot.FechaCreacion <= fechaFin.Value);

            var costos = await query
                .GroupBy(ot => new { ot.EquipoId, ot.Equipo.Codigo, ot.Equipo.Nombre })
                .Select(g => new
                {
                    EquipoCodigo = g.Key.Codigo,
                    EquipoNombre = g.Key.Nombre,
                    TotalOrdenes = g.Count(),
                    CostoTotal = g.Sum(ot => ot.CostoTotal ?? 0),
                    CostoPromedio = g.Average(ot => ot.CostoTotal ?? 0),
                    CostoRepuestos = g.Sum(ot => ot.CostoRepuestos ?? 0),
                    CostoManoObra = g.Sum(ot => ot.CostoManoObra ?? 0)
                })
                .OrderByDescending(c => c.CostoTotal)
                .ToListAsync();

            return Ok(ApiResponse<object>.SuccessResponse(costos));
        }

        [HttpGet("disponibilidad-flota")]
        public async Task<ActionResult<ApiResponse<object>>> GetDisponibilidadFlota()
        {
            var totalEquipos = await _context.Equipos.CountAsync(e => e.Activo);
            var equiposEnMantenimiento = await _context.OrdenesTrabajo
                .Where(ot => ot.Estado == EstadoOrdenEnum.EnDiagnostico ||
                            ot.Estado == EstadoOrdenEnum.EnEjecucion ||
                            ot.Estado == EstadoOrdenEnum.Presupuestada)
                .Select(ot => ot.EquipoId)
                .Distinct()
                .CountAsync();

            var equiposDisponibles = totalEquipos - equiposEnMantenimiento;
            var porcentajeDisponibilidad = totalEquipos > 0 ? (decimal)equiposDisponibles / totalEquipos * 100 : 0;

            var resultado = new
            {
                TotalEquipos = totalEquipos,
                EquiposDisponibles = equiposDisponibles,
                EquiposEnMantenimiento = equiposEnMantenimiento,
                PorcentajeDisponibilidad = Math.Round(porcentajeDisponibilidad, 2)
            };

            return Ok(ApiResponse<object>.SuccessResponse(resultado));
        }

        [HttpGet("consumo-repuestos")]
        public async Task<ActionResult<ApiResponse<object>>> GetConsumoRepuestos([FromQuery] DateTime fechaInicio, [FromQuery] DateTime fechaFin)
        {
            var consumo = await _context.MovimientosInventario
                .Include(m => m.Repuesto)
                .Where(m => m.TipoMovimiento == "Salida" &&
                           m.FechaMovimiento >= fechaInicio &&
                           m.FechaMovimiento <= fechaFin)
                .GroupBy(m => new { m.RepuestoId, m.Repuesto.Codigo, m.Repuesto.Nombre })
                .Select(g => new
                {
                    RepuestoCodigo = g.Key.Codigo,
                    RepuestoNombre = g.Key.Nombre,
                    CantidadTotal = g.Sum(m => m.Cantidad),
                    NumeroMovimientos = g.Count()
                })
                .OrderByDescending(c => c.CantidadTotal)
                .ToListAsync();

            return Ok(ApiResponse<object>.SuccessResponse(consumo));
        }

        [HttpGet("productividad-tecnicos")]
        public async Task<ActionResult<ApiResponse<object>>> GetProductividadTecnicos([FromQuery] DateTime fechaInicio, [FromQuery] DateTime fechaFin)
        {
            var productividad = await _context.OrdenesTrabajo
                .Include(ot => ot.UsuarioAsignado)
                .Where(ot => ot.UsuarioAsignadoId != null &&
                            ot.FechaCreacion >= fechaInicio &&
                            ot.FechaCreacion <= fechaFin)
                .GroupBy(ot => new { ot.UsuarioAsignadoId, ot.UsuarioAsignado!.NombreCompleto })
                .Select(g => new
                {
                    TecnicoNombre = g.Key.NombreCompleto,
                    TotalOrdenes = g.Count(),
                    OrdenesCompletadas = g.Count(ot => ot.Estado == EstadoOrdenEnum.Completada || ot.Estado == EstadoOrdenEnum.Entregada),
                    OrdenesEnProceso = g.Count(ot => ot.Estado == EstadoOrdenEnum.EnEjecucion || ot.Estado == EstadoOrdenEnum.EnDiagnostico),
                    TiempoPromedioHoras = g.Where(ot => ot.FechaInicio != null && ot.FechaFinalizacion != null)
                        .Average(ot => (ot.FechaFinalizacion!.Value - ot.FechaInicio!.Value).TotalHours)
                })
                .OrderByDescending(p => p.OrdenesCompletadas)
                .ToListAsync();

            return Ok(ApiResponse<object>.SuccessResponse(productividad));
        }

        [HttpGet("mantenimientos-vencidos")]
        public async Task<ActionResult<ApiResponse<object>>> GetMantenimientosVencidos()
        {
            var vencidos = await _context.MantenimientosPreventivos
                .Include(mp => mp.Equipo)
                .Where(mp => mp.Activo && mp.ProximaFecha < DateTime.Now)
                .Select(mp => new
                {
                    EquipoCodigo = mp.Equipo.Codigo,
                    EquipoNombre = mp.Equipo.Nombre,
                    MantenimientoNombre = mp.Nombre,
                    FechaVencimiento = mp.ProximaFecha,
                    DiasVencidos = (DateTime.Now - mp.ProximaFecha!.Value).Days
                })
                .OrderBy(m => m.FechaVencimiento)
                .ToListAsync();

            return Ok(ApiResponse<object>.SuccessResponse(vencidos));
        }
    }
}
