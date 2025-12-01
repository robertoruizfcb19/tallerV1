# tallerV1

Backend en memoria (ASP.NET Core) para la gestión de un taller automotriz/agrícola. No usa base de datos ni ORM; todos los datos viven en memoria con semillas de ejemplo y se reinician al reiniciar la aplicación.

## Alcance
- Control de activos con jerarquía (vehículos livianos/pesados, maquinaria agrícola, generadores, bombas, etc.).
- Órdenes de trabajo preventivas, correctivas y predictivas con estados, prioridad, agenda y bitácora.
- Checklists por tipo de activo/servicio, registro de mano de obra y consumo de materiales.
- Almacenes con stock mínimo y alertas para solicitar reposición.
- Notificaciones en memoria (p. ej. alertas de stock o checklist con fallas repetidas).
- Sin persistencia: ideal para explorar flujos y endpoints antes de conectar una base de datos.

## Ejecutar
1. Instala el SDK de .NET 8.x.
2. En la raíz del proyecto ejecuta:
   ```bash
   dotnet run
   ```
3. La API expone Swagger en `http://localhost:5217/swagger` (el puerto puede variar según tu entorno).

## Modelos clave
### Asset
Representa un activo del taller.
- Identidad y jerarquía: `Id`, `Nombre`, `ActivoPadreId`, `Empresa`, `Sede`, `Tipo` (liviano, pesado, tractor, retroexcavadora, plataforma, bomba de riego, generador u otro).
- Uso y criticidad: `UnidadUso` (km u horas), `UltimaLecturaUso`, `Criticidad`.
- Identificadores: `CodigoInterno`, `NumeroSerie`, `UbicacionActual`, `CentroCostoId`.

### WorkOrder
Orden de trabajo asociada a un activo.
- Datos base: `Id`, `ActivoId`, `Titulo`, `Descripcion`, `Tipo` (preventiva/correctiva/predictiva), `Estado`, `Prioridad`, `Sede`, `Bahia`, `Turno`, `FechaProgramada`.
- Seguimiento: `TecnicoAsignado`, `HorasEstimadas`, `HorasReales`, `FechaIngresoReal`, `FechaCierre`, `CentroCostoId`.
- Información relacionada:
  - Notas (`WorkOrderNote`).
  - Checklists (`ChecklistResult` con `ChecklistItemResult` para cada ítem y estado: Aprobado, Observacion, Falla). Las fallas repetidas generan alertas predictivas.
  - Materiales usados (`MaterialUsage`), que disparan alertas de stock si se baja del mínimo.
  - Mano de obra (`LaborTimeEntry` con horas, tarifa y técnico) para costeo interno.
  - Bitácora (`BitacoraEntry`) para registrar eventos relevantes del activo y la orden.

### Operations (operaciones auxiliares)
- Usuarios (`User`) con rol: Administrador, Coordinador, Tecnico, Consulta o Almacen.
- Plantillas de checklist (`ChecklistTemplate` e items) para estandarizar inspecciones por tipo de activo.
- Agenda (`AgendaSlot`) para asignar bahía/turno y programar preventivos o ingresos correctivos.
- Almacenes (`Warehouse` con `StockItem`) y alertas (`StockAlert`).
- Notificaciones (`NotificationMessage`) para eventos clave (PM vencido, stock bajo, checklist con fallas repetidas, cita confirmada).

## Endpoints principales
- **Assets** (`AssetsController`): listar/filtrar, crear activos, actualizar lecturas de uso, consultar bitácora.
- **WorkOrders** (`WorkOrdersController`): crear/filtrar, cambiar estado, asignar agenda, registrar checklist, mano de obra y materiales.
- **Inventory** (`InventoryController`): consultar stock por bodega, actualizar cantidades, ver alertas de stock.
- **Operations** (`OperationsController`): obtener plantillas de checklist, agenda programada y notificaciones generadas.

### Flujo típico
1. Registrar o buscar un activo.
2. Crear una orden (preventiva/correctiva/predictiva) y asignar prioridad/sede/bahía/turno.
3. Registrar checklist, mano de obra y materiales; las alertas se generan en memoria según reglas de stock y fallas repetidas.
4. Cambiar estado hasta cierre y consultar la bitácora/notificaciones para seguimiento.

> Nota: Al no haber persistencia, este backend es ideal para probar la lógica de negocio y los contratos de API antes de conectar almacenamiento real.
