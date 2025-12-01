namespace tallerV1.Models.DTOs
{
    public class DashboardDTO
    {
        public EstadisticasGeneralesDTO EstadisticasGenerales { get; set; } = new EstadisticasGeneralesDTO();
        public List<OrdenPorEstadoDTO> OrdenesPorEstado { get; set; } = new List<OrdenPorEstadoDTO>();
        public List<EquipoConMasFallasDTO> EquiposConMasFallas { get; set; } = new List<EquipoConMasFallasDTO>();
        public List<RepuestoBajoStockDTO> RepuestosBajoStock { get; set; } = new List<RepuestoBajoStockDTO>();
        public List<MantenimientoProximoDTO> MantenimientosProximos { get; set; } = new List<MantenimientoProximoDTO>();
    }

    public class EstadisticasGeneralesDTO
    {
        public int TotalEquipos { get; set; }
        public int EquiposActivos { get; set; }
        public int TotalOrdenes { get; set; }
        public int OrdenesAbiertas { get; set; }
        public int OrdenesCompletadas { get; set; }
        public decimal CostoTotalMes { get; set; }
        public int TotalRepuestos { get; set; }
        public int RepuestosBajoStock { get; set; }
        public decimal DisponibilidadFlota { get; set; }
    }

    public class OrdenPorEstadoDTO
    {
        public string Estado { get; set; } = string.Empty;
        public int Cantidad { get; set; }
    }

    public class EquipoConMasFallasDTO
    {
        public int EquipoId { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public int CantidadFallas { get; set; }
    }

    public class RepuestoBajoStockDTO
    {
        public int RepuestoId { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public int StockActual { get; set; }
        public int StockMinimo { get; set; }
    }

    public class MantenimientoProximoDTO
    {
        public int EquipoId { get; set; }
        public string EquipoCodigo { get; set; } = string.Empty;
        public string EquipoNombre { get; set; } = string.Empty;
        public string TipoMantenimiento { get; set; } = string.Empty;
        public DateTime? FechaProxima { get; set; }
        public decimal? KilometrajeProximo { get; set; }
        public decimal? HorasProximas { get; set; }
        public int DiasRestantes { get; set; }
    }
}
