using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using tallerV1.Data;
using tallerV1.Models;

namespace tallerV1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OperationsController : ControllerBase
{
    private readonly IWorkshopRepository _repository;

    public OperationsController(IWorkshopRepository repository)
    {
        _repository = repository;
    }

    [HttpGet("checklists/plantillas")]
    public ActionResult<IEnumerable<ChecklistTemplate>> GetTemplates([FromQuery] AssetType? tipo)
    {
        return Ok(_repository.GetChecklistTemplates(tipo));
    }

    [HttpPost("checklists/plantillas")]
    public ActionResult<ChecklistTemplate> AddTemplate([FromBody] ChecklistTemplate template)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var created = _repository.AddChecklistTemplate(template);
        return CreatedAtAction(nameof(GetTemplates), new { id = created.Id }, created);
    }

    [HttpGet("agenda")]
    public ActionResult<IEnumerable<AgendaSlot>> GetAgenda([FromQuery] string? sede, [FromQuery] DateTime? desde, [FromQuery] DateTime? hasta)
    {
        return Ok(_repository.GetAgenda(sede, desde, hasta));
    }

    [HttpGet("notificaciones")]
    public ActionResult<IEnumerable<NotificationMessage>> GetNotifications()
    {
        return Ok(_repository.GetNotifications());
    }

    [HttpPost("bitacora")]
    public ActionResult<BitacoraEntry> AddBitacora([FromBody] BitacoraCreateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var entry = new BitacoraEntry
        {
            ActivoId = request.ActivoId,
            Autor = request.Autor,
            Descripcion = request.Descripcion,
            Fecha = request.Fecha ?? DateTime.UtcNow
        };

        var created = _repository.AddBitacoraEntry(entry);
        return Created(string.Empty, created);
    }
}

public class BitacoraCreateRequest
{
    [Required]
    public Guid ActivoId { get; set; }

    [Required]
    [MaxLength(500)]
    public string Descripcion { get; set; } = string.Empty;

    [Required]
    [MaxLength(80)]
    public string Autor { get; set; } = string.Empty;

    public DateTime? Fecha { get; set; }
}
