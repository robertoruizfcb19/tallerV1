using System.Collections.Concurrent;
using tallerV1.Models;

namespace tallerV1.Data;

public class WorkshopRepository : IWorkshopRepository
{
    private readonly ConcurrentDictionary<Guid, Asset> _assets = new();
    private readonly ConcurrentDictionary<Guid, WorkOrder> _orders = new();
    private readonly ConcurrentDictionary<Guid, BitacoraEntry> _bitacora = new();
    private readonly List<ChecklistTemplate> _templates = new();
    private readonly List<AgendaSlot> _agenda = new();
    private readonly List<Warehouse> _warehouses = new();
    private readonly List<NotificationMessage> _notifications = new();

    public WorkshopRepository()
    {
        Seed();
    }

    public Asset AddAsset(Asset asset)
    {
        _assets[asset.Id] = asset;
        AddBitacoraEntry(new BitacoraEntry
        {
            ActivoId = asset.Id,
            Autor = "sistema",
            Descripcion = "Activo creado"
        });
        return asset;
    }

    public bool UpdateUsage(Guid assetId, double lectura)
    {
        if (!_assets.TryGetValue(assetId, out var asset))
        {
            return false;
        }

        asset.UltimaLecturaUso = Math.Max(asset.UltimaLecturaUso, lectura);
        return true;
    }

    public Asset? GetAsset(Guid id) => _assets.TryGetValue(id, out var asset) ? asset : null;

