namespace tallerV1.Models.Enums
{
    public enum EstadoOrdenEnum
    {
        Recepcionada = 1,
        EnDiagnostico = 2,
        Presupuestada = 3,
        AprobacionPendiente = 4,
        Aprobada = 5,
        EnEjecucion = 6,
        EnAlerta = 7, // Para equipos que tienen diagnóstico pero no hay repuesto pero siguen funcionando
        Completada = 8,
        Entregada = 9,
        Cancelada = 10
    }
}
