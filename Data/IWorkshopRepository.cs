using tallerV1.Models;

namespace tallerV1.Data;

public interface IWorkshopRepository
{
    IEnumerable<Asset> GetAssets(string? empresa = null, string? sede = null);
    Asset? GetAsset(Guid id);
    Asset AddAsset(Asset asset);
    bool UpdateUsage(Guid assetId, double lectura);

    IEnumerable<WorkOrder> GetWorkOrders(WorkOrderStatus? estado = null, WorkOrderType? tipo = null);
    WorkOrder? GetWorkOrder(Guid id);
    WorkOrder AddWorkOrder(WorkOrder workOrder);
    bool TryUpdateWorkOrderStatus(Guid id, WorkOrderStatus nuevoEstado, string? autor, string? comentario);
    bool TryAddChecklist(Guid id, ChecklistResult checklist);
    bool TryAddMaterials(Guid id, IEnumerable<MaterialUsage> materials);

    IEnumerable<ChecklistTemplate> GetChecklistTemplates(AssetType? tipoActivo = null);
    ChecklistTemplate AddChecklistTemplate(ChecklistTemplate template);

    IEnumerable<AgendaSlot> GetAgenda(string? sede = null, DateTime? desde = null, DateTime? hasta = null);
    AgendaSlot? SchedulePreventive(Guid workOrderId, string sede, DateTime inicio, DateTime fin, string? bahia, string? turno);

    IEnumerable<Warehouse> GetWarehouses();
    Warehouse UpsertStock(string warehouseId, StockItem item);
    bool TryConsumeStock(Guid workOrderId, string warehouseId, IEnumerable<MaterialUsage> materials, out List<StockAlert> alerts);

    bool TryRegisterLabor(Guid workOrderId, LaborTimeEntry entry);

    IEnumerable<BitacoraEntry> GetBitacora(Guid assetId);
    BitacoraEntry AddBitacoraEntry(BitacoraEntry entry);

    IEnumerable<NotificationMessage> GetNotifications();
}
