using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tallerV1.Data;
using tallerV1.Models.DTOs;

namespace tallerV1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class HistorialEquiposController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public HistorialEquiposController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("equipo/{equipoId}")]
        public async Task<ActionResult<ApiResponse<object>>> GetByEquipo(int equipoId)
        {
            var historial = await _context.HistorialEquipos
                .Include(h => h.Usuario)
                .Include(h => h.OrdenTrabajo)
                .Where(h => h.EquipoId == equipoId)
                .OrderByDescending(h => h.FechaEvento)
                .Select(h => new
                {
                    h.Id,
                    h.TipoEvento,
                    h.Descripcion,
                    UsuarioNombre = h.Usuario.NombreCompleto,
                    h.FechaEvento,
                    OrdenTrabajoNumero = h.OrdenTrabajo != null ? h.OrdenTrabajo.NumeroOrden : null
                })
                .ToListAsync();

            return Ok(ApiResponse<object>.SuccessResponse(historial));
        }
    }
}
