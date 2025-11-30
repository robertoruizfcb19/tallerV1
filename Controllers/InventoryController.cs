using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using tallerV1.Data;
using tallerV1.Models;

namespace tallerV1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InventoryController : ControllerBase
{
    private readonly IWorkshopRepository _repository;

    public InventoryController(IWorkshopRepository repository)
    {
        _repository = repository;
    }

    [HttpGet("bodegas")]
    public ActionResult<IEnumerable<Warehouse>> GetWarehouses()
    {
        return Ok(_repository.GetWarehouses());
    }

    [HttpPost("bodegas/{warehouseId}/stock")]
    public ActionResult<Warehouse> UpsertStock(string warehouseId, [FromBody] StockRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var warehouse = _repository.UpsertStock(warehouseId, new StockItem
        {
            CodigoInterno = request.CodigoInterno,
            Descripcion = request.Descripcion,
            Cantidad = request.Cantidad,
            StockMinimo = request.StockMinimo,
            UnidadMedida = request.UnidadMedida
        });

        return Ok(warehouse);
    }
}

public class StockRequest
{
    [Required]
    [MaxLength(80)]
    public string CodigoInterno { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Descripcion { get; set; } = string.Empty;

    [Range(0, 999999)]
    public decimal Cantidad { get; set; }

    [Range(0, 999999)]
    public decimal StockMinimo { get; set; }

    [MaxLength(40)]
    public string? UnidadMedida { get; set; }
}
