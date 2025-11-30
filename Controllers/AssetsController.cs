using Microsoft.AspNetCore.Mvc;
using tallerV1.Data;
using tallerV1.Models;

namespace tallerV1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AssetsController : ControllerBase
{
    private readonly IWorkshopRepository _repository;

    public AssetsController(IWorkshopRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Asset>> Get([FromQuery] string? empresa, [FromQuery] string? sede)
    {
        var assets = _repository.GetAssets(empresa, sede);
        return Ok(assets);
    }

    [HttpGet("{id:guid}")]
    public ActionResult<Asset> GetById(Guid id)
    {
        var asset = _repository.GetAsset(id);
        return asset is null ? NotFound() : Ok(asset);
    }

    [HttpPost]
    public ActionResult<Asset> Create([FromBody] Asset asset)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var created = _repository.AddAsset(asset);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPatch("{id:guid}/uso")]
    public IActionResult UpdateUsage(Guid id, [FromBody] UsageUpdateRequest request)
    {
        if (request.Lectura < 0)
        {
            return BadRequest("La lectura debe ser mayor o igual a cero");
        }

        var updated = _repository.UpdateUsage(id, request.Lectura);
        return updated ? NoContent() : NotFound();
    }

    [HttpGet("{id:guid}/bitacora")]
    public ActionResult<IEnumerable<BitacoraEntry>> GetBitacora(Guid id)
    {
        var asset = _repository.GetAsset(id);
        if (asset is null)
        {
            return NotFound();
        }

        return Ok(_repository.GetBitacora(id));
    }
}

public class UsageUpdateRequest
{
    public double Lectura { get; set; }
}