    public IEnumerable<Asset> GetAssets(string? empresa = null, string? sede = null)
    {
        var assets = _assets.Values.AsEnumerable();
        if (!string.IsNullOrWhiteSpace(empresa))
        {
            assets = assets.Where(a => a.Empresa.Equals(empresa, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(sede))
        {
            assets = assets.Where(a => a.Sede.Equals(sede, StringComparison.OrdinalIgnoreCase));
        }

        return assets.OrderBy(a => a.Empresa).ThenBy(a => a.Sede).ThenBy(a => a.Nombre);
    }

    public WorkOrder AddWorkOrder(WorkOrder workOrder)
    {
        if (!_assets.ContainsKey(workOrder.ActivoId))
        {
            throw new ArgumentException("El activo no existe", nameof(workOrder.ActivoId));
        }

        workOrder.Estado = WorkOrderStatus.Borrador;
        _orders[workOrder.Id] = workOrder;
        return workOrder;
    }

    public WorkOrder? GetWorkOrder(Guid id) => _orders.TryGetValue(id, out var order) ? order : null;

    public IEnumerable<WorkOrder> GetWorkOrders(WorkOrderStatus? estado = null, WorkOrderType? tipo = null)
    {
        var orders = _orders.Values.AsEnumerable();

        if (estado.HasValue)
        {
            orders = orders.Where(o => o.Estado == estado.Value);
        }

        if (tipo.HasValue)
        {
            orders = orders.Where(o => o.Tipo == tipo.Value);
        }

        return orders.OrderByDescending(o => o.FechaProgramada ?? DateTime.MinValue);
    }

    public bool TryAddChecklist(Guid id, ChecklistResult checklist)
    {
        if (!_orders.TryGetValue(id, out var order))
        {
            return false;
        }

        order.Checklist.Add(checklist);
        if (checklist.SolicitaAlerta)
        {
            order.Observaciones.Add(new WorkOrderNote
            {
                Autor = "sistema",
                Contenido = $"Checklist {checklist.Nombre} marcó alerta"
            });
        }

        var fallas = checklist.Items.Count(i => i.Estado == ChecklistItemStatus.Falla);
        if (fallas >= 2)
        {
            _notifications.Add(new NotificationMessage
            {
                Evento = "Alerta predictiva",
                Detalle = $"WO {order.Id} ({order.Titulo}) tiene {fallas} fallas en checklist {checklist.Nombre}"
            });
        }

        return true;
    }

    public bool TryAddMaterials(Guid id, IEnumerable<MaterialUsage> materials)
    {
        if (!_orders.TryGetValue(id, out var order))
        {
            return false;
        }

        order.Materiales.AddRange(materials);
        return true;
    }

    public bool TryConsumeStock(Guid workOrderId, string warehouseId, IEnumerable<MaterialUsage> materials, out List<StockAlert> alerts)
    {
        alerts = new List<StockAlert>();

        if (!_orders.TryGetValue(workOrderId, out var order))
        {
            return false;
        }

        var warehouse = _warehouses.FirstOrDefault(w => w.Id.Equals(warehouseId, StringComparison.OrdinalIgnoreCase));
        if (warehouse is null)
        {
            return false;
        }

        foreach (var material in materials)
        {
            var stock = warehouse.Stock.FirstOrDefault(s => s.CodigoInterno.Equals(material.CodigoInterno, StringComparison.OrdinalIgnoreCase));
            if (stock is null)
            {
                stock = new StockItem
                {
                    CodigoInterno = material.CodigoInterno,
                    Descripcion = material.Descripcion,
                    Cantidad = 0,
                    StockMinimo = 0
                };
                warehouse.Stock.Add(stock);
            }

            stock.Cantidad -= material.Cantidad;
            order.Materiales.Add(material);

            if (stock.Cantidad <= stock.StockMinimo)
            {
                var alert = new StockAlert
                {
                    CodigoInterno = stock.CodigoInterno,
                    Mensaje = $"Stock bajo en {warehouse.Nombre}: {stock.Cantidad} por debajo de mínimo {stock.StockMinimo}"
                };
                alerts.Add(alert);
                _notifications.Add(new NotificationMessage
                {
                    Evento = "Stock mínimo",
                    Detalle = alert.Mensaje
                });
            }
        }

        return true;
    }

    public bool TryUpdateWorkOrderStatus(Guid id, WorkOrderStatus nuevoEstado, string? autor, string? comentario)
    {
        if (!_orders.TryGetValue(id, out var order))
        {
            return false;
        }

        order.Estado = nuevoEstado;

        if (!string.IsNullOrWhiteSpace(comentario))
        {
            order.Observaciones.Add(new WorkOrderNote
            {
                Autor = autor ?? "sistema",
                Contenido = comentario
            });
        }

        return true;
    }

    public IEnumerable<ChecklistTemplate> GetChecklistTemplates(AssetType? tipoActivo = null)
    {
        var templates = _templates.AsEnumerable();
        if (tipoActivo.HasValue)
        {
            templates = templates.Where(t => t.TipoActivo == tipoActivo.Value);
        }

        return templates.OrderBy(t => t.Nombre);
    }

    public ChecklistTemplate AddChecklistTemplate(ChecklistTemplate template)
    {
        _templates.Add(template);
        return template;
    }

    public IEnumerable<AgendaSlot> GetAgenda(string? sede = null, DateTime? desde = null, DateTime? hasta = null)
    {
        var agenda = _agenda.AsEnumerable();
        if (!string.IsNullOrWhiteSpace(sede))
        {
            agenda = agenda.Where(a => a.Sede.Equals(sede, StringComparison.OrdinalIgnoreCase));
        }

        if (desde.HasValue)
        {
            agenda = agenda.Where(a => a.Inicio >= desde.Value);
        }

        if (hasta.HasValue)
        {
            agenda = agenda.Where(a => a.Fin <= hasta.Value);
        }

        return agenda.OrderBy(a => a.Inicio);
    }

    public AgendaSlot? SchedulePreventive(Guid workOrderId, string sede, DateTime inicio, DateTime fin, string? bahia, string? turno)
    {
        if (!_orders.TryGetValue(workOrderId, out var order))
        {
            return null;
        }

        var slot = new AgendaSlot
        {
            ActivoId = order.ActivoId,
            WorkOrderId = workOrderId,
            Sede = sede,
            Inicio = inicio,
            Fin = fin,
            Bahia = bahia,
            Turno = turno
        };

        _agenda.Add(slot);
        order.Sede ??= sede;
        order.FechaProgramada = inicio;
        order.Bahia = bahia;
        order.Turno = turno;

        return slot;
    }

    public IEnumerable<Warehouse> GetWarehouses() => _warehouses;

    public Warehouse UpsertStock(string warehouseId, StockItem item)
    {
        var warehouse = _warehouses.FirstOrDefault(w => w.Id.Equals(warehouseId, StringComparison.OrdinalIgnoreCase));
        if (warehouse is null)
        {
            warehouse = new Warehouse { Id = warehouseId, Nombre = warehouseId };
            _warehouses.Add(warehouse);
        }

        var existing = warehouse.Stock.FirstOrDefault(s => s.CodigoInterno.Equals(item.CodigoInterno, StringComparison.OrdinalIgnoreCase));
        if (existing is null)
        {
            warehouse.Stock.Add(item);
        }
        else
        {
            existing.Cantidad = item.Cantidad;
            existing.Descripcion = item.Descripcion;
            existing.StockMinimo = item.StockMinimo;
            existing.UnidadMedida = item.UnidadMedida;
        }

        return warehouse;
    }

    public bool TryRegisterLabor(Guid workOrderId, LaborTimeEntry entry)
    {
        if (!_orders.TryGetValue(workOrderId, out var order))
        {
            return false;
        }

        order.ManoObra.Add(entry);
        return true;
    }

    public IEnumerable<BitacoraEntry> GetBitacora(Guid assetId)
    {
        return _bitacora.Values.Where(b => b.ActivoId == assetId).OrderByDescending(b => b.Fecha);
    }

    public BitacoraEntry AddBitacoraEntry(BitacoraEntry entry)
    {
        _bitacora[entry.Id] = entry;
        foreach (var order in _orders.Values.Where(o => o.ActivoId == entry.ActivoId))
        {
            order.Bitacora.Add(entry);
        }

        return entry;
    }

    public IEnumerable<NotificationMessage> GetNotifications() => _notifications.OrderByDescending(n => n.Fecha);

    private void Seed()
    {
        var activo1 = new Asset
        {
            Nombre = "Pickup liviano",
            Empresa = "Empresa A",
            Sede = "Guatemala",
            Tipo = AssetType.Liviano,
            UnidadUso = UsageUnit.Kilometers,
            Criticidad = 3,
            CodigoInterno = "VEH-001",
            UltimaLecturaUso = 11123,
            CentroCostoId = "CC-VEH-01"
        };

        var activo2 = new Asset
        {
            Nombre = "Tractor principal",
            Empresa = "Empresa B",
            Sede = "Escuintla",
            Tipo = AssetType.Tractor,
            UnidadUso = UsageUnit.Hours,
            Criticidad = 5,
            CodigoInterno = "AG-010",
            UltimaLecturaUso = 5200,
            CentroCostoId = "CC-AGR-01"
        };

        var activo3 = new Asset
        {
            Nombre = "Generador emergencia",
            Empresa = "Empresa C",
            Sede = "Quetzaltenango",
            Tipo = AssetType.Generador,
            UnidadUso = UsageUnit.Hours,
            Criticidad = 4,
            CodigoInterno = "GEN-003",
            UltimaLecturaUso = 180,
            CentroCostoId = "CC-GEN-02"
        };

        AddAsset(activo1);
        AddAsset(activo2);
        AddAsset(activo3);

        var orden = new WorkOrder
        {
            ActivoId = activo1.Id,
            Titulo = "PM cambio de aceite 10,000 km",
            Tipo = WorkOrderType.Preventiva,
            Prioridad = WorkOrderPriority.Media,
            Sede = activo1.Sede,
            FechaProgramada = DateTime.UtcNow.AddDays(2)
        };

        orden.Observaciones.Add(new WorkOrderNote
        {
            Autor = "coordinador",
            Contenido = "Ingresar en turno matutino"
        });

        AddWorkOrder(orden);

        SeedTemplates();
        SeedWarehouses();
    }

    private void SeedTemplates()
    {
        _templates.Add(new ChecklistTemplate
        {
            Nombre = "PM 10,000 km liviano",
            TipoActivo = AssetType.Liviano,
            Items =
            {
                new ChecklistTemplateItem { Nombre = "Cambio de aceite", Sistema = "Motor", Indicaciones = "Usar 5W-30" },
                new ChecklistTemplateItem { Nombre = "Filtro de aceite", Sistema = "Motor" },
                new ChecklistTemplateItem { Nombre = "Inspección frenos", Sistema = "Frenos" }
            }
        });

        _templates.Add(new ChecklistTemplate
        {
            Nombre = "PM 250h tractor",
            TipoActivo = AssetType.Tractor,
            Items =
            {
                new ChecklistTemplateItem { Nombre = "Cambio aceite motor", Sistema = "Motor" },
                new ChecklistTemplateItem { Nombre = "Filtro combustible", Sistema = "Combustible" },
                new ChecklistTemplateItem { Nombre = "Presión hidráulica", Sistema = "Hidráulico" }
            }
        });
    }

    private void SeedWarehouses()
    {
        _warehouses.Add(new Warehouse
        {
            Id = "BOD-CENTRAL",
            Nombre = "Bodega Central",
            EsCentral = true,
            Stock =
            {
                new StockItem { CodigoInterno = "ACE-5W30", Descripcion = "Aceite 5W-30", Cantidad = 50, StockMinimo = 20, UnidadMedida = "lt" },
                new StockItem { CodigoInterno = "FILT-ACE", Descripcion = "Filtro de aceite", Cantidad = 15, StockMinimo = 10 }
            }
        });

        _warehouses.Add(new Warehouse
        {
            Id = "BOD-GT",
            Nombre = "Bodega Guatemala",
            Sede = "Guatemala",
            EsCentral = false,
            Stock =
            {
                new StockItem { CodigoInterno = "ACE-5W30", Descripcion = "Aceite 5W-30", Cantidad = 10, StockMinimo = 5, UnidadMedida = "lt" },
                new StockItem { CodigoInterno = "FILT-ACE", Descripcion = "Filtro de aceite", Cantidad = 3, StockMinimo = 3 }
            }
        });
    }
}
