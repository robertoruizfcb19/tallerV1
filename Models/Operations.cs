using System.ComponentModel.DataAnnotations;

namespace tallerV1.Models;

public enum UserRole
{
    Administrador,
    Coordinador,
    Tecnico,
    Consulta,
    Almacen
}

public class User
{
    public Guid Id { get; init; } = Guid.NewGuid();

    [Required]
    [MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    public UserRole Rol { get; set; }
}

public class ChecklistTemplate
{
    public Guid Id { get; init; } = Guid.NewGuid();

    [Required]
    [MaxLength(120)]
    public string Nombre { get; set; } = string.Empty;

    public AssetType TipoActivo { get; set; }

    public List<ChecklistTemplateItem> Items { get; set; } = new();
}

public class ChecklistTemplateItem
{
    [Required]
    [MaxLength(120)]
    public string Nombre { get; set; } = string.Empty;

    [MaxLength(40)]
    public string? Sistema { get; set; }

    [MaxLength(200)]
    public string? Indicaciones { get; set; }
}

public class AgendaSlot
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid ActivoId { get; set; }
    public Guid WorkOrderId { get; set; }

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

public class LaborTimeEntry
{
    public Guid Id { get; init; } = Guid.NewGuid();

    [Required]
    [MaxLength(80)]
    public string Tecnico { get; set; } = string.Empty;

    public double Horas { get; set; }

    public decimal TarifaHora { get; set; }

    public decimal CostoCalculado => (decimal)Horas * TarifaHora;

    public DateTime Fecha { get; set; } = DateTime.UtcNow;
}

public class BitacoraEntry
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public Guid ActivoId { get; set; }

    [MaxLength(500)]
    public string Descripcion { get; set; } = string.Empty;

    [MaxLength(80)]
    public string Autor { get; set; } = string.Empty;

    public DateTime Fecha { get; init; } = DateTime.UtcNow;
}

public class Warehouse
{
    public string Id { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string? Sede { get; set; }
    public bool EsCentral { get; set; }
    public List<StockItem> Stock { get; set; } = new();
}

public class StockItem
{
    [Required]
    [MaxLength(80)]
    public string CodigoInterno { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Descripcion { get; set; } = string.Empty;

    public decimal Cantidad { get; set; }
    public decimal StockMinimo { get; set; }

    public string? UnidadMedida { get; set; }
}

public class StockAlert
{
    public string CodigoInterno { get; set; } = string.Empty;
    public string Mensaje { get; set; } = string.Empty;
}

public class NotificationMessage
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Evento { get; set; } = string.Empty;
    public string Detalle { get; set; } = string.Empty;
    public DateTime Fecha { get; init; } = DateTime.UtcNow;
}
