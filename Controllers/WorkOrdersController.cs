using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using tallerV1.Data;
using tallerV1.Models;

namespace tallerV1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WorkOrdersController : ControllerBase
{
    private readonly IWorkshopRepository _repository;

    public WorkOrdersController(IWorkshopRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public ActionResult<IEnumerable<WorkOrder>> Get([FromQuery] WorkOrderStatus? estado, [FromQuery] WorkOrderType? tipo)
    {
        var orders = _repository.GetWorkOrders(estado, tipo);
        return Ok(orders);
    }

    [HttpGet("{id:guid}")]
    public ActionResult<WorkOrder> GetById(Guid id)
    {
        var order = _repository.GetWorkOrder(id);
        return order is null ? NotFound() : Ok(order);
    }

    [HttpPost]
    public ActionResult<WorkOrder> Create([FromBody] WorkOrderCreateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var workOrder = new WorkOrder
        {
            ActivoId = request.ActivoId,
            Titulo = request.Titulo,
            Descripcion = request.Descripcion,
            Tipo = request.Tipo,
            Prioridad = request.Prioridad,
            Sede = request.Sede,
            FechaProgramada = request.FechaProgramada,
            TecnicoAsignado = request.TecnicoAsignado,
            HorasEstimadas = request.HorasEstimadas,
            CentroCostoId = request.CentroCostoId,
            Bahia = request.Bahia,
            Turno = request.Turno
        };

        var created = _repository.AddWorkOrder(workOrder);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPost("{id:guid}/estado")]
    public IActionResult UpdateStatus(Guid id, [FromBody] WorkOrderStatusUpdateRequest request)
    {
        var ok = _repository.TryUpdateWorkOrderStatus(id, request.Estado, request.Autor, request.Comentario);
        return ok ? NoContent() : NotFound();
    }

    [HttpPost("{id:guid}/checklist")]
    public IActionResult AddChecklist(Guid id, [FromBody] ChecklistResult checklist)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var ok = _repository.TryAddChecklist(id, checklist);
        return ok ? NoContent() : NotFound();
    }

    [HttpPost("{id:guid}/materiales")]
    public IActionResult AddMaterials(Guid id, [FromBody] IEnumerable<MaterialUsage> materials)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var ok = _repository.TryAddMaterials(id, materials);
        return ok ? NoContent() : NotFound();
    }

    [HttpPost("{id:guid}/agenda")]
    public ActionResult<AgendaSlot> Schedule(Guid id, [FromBody] WorkOrderScheduleRequest request)
    {
        if (request.Inicio >= request.Fin)
        {
            return BadRequest("El fin debe ser mayor que el inicio");
        }

        var slot = _repository.SchedulePreventive(id, request.Sede, request.Inicio, request.Fin, request.Bahia, request.Turno);
        return slot is null ? NotFound() : Ok(slot);
    }

    [HttpPost("{id:guid}/labor")]
    public IActionResult RegisterLabor(Guid id, [FromBody] LaborRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var ok = _repository.TryRegisterLabor(id, new LaborTimeEntry
        {
            Tecnico = request.Tecnico,
            Horas = request.Horas,
            TarifaHora = request.TarifaHora,
            Fecha = request.Fecha ?? DateTime.UtcNow
        });

        return ok ? NoContent() : NotFound();
    }

    [HttpPost("{id:guid}/consumo")]
    public ActionResult<IEnumerable<StockAlert>> ConsumeFromWarehouse(Guid id, [FromBody] ConsumeRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.WarehouseId))
        {
            return BadRequest("Debe indicar una bodega");
        }

        var ok = _repository.TryConsumeStock(id, request.WarehouseId, request.Materiales, out var alerts);
        return ok ? Ok(alerts) : NotFound();
    }
}

public class WorkOrderCreateRequest
{
    [Required]
    public Guid ActivoId { get; set; }

    [Required]
    [MaxLength(140)]
    public string Titulo { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Descripcion { get; set; }

    public WorkOrderType Tipo { get; set; }

    public WorkOrderPriority Prioridad { get; set; } = WorkOrderPriority.Media;

    [MaxLength(80)]
    public string? Sede { get; set; }

    public DateTime? FechaProgramada { get; set; }

    public string? TecnicoAsignado { get; set; }

    public double? HorasEstimadas { get; set; }

    public string? CentroCostoId { get; set; }

    public string? Bahia { get; set; }

    public string? Turno { get; set; }
}

public class WorkOrderStatusUpdateRequest
{
    public WorkOrderStatus Estado { get; set; }

    public string? Autor { get; set; }

    public string? Comentario { get; set; }
}

public class WorkOrderScheduleRequest
{
    [Required]
    [MaxLength(80)]
    public string Sede { get; set; } = string.Empty;

    [MaxLength(40)]
    public string? Bahia { get; set; }

    [MaxLength(40)]
    public string? Turno { get; set; }

    public DateTime Inicio { get; set; }

    public DateTime Fin { get; set; }
}

public class LaborRequest
{
    [Required]
    [MaxLength(80)]
    public string Tecnico { get; set; } = string.Empty;

    [Range(0.1, 24)]
    public double Horas { get; set; }

    [Range(0, 10000)]
    public decimal TarifaHora { get; set; }

    public DateTime? Fecha { get; set; }
}

public class ConsumeRequest
{
    [Required]
    public string WarehouseId { get; set; } = string.Empty;

    [Required]
    public List<MaterialUsage> Materiales { get; set; } = new();
}
