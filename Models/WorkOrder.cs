using System.ComponentModel.DataAnnotations;

namespace tallerV1.Models;

public enum WorkOrderType
{
    Preventiva,
    Correctiva,
    Predictiva
}

public enum WorkOrderStatus
{
    Borrador,
    Aprobada,
    EnProgreso,
    EnEspera,
    QA,
    Cerrada,
    Cancelada
}

public enum WorkOrderPriority
{
    Critica,
    Alta,
    Media,
    Baja
}

public class WorkOrder
{
    public Guid Id { get; init; } = Guid.NewGuid();

    [Required]
    public Guid ActivoId { get; set; }

    [Required]
    [MaxLength(140)]
    public string Titulo { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Descripcion { get; set; }

    public WorkOrderType Tipo { get; set; }

    public WorkOrderStatus Estado { get; set; } = WorkOrderStatus.Borrador;

    public WorkOrderPriority Prioridad { get; set; } = WorkOrderPriority.Media;

    [MaxLength(80)]
    public string? Sede { get; set; }

    public DateTime? FechaProgramada { get; set; }

    public string? TecnicoAsignado { get; set; }

    public double? HorasEstimadas { get; set; }

    public double? HorasReales { get; set; }

    public List<WorkOrderNote> Observaciones { get; set; } = new();

    public List<ChecklistResult> Checklist { get; set; } = new();

    public List<MaterialUsage> Materiales { get; set; } = new();

    [MaxLength(80)]
    public string? CentroCostoId { get; set; }

    [MaxLength(40)]
    public string? Bahia { get; set; }

    [MaxLength(40)]
    public string? Turno { get; set; }

    public DateTime? FechaIngresoReal { get; set; }

    public DateTime? FechaCierre { get; set; }

    public List<LaborTimeEntry> ManoObra { get; set; } = new();

    public List<BitacoraEntry> Bitacora { get; set; } = new();
}

public class WorkOrderNote
{
    public DateTime Fecha { get; init; } = DateTime.UtcNow;

    [MaxLength(80)]
    public string Autor { get; set; } = string.Empty;

    [MaxLength(500)]
    public string Contenido { get; set; } = string.Empty;
}

public class MaterialUsage
{
    [Required]
    [MaxLength(80)]
    public string CodigoInterno { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Descripcion { get; set; } = string.Empty;

    [Range(0.01, 999999)]
    public decimal Cantidad { get; set; }

    [Range(0, 9999999)]
    public decimal CostoUnitario { get; set; }
}

public class ChecklistResult
{
    [Required]
    [MaxLength(120)]
    public string Nombre { get; set; } = string.Empty;

    public List<ChecklistItemResult> Items { get; set; } = new();

    public bool SolicitaAlerta { get; set; }
}

public class ChecklistItemResult
{
    [Required]
    [MaxLength(120)]
    public string Nombre { get; set; } = string.Empty;

    [MaxLength(300)]
    public string? Observaciones { get; set; }

    public ChecklistItemStatus Estado { get; set; }
}

public enum ChecklistItemStatus
{
    Aprobado,
    Observacion,
    Falla
}
