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
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<DashboardDTO>>> Get()
        {
            var dashboard = new DashboardDTO();

            // Estadísticas generales
            dashboard.EstadisticasGenerales = new EstadisticasGeneralesDTO
            {
                TotalEquipos = await _context.Equipos.CountAsync(),
                EquiposActivos = await _context.Equipos.CountAsync(e => e.Activo),
                TotalOrdenes = await _context.OrdenesTrabajo.CountAsync(),
                OrdenesAbiertas = await _context.OrdenesTrabajo.CountAsync(ot =>
                    ot.Estado != EstadoOrdenEnum.Completada &&
                    ot.Estado != EstadoOrdenEnum.Entregada &&
                    ot.Estado != EstadoOrdenEnum.Cancelada),
                OrdenesCompletadas = await _context.OrdenesTrabajo.CountAsync(ot =>
                    ot.Estado == EstadoOrdenEnum.Completada ||
                    ot.Estado == EstadoOrdenEnum.Entregada),
                CostoTotalMes = await _context.OrdenesTrabajo
                    .Where(ot => ot.FechaCreacion.Month == DateTime.Now.Month && ot.FechaCreacion.Year == DateTime.Now.Year)
                    .SumAsync(ot => ot.CostoTotal ?? 0),
                TotalRepuestos = await _context.Repuestos.CountAsync(),
                RepuestosBajoStock = await _context.Repuestos.CountAsync(r => r.StockActual < r.StockMinimo)
            };

            // Órdenes por estado
            dashboard.OrdenesPorEstado = await _context.OrdenesTrabajo
                .GroupBy(ot => ot.Estado)
                .Select(g => new OrdenPorEstadoDTO
                {
                    Estado = g.Key.ToString(),
                    Cantidad = g.Count()
                })
                .ToListAsync();

            // Equipos con más fallas (top 5)
            dashboard.EquiposConMasFallas = await _context.OrdenesTrabajo
                .Where(ot => ot.TipoMantenimiento == TipoMantenimientoEnum.Correctivo)
                .GroupBy(ot => new { ot.EquipoId, ot.Equipo.Codigo, ot.Equipo.Nombre })
                .Select(g => new EquipoConMasFallasDTO
                {
                    EquipoId = g.Key.EquipoId,
                    Codigo = g.Key.Codigo,
                    Nombre = g.Key.Nombre,
                    CantidadFallas = g.Count()
                })
                .OrderByDescending(e => e.CantidadFallas)
                .Take(5)
                .ToListAsync();

            // Repuestos bajo stock
            dashboard.RepuestosBajoStock = await _context.Repuestos
                .Where(r => r.StockActual < r.StockMinimo && r.Activo)
                .Select(r => new RepuestoBajoStockDTO
                {
                    RepuestoId = r.Id,
                    Codigo = r.Codigo,
                    Nombre = r.Nombre,
                    StockActual = r.StockActual,
                    StockMinimo = r.StockMinimo
                })
                .OrderBy(r => r.StockActual)
                .Take(10)
                .ToListAsync();

            // Mantenimientos próximos
            dashboard.MantenimientosProximos = await _context.MantenimientosPreventivos
                .Include(mp => mp.Equipo)
                .Where(mp => mp.Activo)
                .Where(mp => mp.ProximaFecha != null && mp.ProximaFecha <= DateTime.Now.AddDays(30))
                .Select(mp => new MantenimientoProximoDTO
                {
                    EquipoId = mp.EquipoId,
                    EquipoCodigo = mp.Equipo.Codigo,
                    EquipoNombre = mp.Equipo.Nombre,
                    TipoMantenimiento = mp.Nombre,
                    FechaProxima = mp.ProximaFecha,
                    KilometrajeProximo = mp.ProximoKilometraje,
                    HorasProximas = mp.ProximasHoras,
                    DiasRestantes = mp.ProximaFecha.HasValue ? (mp.ProximaFecha.Value - DateTime.Now).Days : 0
                })
                .OrderBy(m => m.DiasRestantes)
                .Take(10)
                .ToListAsync();

            return Ok(ApiResponse<DashboardDTO>.SuccessResponse(dashboard));
        }

        [HttpGet("estadisticas-mensuales")]
        public async Task<ActionResult<ApiResponse<object>>> GetEstadisticasMensuales([FromQuery] int anio)
        {
            var estadisticas = await _context.OrdenesTrabajo
                .Where(ot => ot.FechaCreacion.Year == anio)
                .GroupBy(ot => ot.FechaCreacion.Month)
                .Select(g => new
                {
                    Mes = g.Key,
                    TotalOrdenes = g.Count(),
                    CostoTotal = g.Sum(ot => ot.CostoTotal ?? 0),
                    OrdenesCompletadas = g.Count(ot => ot.Estado == EstadoOrdenEnum.Completada || ot.Estado == EstadoOrdenEnum.Entregada)
                })
                .OrderBy(e => e.Mes)
                .ToListAsync();

            return Ok(ApiResponse<object>.SuccessResponse(estadisticas));
        }
    }
}
