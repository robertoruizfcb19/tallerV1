using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tallerV1.Data;
using tallerV1.Models.DTOs;
using System.Security.Claims;

namespace tallerV1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotificacionesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public NotificacionesController(ApplicationDbContext context)
        {
            _context = context;
        }

        private int GetCurrentUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

        [HttpGet("mis-notificaciones")]
        public async Task<ActionResult<ApiResponse<List<NotificacionDTO>>>> GetMisNotificaciones([FromQuery] bool? soloNoLeidas)
        {
            var userId = GetCurrentUserId();
            var query = _context.Notificaciones
                .Include(n => n.OrdenTrabajo)
                .Where(n => n.UsuarioId == userId);

            if (soloNoLeidas.HasValue && soloNoLeidas.Value)
                query = query.Where(n => !n.Leida);

            var notificaciones = await query
                .OrderByDescending(n => n.FechaCreacion)
                .Select(n => new NotificacionDTO
                {
                    Id = n.Id,
                    Titulo = n.Titulo,
                    Mensaje = n.Mensaje,
                    TipoNotificacion = n.TipoNotificacion,
                    Leida = n.Leida,
                    FechaCreacion = n.FechaCreacion,
                    FechaLeida = n.FechaLeida,
                    OrdenTrabajoId = n.OrdenTrabajoId,
                    NumeroOrden = n.OrdenTrabajo != null ? n.OrdenTrabajo.NumeroOrden : null
                })
                .ToListAsync();

            return Ok(ApiResponse<List<NotificacionDTO>>.SuccessResponse(notificaciones));
        }

        [HttpPost("{id}/marcar-leida")]
        public async Task<ActionResult<ApiResponse<object>>> MarcarLeida(int id)
        {
            var userId = GetCurrentUserId();
            var notificacion = await _context.Notificaciones
                .FirstOrDefaultAsync(n => n.Id == id && n.UsuarioId == userId);

            if (notificacion == null)
                return NotFound(ApiResponse<object>.ErrorResponse("Notificación no encontrada"));

            notificacion.Leida = true;
            notificacion.FechaLeida = DateTime.Now;
            await _context.SaveChangesAsync();

            return Ok(ApiResponse<object>.SuccessResponse(null, "Notificación marcada como leída"));
        }

        [HttpPost("marcar-todas-leidas")]
        public async Task<ActionResult<ApiResponse<object>>> MarcarTodasLeidas()
        {
            var userId = GetCurrentUserId();
            var notificaciones = await _context.Notificaciones
                .Where(n => n.UsuarioId == userId && !n.Leida)
                .ToListAsync();

            foreach (var notificacion in notificaciones)
            {
                notificacion.Leida = true;
                notificacion.FechaLeida = DateTime.Now;
            }

            await _context.SaveChangesAsync();

            return Ok(ApiResponse<object>.SuccessResponse(null, "Todas las notificaciones marcadas como leídas"));
        }

        [HttpGet("contador-no-leidas")]
        public async Task<ActionResult<ApiResponse<int>>> GetContadorNoLeidas()
        {
            var userId = GetCurrentUserId();
            var contador = await _context.Notificaciones
                .CountAsync(n => n.UsuarioId == userId && !n.Leida);

            return Ok(ApiResponse<int>.SuccessResponse(contador));
        }
    }
}
