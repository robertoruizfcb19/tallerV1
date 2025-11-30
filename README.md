# tallerV1

Guía breve para subir este backend de ejemplo a un repositorio de GitHub sin base de datos.

## Pasos para publicar el código en GitHub
1. Inicia sesión en GitHub y crea un repositorio vacío (sin README ni .gitignore).
2. En tu máquina local, verifica que estés en la rama actual que deseas publicar y que los cambios estén confirmados.
3. Agrega el remoto a ese nuevo repositorio:
   ```bash
   git remote add origin https://github.com/<tu-usuario>/<nuevo-repo>.git
   ```
4. Si el repositorio remoto está vacío, sube toda la historia con la rama principal actual:
   ```bash
   git push -u origin $(git branch --show-current)
   ```
5. Si GitHub ya tiene la rama principal y solo quieres enviar commits nuevos, actualiza el remoto con:
   ```bash
   git push
   ```
6. Para cambios futuros:
   - Crea una rama de trabajo: `git checkout -b feature/nombre-del-cambio`.
   - Realiza tus modificaciones y confirma: `git commit -am "Mensaje del commit"`.
   - Sube la rama: `git push -u origin feature/nombre-del-cambio`.
   - Crea el Pull Request en GitHub y, tras aprobarse, haz merge.

## Buenas prácticas mínimas
- Usa mensajes de commit claros en español que expliquen el cambio.
- Revisa el estado antes de subir: `git status` y, si es posible, ejecuta `dotnet build` para validar.
- Documenta en el PR si hubo pruebas que no pudiste ejecutar por falta de entorno.

## Contexto actual
Este proyecto es un backend en memoria (sin base de datos) para la gestión de taller automotriz/agrícola. Incluye control de activos, órdenes de trabajo, checklist, agenda, bitácora, mano de obra y almacenes, pero usa datos semilla y se ejecuta como API ASP.NET Core.

## Modelos actuales (referencia rápida para esquema SQL)
El backend no usa base de datos todavía, pero estos son los modelos en `Models/` que deberías mapear a tablas cuando generes tu script SQL:

### Activos (`Asset`)
- `Id` (GUID, PK)
- `Nombre` (nvarchar(120), requerido)
- `Empresa` (nvarchar(80), requerido)
- `Sede` (nvarchar(80), requerido)
- `ActivoPadreId` (GUID, FK opcional para árbol de activos)
- `Tipo` (`AssetType`: Liviano, Pesado, Tractor, Retroexcavadora, Plataforma, BombaRiego, Generador, Otro)
- `UnidadUso` (`UsageUnit`: Kilometers u Hours)
- `UltimaLecturaUso` (float)
- `Criticidad` (int, 1-5)
- `CodigoInterno` (nvarchar(100))
- `NumeroSerie` (nvarchar(100))
- `UbicacionActual` (nvarchar(200))
- `CentroCostoId` (nvarchar(80))

### Órdenes de trabajo (`WorkOrder`)
- `Id` (GUID, PK)
- `ActivoId` (GUID, FK a `Asset`, requerido)
- `Titulo` (nvarchar(140), requerido)
- `Descripcion` (nvarchar(500))
- `Tipo` (`WorkOrderType`: Preventiva, Correctiva, Predictiva)
- `Estado` (`WorkOrderStatus`: Borrador, Aprobada, EnProgreso, EnEspera, QA, Cerrada, Cancelada)
- `Prioridad` (`WorkOrderPriority`: Critica, Alta, Media, Baja)
- `Sede` (nvarchar(80))
- `FechaProgramada` (datetime)
- `TecnicoAsignado` (nvarchar(100) sugerido)
- `HorasEstimadas` (float)
- `HorasReales` (float)
- `CentroCostoId` (nvarchar(80))
- `Bahia` (nvarchar(40))
- `Turno` (nvarchar(40))
- `FechaIngresoReal` (datetime)
- `FechaCierre` (datetime)

Relaciones hijas (tablas separadas con FK a `WorkOrder.Id`):
- `WorkOrderNote`: `Fecha` (datetime), `Autor` (nvarchar(80)), `Contenido` (nvarchar(500)).
- `ChecklistResult`: `Nombre` (nvarchar(120)), `SolicitaAlerta` (bit). Hija: `ChecklistItemResult` con `Nombre` (nvarchar(120)), `Observaciones` (nvarchar(300)), `Estado` (`ChecklistItemStatus`: Aprobado, Observacion, Falla).
- `MaterialUsage`: `CodigoInterno` (nvarchar(80)), `Descripcion` (nvarchar(200)), `Cantidad` (decimal), `CostoUnitario` (decimal).
- `LaborTimeEntry`: `Tecnico` (nvarchar(80)), `Horas` (float), `TarifaHora` (decimal), `Fecha` (datetime), `CostoCalculado` puede ser columna calculada.
- `BitacoraEntry`: `ActivoId` (GUID), `Descripcion` (nvarchar(500)), `Autor` (nvarchar(80)), `Fecha` (datetime).

### Plantillas y operación (`Operations`)
- `User`: `Id` (GUID), `Nombre` (nvarchar(100)), `Rol` (`UserRole`: Administrador, Coordinador, Tecnico, Consulta, Almacen).
- `ChecklistTemplate`: `Id` (GUID), `Nombre` (nvarchar(120)), `TipoActivo` (`AssetType`). Hija `ChecklistTemplateItem`: `Nombre` (nvarchar(120)), `Sistema` (nvarchar(40)), `Indicaciones` (nvarchar(200)).
- `AgendaSlot`: `Id` (GUID), `ActivoId` (GUID), `WorkOrderId` (GUID), `Sede` (nvarchar(80)), `Bahia` (nvarchar(40)), `Turno` (nvarchar(40)), `Inicio`/`Fin` (datetime).
- `Warehouse`: `Id` (nvarchar(50) sugerido), `Nombre` (nvarchar(100)), `Sede` (nvarchar(80)), `EsCentral` (bit). Hija `StockItem`: `CodigoInterno` (nvarchar(80)), `Descripcion` (nvarchar(200)), `Cantidad` (decimal), `StockMinimo` (decimal), `UnidadMedida` (nvarchar(40)).
- `StockAlert`: `CodigoInterno` (nvarchar(80)), `Mensaje` (nvarchar(300)).
- `NotificationMessage`: `Id` (GUID), `Evento` (nvarchar(120)), `Detalle` (nvarchar(500)), `Fecha` (datetime).

Con este listado puedes generar el script SQL inicial ajustando tipos y longitudes según tu SGBD (SQL Server, PostgreSQL, etc.).
