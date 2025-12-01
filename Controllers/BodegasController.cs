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
    public class BodegasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BodegasController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<BodegaDTO>>>> GetAll()
        {
            var bodegas = await _context.Bodegas
                .Select(b => new BodegaDTO
                {
                    Id = b.Id,
                    Nombre = b.Nombre,
                    Ubicacion = b.Ubicacion,
                    Responsable = b.Responsable,
                    Activo = b.Activo,
                    FechaCreacion = b.FechaCreacion,
                    TotalRepuestos = b.Repuestos.Count
                })
                .ToListAsync();

            return Ok(ApiResponse<List<BodegaDTO>>.SuccessResponse(bodegas));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<BodegaDTO>>> GetById(int id)
        {
            var bodega = await _context.Bodegas
                .Where(b => b.Id == id)
                .Select(b => new BodegaDTO
                {
                    Id = b.Id,
                    Nombre = b.Nombre,
                    Ubicacion = b.Ubicacion,
                    Responsable = b.Responsable,
                    Activo = b.Activo,
                    FechaCreacion = b.FechaCreacion,
                    TotalRepuestos = b.Repuestos.Count
                })
                .FirstOrDefaultAsync();

            if (bodega == null)
                return NotFound(ApiResponse<BodegaDTO>.ErrorResponse("Bodega no encontrada"));

            return Ok(ApiResponse<BodegaDTO>.SuccessResponse(bodega));
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<ApiResponse<BodegaDTO>>> Create([FromBody] CreateBodegaDTO dto)
        {
            var bodega = new Bodega
            {
                Nombre = dto.Nombre,
                Ubicacion = dto.Ubicacion,
                Responsable = dto.Responsable,
                Activo = true,
                FechaCreacion = DateTime.Now
            };

            _context.Bodegas.Add(bodega);
            await _context.SaveChangesAsync();

            var bodegaDTO = new BodegaDTO
            {
                Id = bodega.Id,
                Nombre = bodega.Nombre,
                Ubicacion = bodega.Ubicacion,
                Responsable = bodega.Responsable,
                Activo = bodega.Activo,
                FechaCreacion = bodega.FechaCreacion,
                TotalRepuestos = 0
            };

            return CreatedAtAction(nameof(GetById), new { id = bodega.Id },
                ApiResponse<BodegaDTO>.SuccessResponse(bodegaDTO, "Bodega creada exitosamente"));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<ApiResponse<BodegaDTO>>> Update(int id, [FromBody] UpdateBodegaDTO dto)
        {
            var bodega = await _context.Bodegas.FindAsync(id);
            if (bodega == null)
                return NotFound(ApiResponse<BodegaDTO>.ErrorResponse("Bodega no encontrada"));

            bodega.Nombre = dto.Nombre;
            bodega.Ubicacion = dto.Ubicacion;
            bodega.Responsable = dto.Responsable;
            bodega.Activo = dto.Activo;

            await _context.SaveChangesAsync();

            var bodegaDTO = new BodegaDTO
            {
                Id = bodega.Id,
                Nombre = bodega.Nombre,
                Ubicacion = bodega.Ubicacion,
                Responsable = bodega.Responsable,
                Activo = bodega.Activo,
                FechaCreacion = bodega.FechaCreacion,
                TotalRepuestos = await _context.Repuestos.CountAsync(r => r.BodegaId == bodega.Id)
            };

            return Ok(ApiResponse<BodegaDTO>.SuccessResponse(bodegaDTO, "Bodega actualizada exitosamente"));
        }
    }
}
