using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tallerV1.Data;
using tallerV1.Models.DTOs;
using tallerV1.Models.Entities;
using System.Security.Claims;

namespace tallerV1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RepuestosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RepuestosController> _logger;

        public RepuestosController(ApplicationDbContext context, ILogger<RepuestosController> logger)
        {
            _context = context;
            _logger = logger;
        }

        private int GetCurrentUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<RepuestoDTO>>>> GetAll([FromQuery] int? bodegaId, [FromQuery] bool? stockBajo)
        {
            try
            {
                var query = _context.Repuestos.Include(r => r.Bodega).AsQueryable();

                if (bodegaId.HasValue)
                    query = query.Where(r => r.BodegaId == bodegaId.Value);

                if (stockBajo.HasValue && stockBajo.Value)
                    query = query.Where(r => r.StockActual < r.StockMinimo);

                var repuestos = await query
                    .OrderBy(r => r.Codigo)
                    .Select(r => new RepuestoDTO
                    {
                        Id = r.Id,
                        Codigo = r.Codigo,
                        CodigoBarras = r.CodigoBarras,
                        Nombre = r.Nombre,
                        Descripcion = r.Descripcion,
                        Marca = r.Marca,
                        UnidadMedida = r.UnidadMedida,
                        BodegaId = r.BodegaId,
                        BodegaNombre = r.Bodega.Nombre,
                        StockActual = r.StockActual,
                        StockMinimo = r.StockMinimo,
                        StockMaximo = r.StockMaximo,
                        PrecioUnitario = r.PrecioUnitario,
                        MesesGarantia = r.MesesGarantia,
                        Proveedor = r.Proveedor,
                        Activo = r.Activo,
                        StockBajo = r.StockActual < r.StockMinimo,
                        FechaCreacion = r.FechaCreacion
                    })
                    .ToListAsync();

                return Ok(ApiResponse<List<RepuestoDTO>>.SuccessResponse(repuestos));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener repuestos");
                return StatusCode(500, ApiResponse<List<RepuestoDTO>>.ErrorResponse("Error interno del servidor"));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<RepuestoDTO>>> GetById(int id)
        {
            try
            {
                var repuesto = await _context.Repuestos
                    .Include(r => r.Bodega)
                    .Where(r => r.Id == id)
                    .Select(r => new RepuestoDTO
                    {
                        Id = r.Id,
                        Codigo = r.Codigo,
                        CodigoBarras = r.CodigoBarras,
                        Nombre = r.Nombre,
                        Descripcion = r.Descripcion,
                        Marca = r.Marca,
                        UnidadMedida = r.UnidadMedida,
                        BodegaId = r.BodegaId,
                        BodegaNombre = r.Bodega.Nombre,
                        StockActual = r.StockActual,
                        StockMinimo = r.StockMinimo,
                        StockMaximo = r.StockMaximo,
                        PrecioUnitario = r.PrecioUnitario,
                        MesesGarantia = r.MesesGarantia,
                        Proveedor = r.Proveedor,
                        Activo = r.Activo,
                        StockBajo = r.StockActual < r.StockMinimo,
                        FechaCreacion = r.FechaCreacion
                    })
                    .FirstOrDefaultAsync();

                if (repuesto == null)
                    return NotFound(ApiResponse<RepuestoDTO>.ErrorResponse("Repuesto no encontrado"));

                return Ok(ApiResponse<RepuestoDTO>.SuccessResponse(repuesto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener repuesto {RepuestoId}", id);
                return StatusCode(500, ApiResponse<RepuestoDTO>.ErrorResponse("Error interno del servidor"));
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrador,Coordinador,Bodeguero")]
        public async Task<ActionResult<ApiResponse<RepuestoDTO>>> Create([FromBody] CreateRepuestoDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ApiResponse<RepuestoDTO>.ErrorResponse("Datos inválidos"));

                if (await _context.Repuestos.AnyAsync(r => r.Codigo == dto.Codigo))
                    return BadRequest(ApiResponse<RepuestoDTO>.ErrorResponse("El código ya existe"));

                var repuesto = new Repuesto
                {
                    Codigo = dto.Codigo,
                    CodigoBarras = dto.CodigoBarras,
                    Nombre = dto.Nombre,
                    Descripcion = dto.Descripcion,
                    Marca = dto.Marca,
                    UnidadMedida = dto.UnidadMedida,
                    BodegaId = dto.BodegaId,
                    StockActual = dto.StockActual,
                    StockMinimo = dto.StockMinimo,
                    StockMaximo = dto.StockMaximo,
                    PrecioUnitario = dto.PrecioUnitario,
                    MesesGarantia = dto.MesesGarantia,
                    Proveedor = dto.Proveedor,
                    Activo = true,
                    FechaCreacion = DateTime.Now
                };

                _context.Repuestos.Add(repuesto);
                await _context.SaveChangesAsync();

                // Registrar movimiento inicial si hay stock
                if (repuesto.StockActual > 0)
                {
                    var movimiento = new MovimientoInventario
                    {
                        RepuestoId = repuesto.Id,
                        TipoMovimiento = "Entrada",
                        Cantidad = repuesto.StockActual,
                        StockAnterior = 0,
                        StockNuevo = repuesto.StockActual,
                        Observaciones = "Stock inicial",
                        UsuarioId = GetCurrentUserId(),
                        FechaMovimiento = DateTime.Now
                    };
                    _context.MovimientosInventario.Add(movimiento);
                    await _context.SaveChangesAsync();
                }

                await _context.Entry(repuesto).Reference(r => r.Bodega).LoadAsync();

                var repuestoDTO = new RepuestoDTO
                {
                    Id = repuesto.Id,
                    Codigo = repuesto.Codigo,
                    CodigoBarras = repuesto.CodigoBarras,
                    Nombre = repuesto.Nombre,
                    Descripcion = repuesto.Descripcion,
                    Marca = repuesto.Marca,
                    UnidadMedida = repuesto.UnidadMedida,
                    BodegaId = repuesto.BodegaId,
                    BodegaNombre = repuesto.Bodega.Nombre,
                    StockActual = repuesto.StockActual,
                    StockMinimo = repuesto.StockMinimo,
                    StockMaximo = repuesto.StockMaximo,
                    PrecioUnitario = repuesto.PrecioUnitario,
                    MesesGarantia = repuesto.MesesGarantia,
                    Proveedor = repuesto.Proveedor,
                    Activo = repuesto.Activo,
                    StockBajo = repuesto.StockActual < repuesto.StockMinimo,
                    FechaCreacion = repuesto.FechaCreacion
                };

                return CreatedAtAction(nameof(GetById), new { id = repuesto.Id },
                    ApiResponse<RepuestoDTO>.SuccessResponse(repuestoDTO, "Repuesto creado exitosamente"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear repuesto");
                return StatusCode(500, ApiResponse<RepuestoDTO>.ErrorResponse("Error interno del servidor"));
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador,Coordinador,Bodeguero")]
        public async Task<ActionResult<ApiResponse<RepuestoDTO>>> Update(int id, [FromBody] UpdateRepuestoDTO dto)
        {
            try
            {
                var repuesto = await _context.Repuestos.Include(r => r.Bodega).FirstOrDefaultAsync(r => r.Id == id);
                if (repuesto == null)
                    return NotFound(ApiResponse<RepuestoDTO>.ErrorResponse("Repuesto no encontrado"));

                repuesto.Nombre = dto.Nombre;
                repuesto.Descripcion = dto.Descripcion;
                repuesto.Marca = dto.Marca;
                repuesto.UnidadMedida = dto.UnidadMedida;
                repuesto.StockMinimo = dto.StockMinimo;
                repuesto.StockMaximo = dto.StockMaximo;
                repuesto.PrecioUnitario = dto.PrecioUnitario;
                repuesto.MesesGarantia = dto.MesesGarantia;
                repuesto.Proveedor = dto.Proveedor;
                repuesto.Activo = dto.Activo;

                await _context.SaveChangesAsync();

                var repuestoDTO = new RepuestoDTO
                {
                    Id = repuesto.Id,
                    Codigo = repuesto.Codigo,
                    CodigoBarras = repuesto.CodigoBarras,
                    Nombre = repuesto.Nombre,
                    Descripcion = repuesto.Descripcion,
                    Marca = repuesto.Marca,
                    UnidadMedida = repuesto.UnidadMedida,
                    BodegaId = repuesto.BodegaId,
                    BodegaNombre = repuesto.Bodega.Nombre,
                    StockActual = repuesto.StockActual,
                    StockMinimo = repuesto.StockMinimo,
                    StockMaximo = repuesto.StockMaximo,
                    PrecioUnitario = repuesto.PrecioUnitario,
                    MesesGarantia = repuesto.MesesGarantia,
                    Proveedor = repuesto.Proveedor,
                    Activo = repuesto.Activo,
                    StockBajo = repuesto.StockActual < repuesto.StockMinimo,
                    FechaCreacion = repuesto.FechaCreacion
                };

                return Ok(ApiResponse<RepuestoDTO>.SuccessResponse(repuestoDTO, "Repuesto actualizado exitosamente"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar repuesto {RepuestoId}", id);
                return StatusCode(500, ApiResponse<RepuestoDTO>.ErrorResponse("Error interno del servidor"));
            }
        }

        [HttpPost("movimiento")]
        [Authorize(Roles = "Administrador,Coordinador,Bodeguero")]
        public async Task<ActionResult<ApiResponse<MovimientoInventarioDTO>>> CrearMovimiento([FromBody] CreateMovimientoInventarioDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ApiResponse<MovimientoInventarioDTO>.ErrorResponse("Datos inválidos"));

                var repuesto = await _context.Repuestos.FindAsync(dto.RepuestoId);
                if (repuesto == null)
                    return NotFound(ApiResponse<MovimientoInventarioDTO>.ErrorResponse("Repuesto no encontrado"));

                var stockAnterior = repuesto.StockActual;
                var stockNuevo = stockAnterior;

                switch (dto.TipoMovimiento.ToUpper())
                {
                    case "ENTRADA":
                        stockNuevo = stockAnterior + dto.Cantidad;
                        break;
                    case "SALIDA":
                        if (stockAnterior < dto.Cantidad)
                            return BadRequest(ApiResponse<MovimientoInventarioDTO>.ErrorResponse("Stock insuficiente"));
                        stockNuevo = stockAnterior - dto.Cantidad;
                        break;
                    case "AJUSTE":
                        stockNuevo = dto.Cantidad;
                        break;
                    default:
                        return BadRequest(ApiResponse<MovimientoInventarioDTO>.ErrorResponse("Tipo de movimiento inválido (Entrada, Salida, Ajuste)"));
                }

                var movimiento = new MovimientoInventario
                {
                    RepuestoId = dto.RepuestoId,
                    TipoMovimiento = dto.TipoMovimiento,
                    Cantidad = dto.Cantidad,
                    StockAnterior = stockAnterior,
                    StockNuevo = stockNuevo,
                    Observaciones = dto.Observaciones,
                    UsuarioId = GetCurrentUserId(),
                    FechaMovimiento = DateTime.Now
                };

                repuesto.StockActual = stockNuevo;

                _context.MovimientosInventario.Add(movimiento);
                await _context.SaveChangesAsync();

                await _context.Entry(movimiento).Reference(m => m.Repuesto).LoadAsync();
                await _context.Entry(movimiento).Reference(m => m.Usuario).LoadAsync();

                var movimientoDTO = new MovimientoInventarioDTO
                {
                    Id = movimiento.Id,
                    RepuestoId = movimiento.RepuestoId,
                    RepuestoNombre = movimiento.Repuesto.Nombre,
                    TipoMovimiento = movimiento.TipoMovimiento,
                    Cantidad = movimiento.Cantidad,
                    StockAnterior = movimiento.StockAnterior,
                    StockNuevo = movimiento.StockNuevo,
                    Observaciones = movimiento.Observaciones,
                    UsuarioId = movimiento.UsuarioId,
                    UsuarioNombre = movimiento.Usuario.NombreCompleto,
                    FechaMovimiento = movimiento.FechaMovimiento
                };

                return Ok(ApiResponse<MovimientoInventarioDTO>.SuccessResponse(movimientoDTO, "Movimiento registrado exitosamente"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear movimiento de inventario");
                return StatusCode(500, ApiResponse<MovimientoInventarioDTO>.ErrorResponse("Error interno del servidor"));
            }
        }

        [HttpGet("{id}/movimientos")]
        public async Task<ActionResult<ApiResponse<List<MovimientoInventarioDTO>>>> GetMovimientos(int id)
        {
            try
            {
                var movimientos = await _context.MovimientosInventario
                    .Include(m => m.Repuesto)
                    .Include(m => m.Usuario)
                    .Where(m => m.RepuestoId == id)
                    .OrderByDescending(m => m.FechaMovimiento)
                    .Select(m => new MovimientoInventarioDTO
                    {
                        Id = m.Id,
                        RepuestoId = m.RepuestoId,
                        RepuestoNombre = m.Repuesto.Nombre,
                        TipoMovimiento = m.TipoMovimiento,
                        Cantidad = m.Cantidad,
                        StockAnterior = m.StockAnterior,
                        StockNuevo = m.StockNuevo,
                        Observaciones = m.Observaciones,
                        UsuarioId = m.UsuarioId,
                        UsuarioNombre = m.Usuario.NombreCompleto,
                        FechaMovimiento = m.FechaMovimiento
                    })
                    .ToListAsync();

                return Ok(ApiResponse<List<MovimientoInventarioDTO>>.SuccessResponse(movimientos));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener movimientos del repuesto {RepuestoId}", id);
                return StatusCode(500, ApiResponse<List<MovimientoInventarioDTO>>.ErrorResponse("Error interno del servidor"));
            }
        }
    }
}
