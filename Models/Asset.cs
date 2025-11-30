using System.ComponentModel.DataAnnotations;

namespace tallerV1.Models;

public enum UsageUnit
{
    Kilometers,
    Hours
}

public enum AssetType
{
    Liviano,
    Pesado,
    Tractor,
    Retroexcavadora,
    Plataforma,
    BombaRiego,
    Generador,
    Otro
}

public class Asset
{
    public Guid Id { get; init; } = Guid.NewGuid();

    [Required]
    [MaxLength(120)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [MaxLength(80)]
    public string Empresa { get; set; } = string.Empty;

    [Required]
    [MaxLength(80)]
    public string Sede { get; set; } = string.Empty;

    public Guid? ActivoPadreId { get; set; }

    public AssetType Tipo { get; set; }

    public UsageUnit UnidadUso { get; set; }

    /// <summary>
    /// Valor de odómetro u horómetro reportado más reciente.
    /// </summary>
    public double UltimaLecturaUso { get; set; }

    /// <summary>
    /// Criticidad operativa para priorización.
    /// </summary>
    [Range(1, 5)]
    public int Criticidad { get; set; } = 3;

    [MaxLength(100)]
    public string? CodigoInterno { get; set; }

    [MaxLength(100)]
    public string? NumeroSerie { get; set; }

    [MaxLength(200)]
    public string? UbicacionActual { get; set; }

    [MaxLength(80)]
    public string? CentroCostoId { get; set; }
}
