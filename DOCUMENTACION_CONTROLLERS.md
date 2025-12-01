# 📡 Documentación Completa de Controllers - Sistema de Gestión de Taller Agrícola

## ✅ ESTADO: TODOS LOS CONTROLLERS IMPLEMENTADOS Y COMPILANDO EXITOSAMENTE

---

## 📋 ÍNDICE DE CONTROLLERS

1. [AuthController](#1-authcontroller) - Autenticación y Login
2. [EmpresasController](#2-empresascontroller) - Gestión de Empresas
3. [UsuariosController](#3-usuarioscontroller) - Gestión de Usuarios
4. [EquiposController](#4-equiposcontroller) - Gestión de Equipos/Vehículos
5. [OrdenesTrabajoController](#5-ordenesTrabajocontroller) - Órdenes de Trabajo
6. [RepuestosController](#6-repuestoscontroller) - Inventario de Repuestos
7. [BodegasController](#7-bodegascontroller) - Gestión de Bodegas
8. [MantenimientosPreventivosController](#8-mantenimientospreventivoscontroller) - Mantenimientos Preventivos
9. [NotificacionesController](#9-notificacionescontroller) - Sistema de Notificaciones
10. [HistorialEquiposController](#10-historialequiposcontroller) - Historial de Equipos
11. [DashboardController](#11-dashboardcontroller) - Dashboard y Estadísticas
12. [ReportesController](#12-reportescontroller) - Reportes y Análisis

---

## 1. AuthController

**Ruta base:** `/api/auth`
**Autenticación:** No requiere (público)

### Endpoints:

#### POST `/api/auth/login`
Iniciar sesión en el sistema

**Request:**
```json
{
  "username": "admin",
  "password": "Admin123!"
}
```

**Response:**
```json
{
  "success": true,
  "message": "Login exitoso",
  "data": {
    "usuarioId": 1,
    "username": "admin",
    "nombreCompleto": "Administrador del Sistema",
    "email": "admin@taller.com",
    "rol": "Administrador",
    "empresaId": 1,
    "empresaNombre": "Top Green S.A",
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "fechaExpiracion": "2025-12-01T19:17:00"
  }
}
```

#### GET `/api/auth/health`
Verificar estado del servidor

**Response:**
```json
{
  "success": true,
  "message": "API funcionando correctamente",
  "data": {
    "status": "OK",
    "timestamp": "2025-12-01T11:17:00"
  }
}
```

---

## 2. EmpresasController

**Ruta base:** `/api/empresas`
**Autenticación:** JWT requerido

### Endpoints:

#### GET `/api/empresas`
Obtener todas las empresas

**Roles permitidos:** Todos

#### GET `/api/empresas/{id}`
Obtener empresa por ID

#### POST `/api/empresas`
Crear nueva empresa

**Roles permitidos:** Administrador

**Request:**
```json
{
  "nombre": "Nueva Empresa S.A",
  "ruc": "1234567890",
  "direccion": "Calle Principal 123",
  "telefono": "555-1234",
  "email": "contacto@empresa.com"
}
```

#### PUT `/api/empresas/{id}`
Actualizar empresa

**Roles permitidos:** Administrador

#### DELETE `/api/empresas/{id}`
Desactivar empresa (soft delete)

**Roles permitidos:** Administrador

---

## 3. UsuariosController

**Ruta base:** `/api/usuarios`
**Autenticación:** JWT requerido

### Endpoints:

#### GET `/api/usuarios`
Obtener todos los usuarios

**Roles permitidos:** Administrador, Coordinador

#### GET `/api/usuarios/{id}`
Obtener usuario por ID

#### POST `/api/usuarios`
Crear nuevo usuario

**Roles permitidos:** Administrador, Coordinador

**Request:**
```json
{
  "nombreCompleto": "Juan Pérez",
  "email": "juan@taller.com",
  "username": "jperez",
  "password": "Password123!",
  "rol": 3,
  "empresaId": 1,
  "telefono": "555-5678"
}
```

#### PUT `/api/usuarios/{id}`
Actualizar usuario

**Roles permitidos:** Administrador, Coordinador

#### POST `/api/usuarios/{id}/cambiar-password`
Cambiar contraseña

**Request:**
```json
{
  "passwordActual": "Password123!",
  "passwordNuevo": "NewPassword456!",
  "confirmarPassword": "NewPassword456!"
}
```

#### DELETE `/api/usuarios/{id}`
Desactivar usuario

**Roles permitidos:** Administrador

#### GET `/api/usuarios/por-rol/{rol}`
Obtener usuarios por rol

**Roles permitidos:** Administrador, Coordinador

---

## 4. EquiposController

**Ruta base:** `/api/equipos`
**Autenticación:** JWT requerido

### Endpoints:

#### GET `/api/equipos`
Obtener todos los equipos

**Query params:**
- `empresaId`: Filtrar por empresa
- `tipoEquipo`: Filtrar por tipo (1-5)

#### GET `/api/equipos/{id}`
Obtener equipo por ID

#### POST `/api/equipos`
Crear nuevo equipo

**Roles permitidos:** Administrador, Coordinador

**Request:**
```json
{
  "codigo": "EQ-001",
  "nombre": "Tractor John Deere",
  "tipoEquipo": 3,
  "marca": "John Deere",
  "modelo": "5075E",
  "numeroSerie": "1234567890",
  "placa": "ABC123",
  "anioFabricacion": 2020,
  "empresaId": 1,
  "tipoControl": 2,
  "horasActuales": 500.5,
  "galonesPorHora": 3.5,
  "observaciones": "Equipo en buen estado"
}
```

#### PUT `/api/equipos/{id}`
Actualizar equipo

**Roles permitidos:** Administrador, Coordinador

#### DELETE `/api/equipos/{id}`
Desactivar equipo

**Roles permitidos:** Administrador

#### PUT `/api/equipos/{id}/actualizar-kilometraje`
Actualizar kilometraje del equipo

**Roles permitidos:** Administrador, Coordinador, Mecánico

**Request Body:** `decimal` (número de kilometraje)

#### PUT `/api/equipos/{id}/actualizar-horas`
Actualizar horas del equipo

**Roles permitidos:** Administrador, Coordinador, Mecánico

**Request Body:** `decimal` (número de horas)

---

## 5. OrdenesTrabajoController

**Ruta base:** `/api/ordenestrabajo`
**Autenticación:** JWT requerido

### Endpoints:

#### GET `/api/ordenestrabajo`
Obtener todas las órdenes de trabajo

**Query params:**
- `estado`: Filtrar por estado
- `equipoId`: Filtrar por equipo

#### GET `/api/ordenestrabajo/{id}`
Obtener orden de trabajo por ID

#### POST `/api/ordenestrabajo`
Crear nueva orden de trabajo

**Request:**
```json
{
  "equipoId": 1,
  "tipoMantenimiento": 2,
  "prioridad": 3,
  "descripcionProblema": "Motor presenta ruidos extraños",
  "kilometrajeEquipo": 15000,
  "horometroEquipo": 1500
}
```

**Response:** Genera automáticamente número de orden (OT-YYYYMMDD-XXXX)

#### PUT `/api/ordenestrabajo/{id}`
Actualizar orden de trabajo

**Roles permitidos:** Administrador, Coordinador, Mecánico

**Request:**
```json
{
  "usuarioAsignadoId": 2,
  "prioridad": 4,
  "estado": 6,
  "diagnosticoTecnico": "Se requiere cambio de correa",
  "costoRepuestos": 150.00,
  "costoManoObra": 80.00,
  "costoServiciosExternos": 0,
  "horasHombre": 4.5,
  "observacionesFinales": "Trabajo completado exitosamente"
}
```

#### POST `/api/ordenestrabajo/{id}/asignar`
Asignar orden a un técnico

**Roles permitidos:** Administrador, Coordinador

**Request:**
```json
{
  "usuarioAsignadoId": 2
}
```

#### POST `/api/ordenestrabajo/{id}/aprobar`
Aprobar o rechazar orden

**Roles permitidos:** Administrador, Coordinador

**Request:**
```json
{
  "aprobada": true,
  "observaciones": "Aprobado para ejecución"
}
```

---

## 6. RepuestosController

**Ruta base:** `/api/repuestos`
**Autenticación:** JWT requerido

### Endpoints:

#### GET `/api/repuestos`
Obtener todos los repuestos

**Query params:**
- `bodegaId`: Filtrar por bodega
- `stockBajo`: true para mostrar solo repuestos con stock bajo

#### GET `/api/repuestos/{id}`
Obtener repuesto por ID

#### POST `/api/repuestos`
Crear nuevo repuesto

**Roles permitidos:** Administrador, Coordinador, Bodeguero

**Request:**
```json
{
  "codigo": "REP-001",
  "codigoBarras": "1234567890123",
  "nombre": "Filtro de aceite",
  "descripcion": "Filtro para motor diesel",
  "marca": "Mann Filter",
  "unidadMedida": "Unidad",
  "bodegaId": 1,
  "stockActual": 50,
  "stockMinimo": 10,
  "stockMaximo": 100,
  "precioUnitario": 25.50,
  "mesesGarantia": 6,
  "proveedor": "Repuestos S.A"
}
```

#### PUT `/api/repuestos/{id}`
Actualizar repuesto

**Roles permitidos:** Administrador, Coordinador, Bodeguero

#### POST `/api/repuestos/movimiento`
Registrar movimiento de inventario

**Roles permitidos:** Administrador, Coordinador, Bodeguero

**Request:**
```json
{
  "repuestoId": 1,
  "tipoMovimiento": "Salida",
  "cantidad": 5,
  "observaciones": "Usado en orden OT-20251201-0001"
}
```

**Tipos de movimiento válidos:**
- `Entrada`: Incrementa stock
- `Salida`: Decrementa stock
- `Ajuste`: Establece stock a cantidad específica

#### GET `/api/repuestos/{id}/movimientos`
Obtener historial de movimientos del repuesto

---

## 7. BodegasController

**Ruta base:** `/api/bodegas`
**Autenticación:** JWT requerido

### Endpoints:

#### GET `/api/bodegas`
Obtener todas las bodegas

#### GET `/api/bodegas/{id}`
Obtener bodega por ID

#### POST `/api/bodegas`
Crear nueva bodega

**Roles permitidos:** Administrador

**Request:**
```json
{
  "nombre": "Bodega Central",
  "ubicacion": "Planta Principal",
  "responsable": "Carlos Méndez"
}
```

#### PUT `/api/bodegas/{id}`
Actualizar bodega

**Roles permitidos:** Administrador

---

## 8. MantenimientosPreventivosController

**Ruta base:** `/api/mantenimientospreventivos`
**Autenticación:** JWT requerido

### Endpoints:

#### GET `/api/mantenimientospreventivos`
Obtener todos los mantenimientos preventivos

**Query params:**
- `equipoId`: Filtrar por equipo

#### GET `/api/mantenimientospreventivos/{id}`
Obtener mantenimiento preventivo por ID

#### POST `/api/mantenimientospreventivos`
Crear nuevo mantenimiento preventivo

**Roles permitidos:** Administrador, Coordinador

**Request:**
```json
{
  "nombre": "Mantenimiento 500 horas",
  "descripcion": "Mantenimiento programado cada 500 horas de operación",
  "equipoId": 1,
  "tipoControl": 2,
  "intervaloHoras": 500,
  "tareas": [
    {
      "descripcion": "Cambio de aceite de motor",
      "orden": 1
    },
    {
      "descripcion": "Cambio de filtros",
      "orden": 2
    },
    {
      "descripcion": "Revisión de frenos",
      "orden": 3
    }
  ]
}
```

#### PUT `/api/mantenimientospreventivos/{id}`
Actualizar mantenimiento preventivo

**Roles permitidos:** Administrador, Coordinador

---

## 9. NotificacionesController

**Ruta base:** `/api/notificaciones`
**Autenticación:** JWT requerido

### Endpoints:

#### GET `/api/notificaciones/mis-notificaciones`
Obtener notificaciones del usuario actual

**Query params:**
- `soloNoLeidas`: true para mostrar solo no leídas

#### POST `/api/notificaciones/{id}/marcar-leida`
Marcar notificación como leída

#### POST `/api/notificaciones/marcar-todas-leidas`
Marcar todas las notificaciones como leídas

#### GET `/api/notificaciones/contador-no-leidas`
Obtener cantidad de notificaciones no leídas

---

## 10. HistorialEquiposController

**Ruta base:** `/api/historialequipos`
**Autenticación:** JWT requerido

### Endpoints:

#### GET `/api/historialequipos/equipo/{equipoId}`
Obtener historial completo de un equipo

**Response:**
```json
{
  "success": true,
  "data": [
    {
      "id": 1,
      "tipoEvento": "Mantenimiento",
      "descripcion": "Mantenimiento preventivo 500 horas",
      "usuarioNombre": "Juan Pérez",
      "fechaEvento": "2025-11-15T10:30:00",
      "ordenTrabajoNumero": "OT-20251115-0001"
    }
  ]
}
```

---

## 11. DashboardController

**Ruta base:** `/api/dashboard`
**Autenticación:** JWT requerido

### Endpoints:

#### GET `/api/dashboard`
Obtener datos completos del dashboard

**Response incluye:**
- Estadísticas generales
- Órdenes por estado
- Equipos con más fallas (top 5)
- Repuestos bajo stock (top 10)
- Mantenimientos próximos (próximos 30 días)

**Response:**
```json
{
  "success": true,
  "data": {
    "estadisticasGenerales": {
      "totalEquipos": 100,
      "equiposActivos": 95,
      "totalOrdenes": 250,
      "ordenesAbiertas": 15,
      "ordenesCompletadas": 200,
      "costoTotalMes": 15000.50,
      "totalRepuestos": 500,
      "repuestosBajoStock": 12,
      "disponibilidadFlota": 85.5
    },
    "ordenesPorEstado": [...],
    "equiposConMasFallas": [...],
    "repuestosBajoStock": [...],
    "mantenimientosProximos": [...]
  }
}
```

#### GET `/api/dashboard/estadisticas-mensuales`
Obtener estadísticas mensuales por año

**Query params:**
- `anio`: Año a consultar (requerido)

---

## 12. ReportesController

**Ruta base:** `/api/reportes`
**Autenticación:** JWT requerido
**Roles permitidos:** Administrador, Coordinador, Visualizador

### Endpoints:

#### GET `/api/reportes/ordenes-por-periodo`
Reporte de órdenes por período

**Query params:**
- `fechaInicio`: Fecha inicial (requerido)
- `fechaFin`: Fecha final (requerido)

#### GET `/api/reportes/costos-por-equipo`
Reporte de costos por equipo

**Query params:**
- `fechaInicio`: Fecha inicial (opcional)
- `fechaFin`: Fecha final (opcional)

**Response:**
```json
{
  "success": true,
  "data": [
    {
      "equipoCodigo": "EQ-001",
      "equipoNombre": "Tractor John Deere",
      "totalOrdenes": 15,
      "costoTotal": 5500.00,
      "costoPromedio": 366.67,
      "costoRepuestos": 3000.00,
      "costoManoObra": 2500.00
    }
  ]
}
```

#### GET `/api/reportes/disponibilidad-flota`
Reporte de disponibilidad de la flota

**Response:**
```json
{
  "success": true,
  "data": {
    "totalEquipos": 100,
    "equiposDisponibles": 85,
    "equiposEnMantenimiento": 15,
    "porcentajeDisponibilidad": 85.00
  }
}
```

#### GET `/api/reportes/consumo-repuestos`
Reporte de consumo de repuestos

**Query params:**
- `fechaInicio`: Fecha inicial (requerido)
- `fechaFin`: Fecha final (requerido)

#### GET `/api/reportes/productividad-tecnicos`
Reporte de productividad de técnicos

**Query params:**
- `fechaInicio`: Fecha inicial (requerido)
- `fechaFin`: Fecha final (requerido)

**Response:**
```json
{
  "success": true,
  "data": [
    {
      "tecnicoNombre": "Juan Pérez",
      "totalOrdenes": 25,
      "ordenesCompletadas": 23,
      "ordenesEnProceso": 2,
      "tiempoPromedioHoras": 4.5
    }
  ]
}
```

#### GET `/api/reportes/mantenimientos-vencidos`
Reporte de mantenimientos preventivos vencidos

**Response:**
```json
{
  "success": true,
  "data": [
    {
      "equipoCodigo": "EQ-001",
      "equipoNombre": "Tractor John Deere",
      "mantenimientoNombre": "Mantenimiento 500 horas",
      "fechaVencimiento": "2025-11-01T00:00:00",
      "diasVencidos": 30
    }
  ]
}
```

---

## 🔒 SISTEMA DE ROLES Y PERMISOS

### Roles Disponibles:

1. **Administrador (1)** - Acceso completo
2. **Coordinador (2)** - Gestión operativa
3. **Mecánico (3)** - Ejecución de trabajo
4. **Bodeguero (4)** - Gestión de inventario
5. **Visualizador (5)** - Solo lectura de costos

### Matriz de Permisos:

| Endpoint | Admin | Coord | Mec | Bod | Vis |
|----------|-------|-------|-----|-----|-----|
| Auth | ✅ | ✅ | ✅ | ✅ | ✅ |
| Empresas (Crear/Editar) | ✅ | ❌ | ❌ | ❌ | ❌ |
| Usuarios (Crear/Editar) | ✅ | ✅ | ❌ | ❌ | ❌ |
| Equipos (Crear/Editar) | ✅ | ✅ | ❌ | ❌ | ❌ |
| Equipos (Actualizar KM/Horas) | ✅ | ✅ | ✅ | ❌ | ❌ |
| Órdenes (Crear) | ✅ | ✅ | ✅ | ✅ | ❌ |
| Órdenes (Actualizar) | ✅ | ✅ | ✅ | ❌ | ❌ |
| Órdenes (Aprobar) | ✅ | ✅ | ❌ | ❌ | ❌ |
| Repuestos (Gestión) | ✅ | ✅ | ❌ | ✅ | ❌ |
| Reportes | ✅ | ✅ | ❌ | ❌ | ✅ |

---

## 📊 RESPUESTA ESTÁNDAR DE API

Todos los endpoints retornan el siguiente formato:

**Éxito:**
```json
{
  "success": true,
  "message": "Mensaje descriptivo",
  "data": { /* datos */ }
}
```

**Error:**
```json
{
  "success": false,
  "message": "Mensaje de error",
  "errors": ["lista", "de", "errores"] // opcional
}
```

---

## 🎯 CÓDIGOS HTTP

- `200 OK` - Operación exitosa
- `201 Created` - Recurso creado
- `400 Bad Request` - Datos inválidos
- `401 Unauthorized` - No autenticado
- `403 Forbidden` - Sin permisos
- `404 Not Found` - Recurso no encontrado
- `500 Internal Server Error` - Error del servidor

---

## 📝 NOTAS IMPORTANTES

1. **Autenticación JWT:**
   - Token se incluye en header: `Authorization: Bearer {token}`
   - Tokens expiran en 8 horas (480 minutos)

2. **Validación de Roles:**
   - Se valida mediante atributo `[Authorize(Roles = "...")]`
   - Roles separados por coma para múltiples permisos

3. **Soft Delete:**
   - La mayoría de eliminaciones son "soft delete" (campo `Activo = false`)
   - Los registros no se borran físicamente

4. **Generación Automática:**
   - Números de orden: `OT-YYYYMMDD-XXXX`
   - Fechas de creación: `DateTime.Now`

5. **Cálculos Automáticos:**
   - `CostoTotal` = CostoRepuestos + CostoManoObra + CostoServiciosExternos
   - `StockBajo` = StockActual < StockMinimo

---

## ✅ ESTADO DEL PROYECTO

**COMPLETADO:**
- ✅ 12 Controllers implementados
- ✅ Autenticación JWT
- ✅ Sistema de roles y permisos
- ✅ DTOs completos
- ✅ Validaciones
- ✅ Proyecto compilando sin errores

**LISTO PARA:**
- ✅ Pruebas en Swagger
- ✅ Integración con frontend
- ✅ Pruebas de endpoints

**URL Swagger:** http://localhost:5042/swagger
